using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class LocationRepos : AbstractRepositroy<Location>, ILocationRepo
    {

        public LocationRepos(OptioDB optioDB) : base(optioDB)
        {
        }


        #region AddAsync
        public async Task<long> AddAsync(Location entity)
        {
            try
            {
                if (!await Dbset.AnyAsync(i => i.LocationName.ToLower() == entity.LocationName.ToLower()))
                {
                    await Dbset.AddAsync(entity);
                    await Context.SaveChangesAsync();
                    var max = await Dbset.MaxAsync(io => io.Id);
                    return max;
                }
                else
                {
                    throw new InvalidOperationException("Such a city already exists");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Location>> GetAllAsync()
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

        #region GetAllActiveLocationAsync
        public async Task<IEnumerable<Location>> GetAllActiveLocationAsync()
        {
            return await Dbset.AsNoTracking().Where(i => i.IsActive).ToListAsync();
        }
        #endregion

        #region GetByIdAsync

        public async Task<Location> GetByIdAsync(long id)
        {
            try
            {
                return await Dbset
                .AsNoTracking()
                .SingleAsync(i => i.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Location entity)
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
                var city = await Dbset.FindAsync(id) ?? throw new InvalidOperationException("No such city was found");

                city.IsActive = false;
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
        public async Task<bool> UpdateAsync(long id, Location entity)
        {
            try
            {
                var city = await Dbset.FindAsync(id) ?? throw new InvalidOperationException("No such city was found");
                city.LocationName = entity.LocationName;
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
