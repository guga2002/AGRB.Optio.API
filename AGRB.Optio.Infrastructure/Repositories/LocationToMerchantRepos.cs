﻿using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using AbstractClass = Optio.Core.Repositories.AbstractClass;


namespace RGBA.Optio.Core.Repositories
{
    public class LocationToMerchantRepos : AbstractClass, ILocationToMerchantRepository
    {
        private readonly DbSet<Location> locations;

        public LocationToMerchantRepos(OptioDB optioDB) : base(optioDB)
        {
            locations = Context.Set<Location>();
        }
        #region GetLocationIdByMerchantIdAsync
        public async Task<Location> GetLocationIdByMerchantIdAsync(long merchantId)
        {
            try
            {
                var merch= await Context.LocationToMerchants.FirstOrDefaultAsync(i=>i.MerchantId == merchantId);
                if (merch is null)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    var merchLocation=await locations.Where(i=>i.Id==merch.LocationId).FirstOrDefaultAsync();
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
    }
}
