using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using RGBA.Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using System.Data;
using AbstractClass = Optio.Core.Repositories.AbstractClass;

namespace RGBA.Optio.Core.Repositories
{
    public class CurrencyReposiotry : AbstractClass, ICurrencyRepository
    {
        private readonly DbSet<Currency> currencies;
        public CurrencyReposiotry(OptioDB db):base(db)
        {
            currencies = context.Set<Currency>();
        }

        #region AddAsync
        public async Task<long> AddAsync(Currency entity)
        {
            try
            {
                if (!await currencies.AnyAsync(io => io.NameOfValute == entity.NameOfValute && io.CurrencyCode == entity.CurrencyCode))
                {
                    await currencies.AddAsync(entity);
                    await context.SaveChangesAsync();
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
            return await currencies.AsNoTracking().Where(io=>io.IsActive == true).ToListAsync();
        }

        #endregion

        #region GetByIdAsync
        public async Task<Currency> GetByIdAsync(int id)
        {
            try
            {
                var result = await currencies.FindAsync(id);
                if (result is null) throw new ArgumentException("no entity found!");
                return result;
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

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var res = await currencies.FindAsync(id);
                if (res is not null)
                {
                    res.IsActive = false;
                    await context.SaveChangesAsync();
                    return true;
                }
                throw new ArgumentException("already the data is  soft deleted or no exist");
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
                if (res is not null)
                {
                    res.CurrencyCode= entity.CurrencyCode;
                    res.NameOfValute= entity.NameOfValute;
                    res.IsActive = entity.IsActive;
                    await context.SaveChangesAsync();
                    return true;
                }
                throw new ArgumentException(" no such  currency exist");
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        #endregion
    }
}
