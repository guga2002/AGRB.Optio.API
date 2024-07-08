using Accord.Neuro;
using Accord.Neuro.Learning;
using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class AiRepository : AbstractRepositroy<Transaction>, IAiRepository
    {
        private readonly ActivationNetwork network;

        public AiRepository(OptioDB db) : base(db)
        {
            this.network = new ActivationNetwork(new SigmoidFunction(), 6, 10, 1);
        }

        public async Task TrainModel()
        {
            await PrepareDataForTraining();

            var inputs = new List<double[]>();
            var outputs = new List<double[]>();

            foreach (var item in await Dbset.ToListAsync())
            {
                var input = GetTransactionInput(item);
                var output = GetTransactionOutput(item);

                inputs.Add(input);
                outputs.Add(output);
            }

            double[][] inputsArray = inputs.ToArray();
            double[][] outputsArray = outputs.ToArray();

            new NguyenWidrow(network).Randomize();

            var teacher = new BackPropagationLearning(network)
            {
                LearningRate = 0.03,
                Momentum = 0.9,
            };

            for (int i = 0; i < 1000; i++)
            {
                double error = teacher.RunEpoch(inputsArray, outputsArray);
                if (i % 100 == 0)
                {
                    Console.WriteLine($"Epoch {i}, Error: {error}");
                }
            }
        }

        private double[] GetTransactionInput(Transaction transaction)
        {
            return new double[]
            {
                (double)transaction.Amount,
                transaction.MerchantId,
                transaction.CategoryId,
                (double)transaction.AmountEquivalent,
                transaction.ChannelId,
                transaction.CurrencyId
            };
        }

        private double[] GetTransactionOutput(Transaction transaction)
        {
            return new double[]
            {
                transaction.Fraudable ? 1.0 : 0.0
            };
        }

        private async Task PrepareDataForTraining()
        {
            var transactions = await Dbset
                .Where(t =>
                    (t.Amount > 1000 || t.Amount < 2) ||
                    (t.AmountEquivalent > 1000 || t.AmountEquivalent < 2) ||
                    t.Date < DateTime.Now.AddMonths(-5) ||
                    t.Date > DateTime.Now
                )
                .ToListAsync();

            foreach (var transaction in transactions)
            {
                AdjustFraudStatus(transaction);

                var currency = await Context.Currencies
                    .Include(c => c.Courses)
                    .FirstOrDefaultAsync(c => c.Id == transaction.CurrencyId);

                ValidateCurrencyConversion(transaction, currency);

                if (transaction.MerchantId == 0 || transaction.CategoryId == 0 || transaction.ChannelId == 0)
                {
                    transaction.Fraudable = true;
                }
            }

            await Context.SaveChangesAsync();
        }

        private void AdjustFraudStatus(Transaction transaction)
        {
            double percente = 0;

            if (transaction.Amount > 100000 || transaction.Amount < 2)
            {
                percente += 10; // amount is too much
            }

            if (transaction.AmountEquivalent > 100000 || transaction.AmountEquivalent < 2)
            {
                percente += 10; // equivalent amount is too much
            }

            if (transaction.Date < DateTime.Now.AddMonths(-10))
            {
                percente += 10; // bad data (date)
            }

            if (transaction.Date > DateTime.Now)
            {
                percente += 10; // ambiguous date
            }

            if (percente > 30 && percente <= 60)
            {
                transaction.IsSuspecisious = true;
            }
            else
            {
                transaction.IsSuspecisious = false;
            }

            if (percente >= 60)
            {
                transaction.Fraudable = true;
            }
            else
            {
                transaction.Fraudable = false;
            }
        }

        private void ValidateCurrencyConversion(Transaction transaction, Currency currency)
        {
            if (currency != null && currency.Courses != null)
            {
                var lastCourse = currency.Courses.LastOrDefault();
                if (lastCourse != null)
                {
                    if (transaction.Amount * lastCourse.Rate < transaction.Amount - 3 || transaction.Amount * lastCourse.Rate > transaction.Amount + 3)
                    {
                        transaction.Fraudable = true; // conversion is not correct
                    }
                }
            }
        }

        public async Task<bool> ClearFraudulentStatus(IEnumerable<Transaction> transactions)
        {
            foreach (var tran in transactions)
            {
                var transaction = await Dbset.FirstOrDefaultAsync(t =>
                    t.Amount == tran.Amount &&
                    t.AmountEquivalent == tran.AmountEquivalent &&
                    t.CategoryId == tran.CategoryId &&
                    t.ChannelId == tran.ChannelId &&
                    t.CurrencyId == tran.CurrencyId);

                if (transaction != null)
                {
                    transaction.Fraudable = false;
                }
            }

            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<IDictionary<string, IEnumerable<Transaction>>> GetFraudulentTransactionsByCategory(DateTime start, DateTime end)
        {
            var groupedTransactions = await Dbset
                .Include(t => t.Category)
                .Where(t =>
                    t.Category != null &&
                    t.Fraudable &&
                    t.IsActive &&
                    t.Date >= start &&
                    t.Date <= end)
                .GroupBy(t => t.Category.TransactionCategory)
                .ToDictionaryAsync(
                    group => group.Key,
                    group => group.Select(t => t)
                );

            return groupedTransactions;
        }

        public async Task<IEnumerable<Transaction>> GetSuspiciousTransactions(DateTime start, DateTime end)
        {
            return await Dbset
                .Where(t =>
                    t.IsSuspecisious &&
                    t.IsActive &&
                    t.Date >= start &&
                    t.Date <= end)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsForReview(DateTime start, DateTime end)
        {
            return await Dbset
                .Where(t =>
                    t.Fraudable &&
                    t.IsActive &&
                    t.Date >= start &&
                    t.Date <= end)
                .ToListAsync();
        }

        public async Task<double> MakePredictionAboutFraudulence(Transaction transaction)
        {
            var existingTransaction = await Dbset.FirstOrDefaultAsync(t =>
                t.Amount == transaction.Amount &&
                t.AmountEquivalent == transaction.AmountEquivalent &&
                t.CategoryId == transaction.CategoryId &&
                t.ChannelId == transaction.ChannelId &&
                t.CurrencyId == transaction.CurrencyId);

            var input = GetTransactionInput(transaction);
            var result = network.Compute(input);

            if (existingTransaction == null)
            {
                return result[0];
            }

            if (result[0] >= 0.70 && result[0] <= 0.90)
            {
                existingTransaction.IsSuspecisious = true;
            }
            else if (result[0] > 0.90)
            {
                existingTransaction.Fraudable = true;
            }

            await Context.SaveChangesAsync();
            return result[0];
        }

        public async Task<bool> UpdateTransactionPrediction(Transaction transaction, bool isFraudulent)
        {
            var existingTransaction = await Dbset.FirstOrDefaultAsync(t =>
                t.Amount == transaction.Amount &&
                t.AmountEquivalent == transaction.AmountEquivalent &&
                t.CategoryId == transaction.CategoryId &&
                t.ChannelId == transaction.ChannelId &&
                t.CurrencyId == transaction.CurrencyId);

            if (existingTransaction != null)
            {
                existingTransaction.Fraudable = isFraudulent;
                await Context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
