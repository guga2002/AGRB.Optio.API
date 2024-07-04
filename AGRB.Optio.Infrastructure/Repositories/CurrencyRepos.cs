using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class CurrencyRepos : AbstractRepositroy<Currency>, ICurrencyRepository
    {
        public CurrencyRepos(OptioDB db) : base(db)
        {
        }

        #region AddAsync
        public async Task<long> AddAsync(Currency entity)
        {
            try
            {
                if (!await Dbset.AnyAsync(io => io.NameOfCurrency == entity.NameOfCurrency && io.CurrencyCode == entity.CurrencyCode))
                {
                    await Dbset.AddAsync(entity);
                    await Context.SaveChangesAsync();
                    var max = await Dbset.MaxAsync(io => io.Id);
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
            return await Dbset.AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetAllActiveAsync

        public async Task<IEnumerable<Currency>> GetAllActiveAsync()
        {
            return await Dbset.AsNoTracking().Where(io => io.IsActive).ToListAsync();
        }

        #endregion

        #region GetByIdAsync
        public async Task<Currency> GetByIdAsync(int id)
        {
            try
            {
                var result = await Dbset.FindAsync(id);
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
        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var res = await Dbset.FindAsync(id) ?? throw new ArgumentException("already the data is  soft deleted or no exist");
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
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                var res = await Dbset.FindAsync(id) ?? throw new ArgumentException(" no such  currency exist");
                res.CurrencyCode = entity.CurrencyCode;
                res.NameOfCurrency = entity.NameOfCurrency;
                res.IsActive = entity.IsActive;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
