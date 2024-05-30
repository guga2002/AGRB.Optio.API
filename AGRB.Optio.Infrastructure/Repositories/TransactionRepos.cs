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
<<<<<<< HEAD
            transactions = context.Set<Transaction>();
=======
            transactions=Context.Set<Transaction>();
>>>>>>> 701c60c65654924432982fb2e241108a7a136806
            this._cache = cache;
        }

        #region AddAsync
        public async Task<long> AddAsync(Transaction entity)
        {
            try
            {
<<<<<<< HEAD
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
=======
                if (!await Context.CategoryOfTransactions.AnyAsync(io => io.Id == entity.CategoryId) ||
                    !await Context.Currencies.AnyAsync(io => io.Id == entity.CurrencyId) ||
                     !await Context.Locations.AnyAsync(io => io.Id == entity.ChannelId) ||
                      !await Context.Merchants.AnyAsync(io => io.Id == entity.MerchantId))
                {
                    throw new ArgumentException("No related Table exist, Please fix your data");
>>>>>>> 701c60c65654924432982fb2e241108a7a136806
                }

                if (results[4])
                {
                    throw new ArgumentException("Such a Transaction Already Exist In Db");
                }

                await transactions.AddAsync(entity);
<<<<<<< HEAD
                await context.SaveChangesAsync();

                return entity.Id;
=======
                await Context.SaveChangesAsync();
                var max =await  transactions.MaxAsync(io => io.Id);
                return max;
>>>>>>> 701c60c65654924432982fb2e241108a7a136806
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
            return await transactions.AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetAllWithDetailsAsync
        public async Task<IEnumerable<Transaction>> GetAllWithDetailsAsync()
        {
            try
            {
<<<<<<< HEAD
                var transactionsWithDetails = await transactions
                    .Include(io => io.Category)
                    .Include(io => io.Channel)
                    .Include(io => io.Currency).ThenInclude(io => io.Courses)
                    .Include(io => io.Merchant).ThenInclude(io => io.Locations)
                    .AsNoTracking()
                    .ToListAsync();

                return transactionsWithDetails;
=======
                return await  transactions.Include(io => io.Category)
                .Include(io => io.Channel)
                .Include(io => io.Currency)
                .ThenInclude(io => io.Courses)
                .Include(io => io.Merchant)
                .ThenInclude(io => io.Locations).ToListAsync();
>>>>>>> 701c60c65654924432982fb2e241108a7a136806
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
<<<<<<< HEAD
                return await transactions.AsNoTracking()
                    .FirstOrDefaultAsync(io => io.IsActive && io.Id == id)
                    ?? throw new ArgumentNullException("Transaction not found");
=======
                var cacheKey = $"Transaction_{id}";
                await Task.Delay(1);
                var transaction = _cache.GetOrCreate(cacheKey, () =>
                {
                    return transactions.AsNoTracking()
                        .Single(io => io.IsActive && io.Id == id);
                }, TimeSpan.FromMinutes(15));

                return transaction ?? throw new ArgumentException("No transaction found");
>>>>>>> 701c60c65654924432982fb2e241108a7a136806
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
<<<<<<< HEAD
                var transactionWithDetails = await transactions
                    .Include(io => io.Category)
                    .Include(io => io.Channel)
                    .Include(io => io.Currency).ThenInclude(io => io.Courses)
                    .Include(io => io.Merchant).ThenInclude(io => io.Locations)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(io => io.Id == id);

                return transactionWithDetails ?? throw new ArgumentNullException("Transaction not found");
=======
                var res = await transactions.Include(io => io.Category)
                    .Include(io => io.Channel)
                    .Include(io => io.Currency)
                    .ThenInclude(io => io.Courses)
                    .Include(io => io.Merchant)
                    .ThenInclude(io => io.Locations)
                    .FirstOrDefaultAsync(io => io.Id == ID);
                return res ?? throw new ArgumentNullException("No  data  exsit, on this id");
>>>>>>> 701c60c65654924432982fb2e241108a7a136806
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
                await Context.SaveChangesAsync();
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
<<<<<<< HEAD
                var transaction = await transactions.FindAsync(id);
                if (transaction != null)
                {
                    transaction.IsActive = false;
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
=======
                var res = await transactions.FindAsync(id) ?? throw new InvalidOperationException("No merchant found");
                res.IsActive = false;
                await Context.SaveChangesAsync();
                return true;
>>>>>>> 701c60c65654924432982fb2e241108a7a136806
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
<<<<<<< HEAD
                var transaction = await transactions.FindAsync(id);
                if (transaction != null)
                {
                    context.Entry(transaction).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
=======
                var tran = await transactions.FindAsync(id) ??
                           throw new InvalidOperationException("No merchant found");
                    Context.Entry(tran).CurrentValues.SetValues(entity);
                    await Context.SaveChangesAsync();
>>>>>>> 701c60c65654924432982fb2e241108a7a136806
                    return true;
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
<<<<<<< HEAD
                return await transactions.AsNoTracking().Where(io => io.IsActive).ToListAsync();
=======
                var res = await transactions.AsNoTracking().Where(io => io.IsActive).ToListAsync();
                return res;
>>>>>>> 701c60c65654924432982fb2e241108a7a136806
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
