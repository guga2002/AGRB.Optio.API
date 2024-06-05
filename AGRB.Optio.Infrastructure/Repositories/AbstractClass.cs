using Optio.Core.Data;

namespace Optio.Core.Repositories
{
    public abstract class AbstractClass(OptioDB optioDB)
    {
        protected readonly OptioDB Context = optioDB;
    }

}
