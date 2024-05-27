using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using Optio.Core.Entities;
using Optio.Core.Interfaces;
using RGBA.Optio.Core.PerformanceImprovmentServices;

namespace Optio.Core.Repositories
{
    public class LocationRepos : AbstractClass, ILocationRepo
    {
        private readonly DbSet<Location> locations;
        private readonly CacheService cacheService;

        public LocationRepos(OptioDB optioDB, CacheService cacheService):base(optioDB)
        {
            locations=context.Set<Location>();
            this.cacheService=cacheService;
        }


        #region AddAsync
        public async Task<long> AddAsync(Location entity)
        {
            try
            {
                var city = await locations.SingleOrDefaultAsync(i => i.LocationName.ToLower() == entity.LocationName.ToLower());
                if (city != null)
                {
                    throw new InvalidOperationException("Such a city already exists");
                }
                else
                {
                    await locations.AddAsync(entity);
                    await context.SaveChangesAsync();
                    var max = await locations.MaxAsync(io => io.Id);
                    return max;
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
                return await locations.AsNoTracking().ToListAsync();
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
            try
            {
                var city = await locations.AsNoTracking().Where(i => i.IsActive == true).ToListAsync();
                if (city == null)
                {
                    throw new InvalidOperationException("No active city found");
                }
                else
                {
                    return city;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetByIdAsync

        public async Task<Location> GetByIdAsync(long id)
        {
            try
            {
                string key = $"Location by Id:{id}";
                await Task.Delay(1);
                Location location = cacheService.GetOrCreate(
                    key, () =>
                    {
                        return locations
                        .AsNoTracking()
                        .Single(i => i.Id == id) ??
                        throw new ArgumentException($"No location found by id: {id}");

                    }, TimeSpan.FromMinutes(15)
                    ) ;
                return location ?? throw new ArgumentException($"No location found by id: {id}"); ;

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
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                locations.Remove(entity);
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
            var city = await locations.FindAsync(id);
                if (city is null)
                {
                    throw new InvalidOperationException("No such city was found");
                }
                else
                {
                    city.IsActive = false;
                    await context.SaveChangesAsync();
                    return true;
                }
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
                ArgumentNullException.ThrowIfNull(entity,nameof(entity));
                var city = await locations.FindAsync(id);
                if (city is null)
                {
                    throw new InvalidOperationException("No such city was found");
                }
                else
                {
                    city.LocationName = entity.LocationName;
                    await context.SaveChangesAsync();
                    return true;
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
