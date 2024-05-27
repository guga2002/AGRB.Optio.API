using Optio.Core.Data;

namespace Optio.Core.Repositories
{
    public abstract class AbstractClass
    {
        protected readonly OptioDB context;
        protected AbstractClass(OptioDB optioDB)
        {
            context = optioDB;
        }
    }

}
