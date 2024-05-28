using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using Optio.Core.Entities;
using Optio.Core.Interfaces;

namespace Optio.Core.Repositories
{
    public class TypeOfTransactionRepos : AbstractClass, ITypeOfTransactionRepo
    {
        private readonly DbSet<TypeOfTransaction> typeOfTransaction;

        public TypeOfTransactionRepos(OptioDB optioDB) : base(optioDB)
        {
            typeOfTransaction=Context.Set<TypeOfTransaction>();
        }

        #region AddAsync
        public async Task<long> AddAsync(TypeOfTransaction entity)
        {
            try
            {
                if (await typeOfTransaction.AnyAsync(io => io.TransactionName == entity.TransactionName)) return -1;
                
                await typeOfTransaction.AddAsync(entity);
                await Context.SaveChangesAsync();
                var max = await typeOfTransaction.MaxAsync(io => io.Id);
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
                return await typeOfTransaction.
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
                return await typeOfTransaction.
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
            return await typeOfTransaction.FindAsync(id) ??
                   throw new InvalidOperationException("TypeOfTransaction No Exist");
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(TypeOfTransaction entity)
        {
            try
            {
                typeOfTransaction.Remove(entity);
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
                var typ = await typeOfTransaction
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
                var existingEntity = await typeOfTransaction.FindAsync(id) ??
                                     throw new InvalidOperationException("There is no such Type of transaction");
                
                existingEntity.TransactionName = entity.TransactionName;
                await Context.SaveChangesAsync();
                return true;
                
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
