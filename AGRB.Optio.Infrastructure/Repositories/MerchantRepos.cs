using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Optio.Core.Data;
using Optio.Core.Entities;
using Optio.Core.Interfaces;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;


namespace Optio.Core.Repositories
{
    public class MerchantRepos : AbstractClass, IMerchantRepo
    {
        private readonly DbSet<Merchant> merchant;
        private readonly IConfiguration conf;

        public MerchantRepos(OptioDB optioDB, IConfiguration conf):base(optioDB)
        {
            merchant = context.Set<Merchant>(); 
            this.conf= conf;
        }

        #region AssignLocationtoMerchant
        public async Task<bool> AssignLocationtoMerchant(long Merchantid,long Locationid)
        {
            try
            {
                if(await merchant.AnyAsync(io=>io.Id==Merchantid)&&await context.Locations.AnyAsync(io=>io.Id==Locationid))
                {
                    using (IDbConnection db = new SqlConnection(conf.GetConnectionString("OptiosString")))
                    {
                        string sqlQuery = "INSERT INTO LocationToMerchants (LocatrionId, merchantId) VALUES (@LocatrionId, @merchantId)";
                        await db.ExecuteAsync(sqlQuery, new { LocatrionId = Locationid, merchantId = Merchantid });
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region getalltransactions
        public async Task<List<Transaction>> getalltransactions()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(conf.GetConnectionString("OptiosString")))
                {

                    string sqlQuery ="SELECT Date_Of_Transaction AS Date, Amount, Amount_Equivalent as AmountEquivalent, Transaction_Status AS IsActive, CurrencyId, CategoryId, MerchantId, ChannelId FROM Transactions";
                    var transactions = await db.QueryAsync<Transaction>(sqlQuery);

                    return transactions.ToList<Transaction>();
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
                var obj = await merchant.SingleOrDefaultAsync(i => i.Name == entity.Name);
                if (obj == null)
                {
                    await merchant.AddAsync(entity);
                    await context.SaveChangesAsync();
                    var max = await merchant.MaxAsync(io => io.Id);
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
                if (merchant.IsNullOrEmpty())
                {
                    throw new InvalidOperationException("No merchants found");
                }
                else
                {
                    return await merchant
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
                var store = await merchant.AsNoTracking().Where(i => i.IsActive == true).ToListAsync();
                if (store == null)
                {
                    throw new InvalidOperationException("No active merchants found");
                }
                else
                {
                    return store;
                }

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
                var store = await merchant.FindAsync(id);
                if (store is null)
                {
                    throw new InvalidOperationException("No merchant found");
                }
                else
                {
                    return store;
                }

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
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                merchant.Remove(entity);
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
                var store=await merchant.FindAsync(id);
                if (store is null)
                {
                    throw new InvalidOperationException("No merchant found");
                }
                else
                {
                    store.IsActive = false;
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
        public async Task<bool> UpdateAsync(long id, Merchant entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity,nameof(entity));
                var store = await merchant.FindAsync(id);
                if (store is null)
                {
                    throw new InvalidOperationException("No merchant found");
                }
                else
                {
                    store.Name = entity.Name;
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
