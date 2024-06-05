using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Optio.Core.Data;
using Optio.Core.Interfaces;
using Optio.Core.Repositories;
using RGBA.Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Core.PerformanceImprovmentServices;

namespace RGBA.Optio.Core.Repositories
{
    public class UniteOfWork(
        OptioDB db,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        RoleManager<IdentityRole> role,
        CacheService cash)
        : IUniteOfWork
    {
        public ICategoryRepo CategoryOfTransactionRepository =>new CategoryOfTransactionRepos(db,cash);

        public IChannelRepo ChannelRepository => new ChannelRepos(db,cash);

        public ILocationRepo LocationRepository => new LocationRepos(db, cash);

        public IMerchantRepo MerchantRepository => new MerchantRepos(db,configuration);

        public ITransactionRepo TransactionRepository => new TransactionRepos(db);

        public ITypeOfTransactionRepo TypeOfTransactionRepository => new TypeOfTransactionRepos(db);

        public ICurrencyRepository CurrencyRepository => new CurrencyRepos(db);
    
        public ILocationToMerchantRepository LocationToMerchantRepository => new LocationToMerchantRepos(db);

        public IExchangeRate ExchangeRateRepository => new ExchangeRateRepos(db);

        public async Task CheckAndCommitAsync()
        {
            try
            {
              await  db.SaveChangesAsync();
              await db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await db.Database.RollbackTransactionAsync();
            }
        }

        public void Dispose()
        {
            db.Dispose();
            userManager.Dispose();
            role.Dispose();
        }

       
    }
}
