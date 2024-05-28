using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using RGBA.Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using System.Data;
using AbstractClass = Optio.Core.Repositories.AbstractClass;

namespace RGBA.Optio.Core.Repositories
{
    public class CurrencyRepos : AbstractClass, ICurrencyRepository
    {
        private readonly DbSet<Currency> currencies;
        public CurrencyRepos(OptioDB db):base(db)
        {
            currencies = Context.Set<Currency>();
        }

        #region AddAsync
        public async Task<long> AddAsync(Currency entity)
        {
            try
            {
                if (!await currencies.AnyAsync(io => io.NameOfCurrency == entity.NameOfCurrency && io.CurrencyCode == entity.CurrencyCode))
                {
                    await currencies.AddAsync(entity);
                    await Context.SaveChangesAsync();
                    var max = await currencies.MaxAsync(io => io.Id);
                    return max;
                }
                else
                {
                    throw new ArgumentException("Such Currency already exist!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await currencies.AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetAllActiveAsync

        public async Task<IEnumerable<Currency>> GetAllActiveAsync()
        {
            return await currencies.AsNoTracking().Where(io=> io.IsActive).ToListAsync();
        }

        #endregion

        #region GetByIdAsync
        public async Task<Currency> GetByIdAsync(int id)
        {
            try
            {
                var result = await currencies.FindAsync(id);
                return result ?? throw new ArgumentException("no entity found!");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Currency entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                currencies.Remove(entity);
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

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var res = await currencies.FindAsync(id) ?? throw new ArgumentException("already the data is  soft deleted or no exist");
                res.IsActive = false;
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
        public async Task<bool> UpdateAsync(int id, Currency entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity,nameof(entity));
                var res = await currencies.FindAsync(id);
                if (res is null) throw new ArgumentException(" no such  currency exist");
                res.CurrencyCode = entity.CurrencyCode;
                res.NameOfCurrency = entity.NameOfCurrency;
                res.IsActive = entity.IsActive;
                await Context.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        #endregion
    }
}
