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
    public class UniteOfWork : IUniteOfWork
    {
        private readonly OptioDB db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CacheService chash;
        private readonly IConfiguration configuration;
        public UniteOfWork(OptioDB db, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, RoleManager<IdentityRole> role,CacheService cash)
        {
            this.db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = role;
            this.configuration = configuration;
            this.chash = cash;
        }
        public ICategoryRepo CategoryOfTransactionRepository =>new CategoryOfTransactionRepos(db,chash);

        public IChannelRepo ChanellRepository => new ChannelRepos(db,chash);

        public ILocationRepo LocationRepository => new LocationRepos(db, chash);

        public IMerchantRepo MerchantRepository => new MerchantRepos(db,configuration);

        public ITransactionRepo TransactionRepository => new TransactionRepos(db,chash);

        public ITypeOfTransactionRepo TypeOfTransactionRepository => new TypeOfTransactionRepos(db);


        public IValuteCourse ValuteCourse => new ValuteRepository(db);

        public ICurrencyRepository CurrencyRepository => new CurrencyReposiotry(db);
    
        public ILocationToMerchantRepository LocationToMerchantRepository => new LocationToMerchantRepos(db);

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
            _userManager.Dispose();
            _roleManager.Dispose();
        }

       
    }
}
