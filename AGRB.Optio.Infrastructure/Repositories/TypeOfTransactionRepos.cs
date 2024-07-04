using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public class TypeOfTransactionRepos : AbstractRepositroy<TypeOfTransaction>, ITypeOfTransactionRepo
    {

        public TypeOfTransactionRepos(OptioDB optioDB) : base(optioDB)
        {
        }

        #region AddAsync
        public async Task<long> AddAsync(TypeOfTransaction entity)
        {
            try
            {
                if (await Dbset.AnyAsync(io => io.TransactionName == entity.TransactionName)) throw new ArgumentException("Such Type already exist in DB");

                await Dbset.AddAsync(entity);
                await Context.SaveChangesAsync();
                var max = await Dbset.MaxAsync(io => io.Id);
                return max;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<TypeOfTransaction>> GetAllAsync()
        {
            try
            {
                return await Dbset.
                     AsNoTracking()
                     .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetAllActiveTypeOfTransactionAsync
        public async Task<IEnumerable<TypeOfTransaction>> GetAllActiveTypeOfTransactionAsync()
        {
            try
            {
                return await Dbset.
                     AsNoTracking().
                     Where(io => io.IsActive)
                     .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetByIdAsync

        public async Task<TypeOfTransaction> GetByIdAsync(long id)
        {
            return await Dbset.FindAsync(id) ??
                   throw new InvalidOperationException("TypeOfTransaction No Exist");
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(TypeOfTransaction entity)
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
                var typ = await Dbset
                              .FindAsync(id) ??
                          throw new InvalidOperationException("TypeOfTransaction No Exist");

                typ.IsActive = false;
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

        public async Task<bool> UpdateAsync(long id, TypeOfTransaction entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);
                var existingEntity = await Dbset.FindAsync(id) ??
                                     throw new InvalidOperationException("There is no such Type of transaction");

                existingEntity.TransactionName = entity.TransactionName;
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
