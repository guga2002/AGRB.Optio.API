using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using AGRB.Optio.Domain.Data;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class MerchantRepos : AbstractRepositroy<Merchant>, IMerchantRepo
    {
        private readonly IConfiguration conf;

        public MerchantRepos(OptioDB optioDB, IConfiguration conf) : base(optioDB)
        {
            this.conf = conf;
        }

        #region AssignLocationToMerchant
        public async Task<bool> AssignLocationToMerchant(long merchantId, long locationId)
        {
            try
            {
                if (!await Dbset.AnyAsync(io => io.Id == merchantId) ||
                    !await Context.Locations.AnyAsync(io => io.Id == locationId)) return false;
                using IDbConnection db = new SqlConnection(conf.GetConnectionString("OptiosString"));
                var sqlQuery = "INSERT INTO LocationToMerchants (locationId, merchantId) VALUES (@locationId, @merchantId)";
                await db.ExecuteAsync(sqlQuery, new { LocationId = locationId, MerchantId = merchantId });
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetAllTransactions
        public async Task<List<Transaction>> GetAllTransactions()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(conf.GetConnectionString("OptiosString")))
                {
                    var sqlQuery =
                        "SELECT Date_Of_Transaction AS Date, Amount, Amount_Equivalent as AmountEquivalent, Transaction_Status AS IsActive, CurrencyId, CategoryId, MerchantId, ChannelId FROM Transactions";
                    var transactions = await db.QueryAsync<Transaction>(sqlQuery);
                    return transactions.ToList();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region AddAsync
        public async Task<long> AddAsync(Merchant entity)
        {
            try
            {

                if (!await Dbset.AnyAsync(i => i.Name == entity.Name))
                {
                    await Dbset.AddAsync(entity);
                    await Context.SaveChangesAsync();
                    var max = await Dbset.MaxAsync(io => io.Id);
                    return max;
                }
                else
                {
                    throw new InvalidOperationException("Such a merchant already exists");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Merchant>> GetAllAsync()
        {
            try
            {
                if (Dbset.IsNullOrEmpty())
                {
                    throw new InvalidOperationException("No merchants found");
                }
                else
                {
                    return await Dbset
                        .AsNoTracking()
                        .ToListAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetAllActiveMerchantAsync
        public async Task<IEnumerable<Merchant>> GetAllActiveMerchantAsync()
        {
            try
            {
                var store = await Dbset.AsNoTracking().Where(i => i.IsActive).ToListAsync();
                return store;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region GetByIdAsync
        public async Task<Merchant> GetByIdAsync(long id)
        {
            try
            {
                var store = await Dbset.FindAsync(id) ?? throw new InvalidOperationException("No merchant found");
                return store;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Merchant entity)
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
                var store = await Dbset.FindAsync(id) ?? throw new InvalidOperationException("No merchant found");

                store.IsActive = false;
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
        public async Task<bool> UpdateAsync(long id, Merchant entity)
        {
            try
            {
                var store = await Dbset.FindAsync(id) ?? throw new InvalidOperationException("No merchant found");
                store.Name = entity.Name;
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
