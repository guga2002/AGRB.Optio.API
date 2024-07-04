using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class ChannelRepos(OptioDB optioDB) : AbstractRepositroy<Channels>(optioDB), IChannelRepo
    {

        #region AddAsync
        public async Task<long> AddAsync(Channels entity)
        {
            try
            {
                var channel = await Dbset.AnyAsync(i => i.ChannelType == entity.ChannelType);
                if (!channel)
                {
                    await Dbset.AddAsync(entity);
                    await Context.SaveChangesAsync();
                    var res = await Dbset.MaxAsync(io => io.Id);
                    return res;
                }
                else
                {
                    throw new InvalidOperationException("A similar channel already exists");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region GetAllAsync
        Func<OptioDB, IEnumerable<Channels>> CompiledQueryGetAll =
            EF.CompileQuery(
                (OptioDB db) =>
                db.Channels.ToList()
                );

        public async Task<IEnumerable<Channels>> GetAllAsync()
        {
            try
            {
                return await Dbset.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region GetAllActiveChannelAsync

        public async Task<IEnumerable<Channels>> GetAllActiveChannelAsync()
        {
            try
            {
                return await Dbset.Where(io => io.IsActive)
                      .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetByIdAsync

        Func<OptioDB, long, Channels?> CompiledQueryGetBtId =
            EF.CompileQuery(
                (OptioDB db, long id) =>
                db.Channels.SingleOrDefault(i => i.Id == id)
                );
        public async Task<Channels> GetByIdAsync(long id)
        {
            try
            {
                var res = await Dbset.FindAsync(id) ?? throw new InvalidOperationException(" no entity found");
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Channels entity)
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
                var channel = await Dbset.FindAsync(id);
                if (channel is not null)
                {
                    channel.IsActive = false;
                    await Context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException("No similar channel found");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, Channels entity)
        {
            try
            {
                var channel = await Dbset.FindAsync(id);
                if (channel is not null)
                {
                    channel.ChannelType = entity.ChannelType;
                    await Context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException("No similar channel found");
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
