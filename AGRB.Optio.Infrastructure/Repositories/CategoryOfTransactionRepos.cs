using Microsoft.EntityFrameworkCore;
using AGRB.Optio.Infrastructure.PerformanceImprovmentServices;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using AGRB.Optio.Domain.Data;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class CategoryOfTransactionRepos : AbstractRepositroy<Category>, ICategoryRepo
    {
        private readonly CacheService cacheService;

        public CategoryOfTransactionRepos(OptioDB optioDB, CacheService cacheService) : base(optioDB)
        {
            this.cacheService = cacheService;
        }


        #region AddAsync
        public async Task<long> AddAsync(Category entity)
        {
            try
            {
                var category = await Dbset.SingleOrDefaultAsync(i => i.TransactionCategory == entity.TransactionCategory);
                if (category != null) throw new ArgumentException("There is a similar category");
                if (!await Context.Types.AnyAsync(io => io.Id == entity.TransactionTypeId))
                    throw new ArgumentException("There is a similar category");
                await Dbset.AddAsync(entity);
                await Context.SaveChangesAsync();
                var max = await Dbset.MaxAsync(io => io.Id);
                return max;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                return await Dbset.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<Category> GetByIdAsync(long id)
        {
            try
            {
                return await Context.CategoryOfTransactions.Include(io => io.TypeOfTransaction)
               .SingleAsync(i => i.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllActiveAsync
        public async Task<IEnumerable<Category>> GetAllActiveAsync()
        {
            try
            {
                var cat = await Dbset.Include(io => io.TypeOfTransaction).AsNoTracking().Where(i => i.IsActive == true).ToListAsync();
                if (cat != null)
                {
                    return cat;
                }
                else
                {
                    throw new InvalidOperationException("No active category found");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Category entity)
        {
            try
            {
                Dbset.Remove(entity);
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
                var category = await Dbset.FindAsync(id) ?? throw new InvalidOperationException("There is no such category");
                category.IsActive = false;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, Category entity)
        {
            try
            {
                var category = await Dbset.FindAsync(id);
                if (category is null)
                {
                    throw new InvalidOperationException("There is no such category");
                }
                else
                {
                    category.TransactionCategory = entity.TransactionCategory;
                    category.TransactionTypeId = entity.TransactionTypeId;
                    await Context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
