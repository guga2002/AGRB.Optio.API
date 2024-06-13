using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using Optio.Core.Repositories;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class FeadbackRepository : AbstractClass, IFeadbackRepository
    {
        private readonly DbSet<Feadback> feadbacks;
        public FeadbackRepository(OptioDB optioDB) : base(optioDB)
        {
            feadbacks = Context.Set<Feadback>();
        }

        public async Task<long> AddAsync(Feadback entity)
        {
            if (!await feadbacks.AnyAsync(io => io.UserId == entity.UserId && io.FeadBack == entity.FeadBack))
            {
                await feadbacks.AddAsync(entity);
                await Context.SaveChangesAsync();
                return feadbacks.Max(o => o.Id);
            }
            throw new ArgumentNullException("such review already exist in DB!");
        }

        public async Task<IEnumerable<Feadback>> GetAllAsync()
        {
            return await feadbacks.ToListAsync();
        }

        public async Task<Feadback> GetByIdAsync(long id)
        {
            var res = await feadbacks.FindAsync(id);
            if (res is not null)
            {
                return res;
            }
            throw new ArgumentNullException(" no entity found on this ID");
        }

        public async Task<bool> RemoveAsync(Feadback entity)
        {
            feadbacks.Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(long id)
        {
            var res = await feadbacks.FindAsync(id);
            if (res is not null)
            {
                res.Status = true;
                await Context.SaveChangesAsync();
                return true;
            }
            throw new ArgumentNullException(" no entity found on this ID");
        }

        public async Task<bool> UpdateAsync(long id, Feadback entity)
        {
            var res = await feadbacks.FindAsync(id);
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
    }
}
