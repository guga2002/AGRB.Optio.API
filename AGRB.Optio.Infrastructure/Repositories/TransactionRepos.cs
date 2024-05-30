using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using Optio.Core.Entities;
using Optio.Core.Interfaces;
using RGBA.Optio.Core.PerformanceImprovmentServices;

namespace Optio.Core.Repositories
{
    public class TransactionRepos : AbstractClass, ITransactionRepo
    {
        private readonly DbSet<Transaction> transactions;

        public TransactionRepos(OptioDB optioDB) : base(optioDB)
        {
            transactions = Context.Set<Transaction>();
        }

        #region AddAsync
        public async Task<long> AddAsync(Transaction entity)
        {
            var tasks = new Task<bool>[]
            {
                Context.CategoryOfTransactions.AnyAsync(io => io.Id == entity.CategoryId),
                Context.Currencies.AnyAsync(io => io.Id == entity.CurrencyId),
                Context.Locations.AnyAsync(io => io.Id == entity.ChannelId),
                Context.Merchants.AnyAsync(io => io.Id == entity.MerchantId),
                transactions.AnyAsync(io => io.Id == entity.Id)
            };

            var results = await Task.WhenAll(tasks);

            if (results.Take(4).Any(e => !e))
            {
                throw new ArgumentException("No related Table exist, Please correct your data");
            }

            if (results[4])
            {
                throw new ArgumentException("Such a Transaction Already Exist In Db");
            }

            await transactions.AddAsync(entity);
            await Context.SaveChangesAsync();

            return entity.Id;
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await transactions.AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetAllWithDetailsAsync
        public async Task<IEnumerable<Transaction>> GetAllWithDetailsAsync()
        {
            var transactionsWithDetails = await transactions
                .Include(io => io.Category)
                .Include(io => io.Channel)
                .Include(io => io.Currency).ThenInclude(io => io.Courses)
                .Include(io => io.Merchant).ThenInclude(io => io.Locations)
                .AsNoTracking()
                .ToListAsync();

            return transactionsWithDetails;
        }
        #endregion

        #region GetByIdAsync
        public async Task<Transaction> GetByIdAsync(long id)
        {
            return await transactions.AsNoTracking()
                       .FirstOrDefaultAsync(io => io.IsActive && io.Id == id)
                   ?? throw new ArgumentNullException("Transaction not found");
        }
        #endregion

        #region GetByIdWithDetailsAsync
        public async Task<Transaction> GetByIdWithDetailsAsync(long id)
        {
            var transactionWithDetails = await transactions
                .Include(io => io.Category)
                .Include(io => io.Channel)
                .Include(io => io.Currency).ThenInclude(io => io.Courses)
                .Include(io => io.Merchant).ThenInclude(io => io.Locations)
                .AsNoTracking()
                .FirstOrDefaultAsync(io => io.Id == id);

            return transactionWithDetails ?? throw new ArgumentNullException("Transaction not found");
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Transaction entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            transactions.Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(long id)
        {
            var transaction = await transactions.FindAsync(id);
            if (transaction != null)
            {
                transaction.IsActive = false;
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(long id, Transaction entity)
        {
            try
            {
                var transaction = await transactions.FindAsync(id);
                if (transaction != null)
                {
                    Context.Entry(transaction).CurrentValues.SetValues(entity);
                    await Context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var databaseEntity = (Transaction)entry.Entity;
                entry.CurrentValues.SetValues(databaseEntity);
                throw;
            }
        }
        #endregion

        #region GetAllActiveAsync
        public async Task<IEnumerable<Transaction>> GetAllActiveAsync()
        {
            return await transactions.AsNoTracking().Where(io => io.IsActive).ToListAsync();
        }
        #endregion
    }
}
