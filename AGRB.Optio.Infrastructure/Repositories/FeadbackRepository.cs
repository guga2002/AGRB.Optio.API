using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class FeadbackRepository : AbstractRepositroy<Feadback>, IFeadbackRepository
    {
        public FeadbackRepository(OptioDB optioDB) : base(optioDB)
        {
        }

        #region AddAsync
        public async Task<long> AddAsync(Feadback entity)
        {
            if (!await Dbset.AnyAsync(io => io.UserId == entity.UserId && io.FeadBack == entity.FeadBack))
            {
                await Dbset.AddAsync(entity);
                await Context.SaveChangesAsync();
                return Dbset.Max(o => o.Id);
            }
            throw new ArgumentNullException("such review already exist in DB!");
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Feadback>> GetAllAsync()
        {
            return await Dbset.ToListAsync();
        }
        #endregion

        #region GetByIdAsync
        public async Task<Feadback> GetByIdAsync(long id)
        {
            var res = await Dbset.FindAsync(id);
            if (res is not null)
            {
                return res;
            }
            throw new ArgumentNullException(" no entity found on this ID");
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Feadback entity)
        {
            Dbset.Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(long id)
        {
            var res = await Dbset.FindAsync(id);
            if (res is not null)
            {
                res.Status = true;
                await Context.SaveChangesAsync();
                return true;
            }
            throw new ArgumentNullException(" no entity found on this ID");
        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(long id, Feadback entity)
        {
            var res = await Dbset.FindAsync(id);
            if (res is not null)
            {
                res.Email = entity.Email;
                res.Status = entity.Status;
                res.FeadbackDate = entity.FeadbackDate;
                res.RatingGivedByUser = entity.RatingGivedByUser;
                res.Name = entity.Name;
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
    }
}
