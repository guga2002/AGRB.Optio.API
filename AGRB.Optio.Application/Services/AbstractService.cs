using RGBA.Optio.Core.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace RGBA.Optio.Domain.Services
{
    public abstract class AbstractService<T>(IUniteOfWork work, IMapper map, ILogger<T> log)
        where T : class
    {
        protected readonly IUniteOfWork work = work;
        protected readonly IMapper mapper = map;
        protected readonly ILogger<T> logger = log;
    }
}
