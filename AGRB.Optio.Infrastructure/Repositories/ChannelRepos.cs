using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using Optio.Core.Entities;
using Optio.Core.Interfaces;
using RGBA.Optio.Core.PerformanceImprovmentServices;

namespace Optio.Core.Repositories
{
    public class ChannelRepos(OptioDB optioDB, CacheService cacheService) : AbstractClass(optioDB), IChannelRepo
    {
      
        private readonly DbSet<Channels> channels = optioDB.Set<Channels>();


        #region AddAsync
        public async Task<long> AddAsync(Channels entity)
        {
            try
            { 
                var channel= await channels.AnyAsync(i=>i.ChannelType==entity.ChannelType);
                if (!channel)
                {
                    await channels.AddAsync(entity);
                    await Context.SaveChangesAsync();
                    var res= await channels.MaxAsync(io=>io.Id);
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
                const string cacheKey = "all channels";
                await Task.Delay(1);
                var ch = cacheService.GetOrCreate(cacheKey, () => CompiledQueryGetAll.Invoke(Context) ??
                                                                  throw new ArgumentException("No channel found"), TimeSpan.FromMinutes(30)); 
                return ch ?? throw new ArgumentException("No channel found");
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
              return await channels.Where(io => io.IsActive)
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
                db.Channels.SingleOrDefault(i=>i.Id==id)
                );
        public async Task<Channels> GetByIdAsync(long id)
        {
            try
            {
                var cacheKey = $"channel with {id}";
                await Task.Delay(1);
                var channel = cacheService.GetOrCreate(cacheKey, () => CompiledQueryGetBtId.Invoke(Context, id) ??
                                                                       throw new ArgumentException("No channel found"), TimeSpan.FromMinutes(15));

                return channel ?? throw new ArgumentException("No channel found");

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
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                channels.Remove(entity);
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
                var channel = await channels.FindAsync(id);
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
                ArgumentNullException.ThrowIfNull(entity,nameof(entity));
                var channel = await channels.FindAsync(id);
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
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch(Exception )
            {
                throw;
            }
        }
        #endregion
    }
}
