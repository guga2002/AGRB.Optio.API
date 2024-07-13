using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace AGRB.Optio.Infrastructure.Repositories
{
    public class LocationToMerchantRepos : AbstractRepositroy<LocationToMerchant>, ILocationToMerchantRepository
    {

        public LocationToMerchantRepos(OptioDB optioDB) : base(optioDB)
        {
        }

        #region GetLocationIdByMerchantIdAsync
        public async Task<Location> GetLocationIdByMerchantIdAsync(long merchantId)
        {
            try
            {
                var merch = await Context.LocationToMerchants.FirstOrDefaultAsync(i => i.MerchantId == merchantId);
                if (merch is null)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    var merchLocation = await Context.Locations.Where(i => i.Id == merch.LocationId).FirstOrDefaultAsync();
                    if (merchLocation is null)
                    {
                        throw new InvalidOperationException();
                    }
                    else
                    {
                        return merchLocation;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAll
        public async Task<IEnumerable<LocationToMerchant>> GetAllLocationToMerchant()
        {
            return await Dbset
                .ToListAsync();
        }
        #endregion
    }
}
