using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class TransactionRepos : AbstractRepositroy<Transaction>, ITransactionRepo
    {

        public TransactionRepos(OptioDB optioDB) : base(optioDB)
        {
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
                Dbset.AnyAsync(io => io.Id == entity.Id)
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

            await Dbset.AddAsync(entity);
            await Context.SaveChangesAsync();

            return entity.Id;
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await Dbset.AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetAllWithDetailsAsync
        public async Task<IEnumerable<Transaction>> GetAllWithDetailsAsync()
        {
            var transactionsWithDetails = await Dbset
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
            return await Dbset.AsNoTracking()
                       .FirstOrDefaultAsync(io => io.IsActive && io.Id == id)
                   ?? throw new ArgumentNullException("Transaction not found");
        }
        #endregion

        #region GetByIdWithDetailsAsync
        public async Task<Transaction> GetByIdWithDetailsAsync(long id)
        {
            var transactionWithDetails = await Dbset
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
            Dbset.Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(long id)
        {
            var transaction = await Dbset.FindAsync(id);
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
                var transaction = await Dbset.FindAsync(id);
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
            return await Dbset.AsNoTracking().Where(io => io.IsActive).ToListAsync();
        }
        #endregion
    }
}
