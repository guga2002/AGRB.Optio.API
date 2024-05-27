using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using Optio.Core.Entities;
using Optio.Core.Interfaces;
using System.Numerics;

namespace Optio.Core.Repositories
{
    public class TypeOfTransactionRepos : AbstractClass, ITypeOfTransactionRepo
    {
        private readonly DbSet<TypeOfTransaction> TypeOfTransaction;

        public TypeOfTransactionRepos(OptioDB optioDB) : base(optioDB)
        {
            TypeOfTransaction=context.Set<TypeOfTransaction>();
        }

        #region AddAsync
        public async Task<long> AddAsync(TypeOfTransaction entity)
        {
            try
            {
                if (!await TypeOfTransaction.AnyAsync(io => io.TransactionName == entity.TransactionName))
                {
                    await TypeOfTransaction.AddAsync(entity);
                    await context.SaveChangesAsync();
                    var max = await TypeOfTransaction.MaxAsync(io => io.Id);
                    return max;
                }
                return -1;
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
                return await TypeOfTransaction.
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
                return await TypeOfTransaction.
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
            return await TypeOfTransaction.FindAsync(id)?? throw new ArgumentNullException("Typeoftransaction No Exist");
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(TypeOfTransaction entity)
        {
            try
            {
                if (entity is not null)
                {
                    TypeOfTransaction.Remove(entity);
                    await context.SaveChangesAsync();
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

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(long id)
        {
            try
            {
                var typ = await TypeOfTransaction
                    .FirstOrDefaultAsync(io => io.Id == id && io.IsActive);

                if (typ is not null)
                {
                    typ.IsActive = false;
                    await context.SaveChangesAsync();
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

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, TypeOfTransaction entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                var existingEntity = await TypeOfTransaction.FindAsync(id);
                if (existingEntity is null)
                {
                    throw new InvalidOperationException("There is no such Type of transaction");
                }
                else
                {
                    existingEntity.TransactionName = entity.TransactionName;
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
