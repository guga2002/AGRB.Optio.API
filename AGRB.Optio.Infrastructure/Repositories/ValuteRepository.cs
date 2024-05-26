using Microsoft.EntityFrameworkCore;
using Optio.Core.Data;
using Optio.Core.Repositories;
using RGBA.Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;

namespace RGBA.Optio.Core.Repositories
{
    public class ValuteRepository : AbstractClass, IValuteCourse
    {
        private readonly DbSet<ValuteCourse> courses;
        public ValuteRepository(OptioDB optioDB) : base(optioDB)
        {
            courses = context.Set<ValuteCourse>();
        }

        #region AddAsync
        public async Task<long> AddAsync(ValuteCourse entity)
        {
            try
            {
                if(! await context.Currencies.AnyAsync(io=>io.Id==entity.CurrencyID))
                {
                    throw new ArgumentNullException(" no such a Currency  Exist!");
                }
                if (!await courses.AnyAsync(io => io.CurrencyID == entity.CurrencyID && io.DateOfValuteCourse == entity.DateOfValuteCourse))
                {
                    await courses.AddAsync(entity);
                    await context.SaveChangesAsync();
                    var res = await courses.MaxAsync(io => io.Id);
                    return res;
                }
                else
                {
                    throw new ArgumentException("Error while adding entity, Entity already exist in same Date");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<ValuteCourse>> GetAllAsync()
        {
            try
            {
                var res = await courses.AsNoTracking().ToListAsync();
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllActiveValuteAsync
        public async Task<IEnumerable<ValuteCourse>> GetAllActiveValuteAsync()
        {
            try
            {
                return await courses
                    .AsNoTracking()
                    .Where(io=>io.IsActive==true)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region GetByIdAsync
        public async Task<ValuteCourse> GetByIdAsync(long id)
        {
            try
            {
                return await courses.
                    AsNoTracking()
                    .FirstOrDefaultAsync(io => io.Id == id && io.IsActive==true) 
                    ?? throw new ArgumentException(" No  Valute Course exist On this ID :(");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(ValuteCourse entity)
        {
            try
            { 
                if (entity is not null)
                {
                    courses.Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                throw new ArgumentException("No such valute exist");
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
                var course = await courses
                    .FirstOrDefaultAsync(io => io.Id == id&&io.IsActive== true);

                if (course is not null)
                {
                    course.IsActive=false;
                    await context.SaveChangesAsync();
                    return true;
                }
                throw new ArgumentException(" no data exist or alrady soft deleted");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, ValuteCourse entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity,nameof(entity));
                var course = await courses.FindAsync(id);
                if (course is not null)
                {
                   course.DateOfValuteCourse = entity.DateOfValuteCourse;
                    course.IsActive=entity.IsActive;
                    course.DateOfValuteCourse = entity.DateOfValuteCourse;
                    course.CurrencyID = entity.CurrencyID;
                    course.ExchangeRate = entity.ExchangeRate;
                    await context.SaveChangesAsync();
                    return true;
                }
                throw new ArgumentException("The data is already up to data, or  such  a data no exist");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
        }
        #endregion
    }
}
