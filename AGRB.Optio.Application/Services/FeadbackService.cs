using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Domain.Services;

namespace AGRB.Optio.Application.Services
{
    public class FeadbackService : AbstractService<FeadbackService>, IFeadbackService
    {
        public FeadbackService(IUniteOfWork work, IMapper map, ILogger<FeadbackService> log) : base(work, map, log)
        {
        }

        public async Task<long> AddAsync(FeadbackModel entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            var mapped = mapper.Map<Feadback>(entity);
            if(mapped is not null)
            {
               return await  work.FeadbackRepository.AddAsync(mapped);
            }
            throw new ArgumentNullException(" somethings wrong");
        }

        public async Task<IEnumerable<FeadbackModel>> GetAllActiveAsync(FeadbackModel identify)
        {
            var ser =  await work.FeadbackRepository.GetAllAsync();
            if(ser.Any())
            {
                var filtered = ser.Where(io => io.Status == true).ToList();
                var mapped=mapper.Map<IEnumerable<FeadbackModel>>(filtered);
                return mapped;
            }
            throw new ArgumentNullException(" no entitites found!");
        }

        public async Task<IEnumerable<FeadbackModel>> GetAllAsync(FeadbackModel identify)
        {
            var ser = await work.FeadbackRepository.GetAllAsync();
            if (ser.Any())
            {
                var mapped = mapper.Map<IEnumerable<FeadbackModel>>(ser);
                return mapped;
            }
            throw new ArgumentNullException(" no entitites found!");
        }

        public  async Task<FeadbackModel> GetByIdAsync(long id, FeadbackModel identify)
        {
            var ser = await work.FeadbackRepository.GetByIdAsync(id);
            if (ser is not null)
            {
                var mapped = mapper.Map<FeadbackModel>(ser);
                return mapped;
            }
            throw new ArgumentNullException(" no entitites found!");
        }

        public async Task<bool> RemoveAsync(long id, FeadbackModel identity)
        {
            var feadbback = await work.FeadbackRepository.GetByIdAsync(id);
            if (feadbback is not null)
            {
              return  await  work.FeadbackRepository.RemoveAsync(feadbback);
            }
            throw new ArgumentNullException("No entity found on this ID");
        }

        public  async Task<bool> SoftDeleteAsync(long id, FeadbackModel identify)
        {
            var feadbback = await work.FeadbackRepository.SoftDeleteAsync(id);
            return feadbback;
        }
    }
}
