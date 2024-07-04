using AGRB.Optio.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace AGRB.Optio.Infrastructure.Repositories
{
    public abstract class AbstractRepositroy<T> where T : class
    {
        public virtual OptioDB Context { get; set; }

        public virtual DbSet<T> Dbset { get; set; }

        protected AbstractRepositroy(OptioDB db)
        {
            Context = db;
            Dbset = Context.Set<T>();
        }
    }

}
