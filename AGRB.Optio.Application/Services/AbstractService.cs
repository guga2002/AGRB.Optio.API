using RGBA.Optio.Core.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace RGBA.Optio.Domain.Services
{
    public abstract  class AbstractService<T> where T:class
    {
        protected readonly IUniteOfWork work;
        protected readonly IMapper mapper;
        protected readonly ILogger<T> logger;

        protected AbstractService(IUniteOfWork work,IMapper map,ILogger<T> log)
        {
            this.mapper = map;
            this.work = work;
            this.logger = log; 
        }
    }
}
