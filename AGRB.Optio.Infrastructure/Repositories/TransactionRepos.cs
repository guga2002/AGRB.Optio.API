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
        private readonly CacheService _cache;

        public TransactionRepos(OptioDB optioDB, CacheService cache) : base(optioDB)
        {
            transactions = context.Set<Transaction>();
            this._cache = cache;
        }

        #region AddAsync
        public async Task<long> AddAsync(Transaction entity)
        {
            try
            {
                var tasks = new Task<bool>[]
                {
                    context.CategoryOfTransactions.AnyAsync(io => io.Id == entity.CategoryId),
                    context.Currencies.AnyAsync(io => io.Id == entity.CurrencyId),
                    context.Locations.AnyAsync(io => io.Id == entity.ChannelId),
                    context.Merchants.AnyAsync(io => io.Id == entity.MerchantId),
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
                await context.SaveChangesAsync();

                return entity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            try
            {
                return await transactions.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllWithDetailsAsync
        public async Task<IEnumerable<Transaction>> GetAllWithDetailsAsync()
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<Transaction> GetByIdAsync(long id)
        {
            try
            {
                return await transactions.AsNoTracking()
                    .FirstOrDefaultAsync(io => io.IsActive && io.Id == id)
                    ?? throw new ArgumentNullException("Transaction not found");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetByIdWithDetailsAsync
        public async Task<Transaction> GetByIdWithDetailsAsync(long id)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Transaction entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);
                transactions.Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(long id)
        {
            try
            {
                var transaction = await transactions.FindAsync(id);
                if (transaction != null)
                {
                    transaction.IsActive = false;
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
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
                    context.Entry(transaction).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllActiveAsync
        public async Task<IEnumerable<Transaction>> GetAllActiveAsync()
        {
            try
            {
                return await transactions.AsNoTracking().Where(io => io.IsActive).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
