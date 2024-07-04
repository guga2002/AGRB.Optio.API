using AutoMapper;
using Microsoft.Extensions.Logging;
using AGRB.Optio.Domain.Interfaces;

namespace AGRB.Optio.Application.Services
{
    public abstract class AbstractService<T>(IUniteOfWork work, IMapper map, ILogger<T> log)
        where T : class
    {
        protected readonly IUniteOfWork work = work;
        protected readonly IMapper mapper = map;
        protected readonly ILogger<T> logger = log;
    }
}
