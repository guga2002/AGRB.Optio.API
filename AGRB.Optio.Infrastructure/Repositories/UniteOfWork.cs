using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using AGRB.Optio.Infrastructure.PerformanceImprovmentServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class UniteOfWork(
        OptioDB db,
        UserManager<User> userManager,
        IConfiguration configuration,
        RoleManager<IdentityRole> role,
        CacheService cash)
        : IUniteOfWork
    {
        public ICategoryRepo CategoryOfTransactionRepository => new CategoryOfTransactionRepos(db, cash);

        public IChannelRepo ChannelRepository => new ChannelRepos(db);

        public ILocationRepo LocationRepository => new LocationRepos(db);

        public IMerchantRepo MerchantRepository => new MerchantRepos(db, configuration);

        public ITransactionRepo TransactionRepository => new TransactionRepos(db);

        public ITypeOfTransactionRepo TypeOfTransactionRepository => new TypeOfTransactionRepos(db);

        public ICurrencyRepository CurrencyRepository => new CurrencyRepos(db);

        public ILocationToMerchantRepository LocationToMerchantRepository => new LocationToMerchantRepos(db);

        public IExchangeRate ExchangeRateRepository => new ExchangeRateRepos(db);

        public IFeadbackRepository FeadbackRepository => new FeadbackRepository(db);

        public async Task CheckAndCommitAsync()
        {
            try
            {
                await db.SaveChangesAsync();
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
