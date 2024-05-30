using Optio.Core.Data;
using RGBA.Optio.Core.Interfaces;
using Optio.Core.Entities;
using RGBA.Optio.Stream.Interfaces;
using RGBA.Optio.Stream.DecerializerClasses;
using RGBA.Optio.Core.Entities;
using Currency = RGBA.Optio.Core.Entities.Currency;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace RGBA.Optio.Stream.SeedServices
{
    public class TransactionRelatedSer:ITransactionRelatedSer
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly OptioDB optioDB;
        private readonly Random rand;
        private readonly Random rand1;
        private readonly IConfiguration conf;
        public TransactionRelatedSer(IUniteOfWork _uniteOfWork, OptioDB optioDB, IConfiguration conf)
        {
            this._uniteOfWork = _uniteOfWork;
            this.optioDB = optioDB;
            rand = new Random();
            rand1 = new Random();
            this.conf = conf;
        }

        #region channel
        public async Task<bool> fillChannel()
        {
            await _uniteOfWork.ChanellRepository.AddAsync(new Channels { ChannelType="ფილიალი" });
            await _uniteOfWork.ChanellRepository.AddAsync(new Channels { ChannelType = "მობილური ინტერნეტ ბანკი" });
            await _uniteOfWork.ChanellRepository.AddAsync(new Channels { ChannelType = "ინტერნეტ ბანკი" });
            await _uniteOfWork.ChanellRepository.AddAsync(new Channels { ChannelType = "ტერმინალი" });
            await _uniteOfWork.ChanellRepository.AddAsync(new Channels { ChannelType = "ნაღდი ანგარიშსწორება" });
            await _uniteOfWork.ChanellRepository.AddAsync(new Channels { ChannelType = "განვადება" });
            return true;
        }
        #endregion

        #region TypeOfTransaction
        public async Task<bool> FillTypeOfTransaction()
        {
            await optioDB.Types.AddAsync(new TypeOfTransaction()
            {
                TransactionName = "შემოსავალი",
              
                Category = new List<Category>()
                {
                    new Category()
                    {
                        IsActive = true,

                        TransactionCategory="ხელფასი",
                    },
                      new Category()
                    {
                        IsActive = true,
                        TransactionCategory="ბონუსი",
                    },
                        new Category()
                    {
                        IsActive = true,
                        TransactionCategory="დივიდენტი",
                    },
                              new Category()
                    {
                        IsActive = true,
                        TransactionCategory="სხვა შემოსავალი",
                    },
                }
            });
            await optioDB.SaveChangesAsync();
            await optioDB.Types.AddAsync(new TypeOfTransaction()
            {
                TransactionName = "ხარჯი",
                Category = new List<Category>()
                {
                    new Category()
                    {
                        IsActive = true,
                        TransactionCategory="საყოფაცხოვრებო ხარჯი",
                    },
                      new Category()
                    {
                        IsActive = true,
                        TransactionCategory="ტრანსპორტი",
                    },
                        new Category()
                    {
                        IsActive = true,
                        TransactionCategory="კვება და სურსათი",
                    },
                              new Category()
                    {
                        IsActive = true,
                        TransactionCategory="ტრანსპორტი",
                        TransactionTypeID=1,
                    },
                    new Category()
                    {
                        IsActive = true,
                        TransactionCategory="კომუნალურები",
                    },
                       new Category()
                    {
                        IsActive = true,
                        TransactionCategory="განათლება",
                    },
                }
            });
            await optioDB.SaveChangesAsync();
            await optioDB.Types.AddAsync(new TypeOfTransaction()
            {
                TransactionName = "გადარიცხვა საკუთარ ანგარიშზე",
                Category = new List<Category>()
                {
                    new Category()
                    {
                        IsActive = true,
                        TransactionCategory="შიდა გადარიცხვა",
                        TransactionTypeID=1,
                    },
                }
            });
            await optioDB.SaveChangesAsync();
            await optioDB.Types.AddAsync(new TypeOfTransaction()
            {
                TransactionName = "სხვასთან გადარიცხვა",
                Category = new List<Category>()
                {
                    new Category()
                    {
                        IsActive = true,
                        TransactionCategory="სხვა ბანკში გადარიცხვა",
                        TransactionTypeID=1
                    }
                }
            });
            await optioDB.SaveChangesAsync();
            await optioDB.Types.AddAsync(new TypeOfTransaction()
            {
                TransactionName = "განაღდება",
                Category = new List<Category>()
                {
                    new Category()
                    {
                        IsActive = true,
                        TransactionCategory="თანხის განაღდება",
                        TransactionTypeID=1
                    },
                }
            });
            await optioDB.SaveChangesAsync();
            return true;
        }
        #endregion

        #region InsertCurrencies
        public async Task  InsertCurrencies(List<CurrenciesResponse> cur)
        {

            foreach (var currency in cur)
            {
                foreach (var Dgiuri in currency.Currencies)
                {
                    var curenc=await optioDB.Currencies.FirstOrDefaultAsync(io=>io.NameOfValute==Dgiuri.name);
                    if (curenc is  null)
                    {
                        curenc = new Currency()
                        {
                            CurrencyCode = Dgiuri.code,
                            NameOfValute = Dgiuri.name,
                            IsActive = true,
                        };
                    }

                    var valute = new ValuteCourse()
                    {
                        ExchangeRate = (decimal)Dgiuri.rate/Dgiuri.quantity,
                        DateOfValuteCourse = currency.Date,
                        Currency = curenc,
                    };
                    optioDB.ValuteCourses.Add(valute);
                    await optioDB.SaveChangesAsync();
                }
            }
        }
        #endregion

        #region FIllBUlkTransaction
        public async Task<bool> FillTransactionsBulk(int n)
        {
            List<Transaction> transactions = new List<Transaction>();
            var category = await optioDB.CategoryOfTransactions.ToListAsync();
            var merchants = await optioDB.Merchants.ToListAsync();
            var channel = await optioDB.Channels.ToListAsync();
            var currency = await optioDB.Currencies.ToListAsync();
            for (var i = 0; i < n; i++)
            {
                var categoryIndex = rand.Next(0, (int)category.Max(i => i.Id));
                if (!await optioDB.CategoryOfTransactions.AnyAsync(i => i.Id == categoryIndex))
                {
                    i--;
                    continue;
                }
                var merchantIndex = rand.Next(0, (int)merchants.Max(i => i.Id));
                if (!await optioDB.Merchants.AnyAsync(i => i.Id == merchantIndex))
                {
                    i--;
                    continue;
                }
                var channelIndex = rand.Next(0, (int)channel.Max(i => i.Id));
                if (!await optioDB.Channels.AnyAsync(i => i.Id == channelIndex))
                {
                    i--;
                    continue;
                }

                var currencyIndex = rand.Next(0, (int)currency.Max(i => i.Id));
                var currencyIn = await optioDB.Currencies.Where(i => i.Id == currencyIndex).Include(i => i.Courses).FirstOrDefaultAsync();
                if (currencyIn is null)
                {
                    i--;
                    continue;
                }

                var trans = new Transaction
                {
                    Date = DateTime.Now,
                    Amount = rand1.Next(10000),
                    AmountEquivalent = 0,
                    CurrencyId = currencyIndex,
                    CategoryId = categoryIndex,
                    MerchantId = merchantIndex,
                    ChannelId = channelIndex,
                    IsActive = true,
                };
              
                trans.AmountEquivalent = trans.Amount * currencyIn.Courses.OrderByDescending(i => i.DateOfValuteCourse).FirstOrDefault().ExchangeRate;
               transactions.Add(trans);
            }
            await optioDB.Transactions.AddRangeAsync(transactions);
            await optioDB.SaveChangesAsync();
            return true;
        }
        #endregion

        #region FillTransactions
        public async Task<bool> FillTransactions(int n)
        {
            var category= await optioDB.CategoryOfTransactions.ToListAsync();
            var merchants=await optioDB.Merchants.ToListAsync();
            var channel = await optioDB.Channels.ToListAsync();
            var currency = await optioDB.Currencies.ToListAsync();
            for (var i = 0; i < n; i++)
            {
                var categoryIndex = rand.Next(0, (int)category.Max(i => i.Id));
                if(!await optioDB.CategoryOfTransactions.AnyAsync(i => i.Id == categoryIndex))
                {
                    i--;
                    continue;
                }
                var merchantIndex = rand.Next(0, (int)merchants.Max(i => i.Id));
                if (!await optioDB.Merchants.AnyAsync(i => i.Id == merchantIndex))
                {
                    i--;
                    continue;
                }
                var channelIndex = rand.Next(0, (int)channel.Max(i => i.Id));
                if (!await optioDB.Channels.AnyAsync(i => i.Id == channelIndex))
                {
                    i--;
                    continue;
                }
                
                var currencyIndex = rand.Next(0, (int)currency.Max(i => i.Id));
                var currencyIn = await optioDB.Currencies.Where(i => i.Id == currencyIndex).Include(i=>i.Courses).FirstOrDefaultAsync();
                if (currencyIn is null)
                {
                    i--;
                    continue;
                }
                Random randomi=new Random();
                var trans = new Transaction
                {
                    Date = DateTime.Now.AddDays(-randomi.Next(3,200)),
                    Amount = rand1.Next(10000),
                    AmountEquivalent = 0,
                    CurrencyId = currencyIndex,
                    CategoryId = categoryIndex,
                    MerchantId = merchantIndex,
                    ChannelId = channelIndex,
                    IsActive = true,
                };
                trans.AmountEquivalent = trans.Amount * currencyIn.Courses.OrderByDescending(i => i.DateOfValuteCourse).FirstOrDefault().ExchangeRate;
                try
                {
                    using (IDbConnection db = new SqlConnection(conf.GetConnectionString("OptiosString")))
                    {

                        string sqlQuery = @"
                        INSERT INTO Transactions (Date_Of_Transaction, Amount, Amount_Equivalent,Transaction_Status, CurrencyId, CategoryId, MerchantId, ChannelId)
                        VALUES (@Date, @Amount, @AmountEquivalent,@IsActive, @CurrencyId, @CategoryId, @MerchantId, @ChannelId)";
                        await db.ExecuteAsync(sqlQuery, trans);
                    }
                }
                catch (SqlException ex)
                {
                    throw;
                }
            }
            return true;
        }
        #endregion


        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            return await _uniteOfWork.MerchantRepository.getalltransactions();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsWithoutDapper()
        {
            return await _uniteOfWork.TransactionRepository.GetAllWithDetailsAsync();
        }
    }
}
