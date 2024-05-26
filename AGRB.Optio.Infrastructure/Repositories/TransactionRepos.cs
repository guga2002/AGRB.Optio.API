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

        public TransactionRepos(OptioDB optioDB, CacheService cache) :base(optioDB)
        {
            transactions=context.Set<Transaction>();
            this._cache = cache;
        }

        #region AddAsync
        public async Task<long> AddAsync(Transaction entity)
        {
            try
            {
                if (!await context.CategoryOfTransactions.AnyAsync(io => io.Id == entity.CategoryId) ||
                    !await context.Currencies.AnyAsync(io => io.Id == entity.CurrencyId) ||
                     !await context.Locations.AnyAsync(io => io.Id == entity.ChannelId) ||
                      !await context.Merchants.AnyAsync(io => io.Id == entity.MerchantId))
                {
                    throw new ArgumentException("No related Table exist, Please   Recorect your data");
                }
                if (await transactions.AnyAsync(io => io.Id == entity.Id))
                {
                    throw new ArgumentException("Such a Transaction Already Exist In Db");
                }
                await transactions.AddAsync(entity);
                await context.SaveChangesAsync();
                var max =await  transactions.MaxAsync(io => io.Id);
                return max;
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
                return await  transactions.Include(io => io.Category)
               .Include(io => io.Channel)
                .Include(io => io.Currency)
                .ThenInclude(io => io.Courses)
                .Include(io => io.Merchant)
                .ThenInclude(io => io.Locations).ToListAsync();
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
                string cacheKey = $"Transaction_{id}";
                await Task.Delay(1);
                Transaction transaction = _cache.GetOrCreate(cacheKey, () =>
                {
                    return transactions.AsNoTracking()
                        .Single(io => io.IsActive && io.Id == id);
                }, TimeSpan.FromMinutes(15));

                return transaction ?? throw new ArgumentException("No transaction found");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region GetByIdWithDetailsAsync
        public async Task<Transaction> GetByIdWithDetailsAsync(long ID)
        {
            try
            {
                var res = await transactions.Include(io => io.Category)
                 .Include(io => io.Channel)
                .Include(io => io.Currency)
                 .ThenInclude(io => io.Courses)
                    .Include(io => io.Merchant)
                     .ThenInclude(io => io.Locations)
                    .FirstOrDefaultAsync(io => io.Id == ID);
                return res ?? throw new ArgumentNullException("no  data  exsit, On this ID");
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
                var res = await transactions.FindAsync(id);
                if (res is not null)
                {
                    res.IsActive = false;
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
        public async Task<bool> UpdateAsync(long id,Transaction entity)
        {
            try
            {
                var tran = await transactions.FindAsync(id);
                if (tran is not null)
                {
                    context.Entry(tran).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var en = ex.Entries.Single();
                var value = (Transaction)en.Entity;
                en.CurrentValues.SetValues(value);
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
                var res = await transactions.AsNoTracking().Where(io => io.IsActive == true).ToListAsync();
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
