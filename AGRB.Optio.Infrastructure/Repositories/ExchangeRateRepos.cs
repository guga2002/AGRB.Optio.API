using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class ExchangeRateRepos : AbstractRepositroy<ExchangeRate>, IExchangeRate
    {
        public ExchangeRateRepos(OptioDB optioDB) : base(optioDB)
        {
        }

        #region AddAsync
        public async Task<long> AddAsync(ExchangeRate entity)
        {
            try
            {
                if (!await Context.Currencies.AnyAsync(io => io.Id == entity.CurrencyId))
                {
                    throw new InvalidOperationException(" no such a Currency  Exist!");
                }
                if (!await Dbset.AnyAsync(io => io.CurrencyId == entity.CurrencyId && io.Date == entity.Date))
                {
                    await Dbset.AddAsync(entity);
                    await Context.SaveChangesAsync();
                    var res = await Dbset.MaxAsync(io => io.Id);
                    return res;
                }
                else
                {
                    throw new ArgumentException("Error while adding entity, Entity already exist in same Date");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<ExchangeRate>> GetAllAsync()
        {
            try
            {
                var res = await Dbset.AsNoTracking().ToListAsync();
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllActiveExchangeRateAsync

        public async Task<IEnumerable<ExchangeRate>> GetAllActiveRateAsync()
        {
            try
            {
                return await Dbset
                    .AsNoTracking()
                    .Where(io => io.IsActive)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region GetByIdAsync
        public async Task<ExchangeRate> GetByIdAsync(long id)
        {
            try
            {
                return await Dbset.
                    AsNoTracking()
                    .FirstOrDefaultAsync(io => io.Id == id && io.IsActive)
                    ?? throw new ArgumentException(" No  exchange rate exist On this ID :(");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(ExchangeRate entity)
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
                var course = await Dbset
                    .FirstOrDefaultAsync(io => io.Id == id && io.IsActive) ?? throw new ArgumentException(" no data exist or already soft deleted");
                course.IsActive = false;
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
        public async Task<bool> UpdateAsync(long id, ExchangeRate entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);
                var course = await Dbset.FindAsync(id) ??
                             throw new ArgumentException("The data is already up to data, or  such  a data no exist");

                course.IsActive = entity.IsActive;
                course.Date = entity.Date;
                course.CurrencyId = entity.CurrencyId;
                course.Rate = entity.Rate;
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
