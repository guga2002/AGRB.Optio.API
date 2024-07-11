using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Application.StaticFiles;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AGRB.Optio.Application.Services
{
    public class FeadbackService : AbstractService<FeadbackService>, IFeadbackService
    {
        public FeadbackService(IUniteOfWork work, IMapper map, ILogger<FeadbackService> log) : base(work, map, log)
        {
        }

        #region AddAsync
        public async Task<long> AddAsync(FeadbackModel entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            var mapped = mapper.Map<Feadback>(entity);
            if(mapped is not null)
            {
               return await  work.FeadbackRepository.AddAsync(mapped);
            }
            throw new ArgumentNullException(ErrorKeys.InternalServerError);
        }
        #endregion

        #region GetAllActiveAsync
        public async Task<IEnumerable<FeadbackModel>> GetAllActiveAsync(FeadbackModel identify)
        {
            var ser =  await work.FeadbackRepository.GetAllAsync();
            if(ser.Any())
            {
                var filtered = ser.Where(io => io.Status == true).ToList();
                var mapped=mapper.Map<IEnumerable<FeadbackModel>>(filtered);
                return mapped;
            }
            throw new ArgumentNullException(ErrorKeys.NotFound);
        }
        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<FeadbackModel>> GetAllAsync(FeadbackModel identify)
        {
            var ser = await work.FeadbackRepository.GetAllAsync();
            if (ser.Any())
            {
                var mapped = mapper.Map<IEnumerable<FeadbackModel>>(ser);
                return mapped;
            }
            throw new ArgumentNullException(ErrorKeys.NotFound);
        }

        #endregion

        #region GetByIdAsync
        public async Task<FeadbackModel> GetByIdAsync(long id, FeadbackModel identify)
        {
            var ser = await work.FeadbackRepository.GetByIdAsync(id);
            if (ser is not null)
            {
                var mapped = mapper.Map<FeadbackModel>(ser);
                return mapped;
            }
            throw new ArgumentNullException(ErrorKeys.NotFound);
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(long id, FeadbackModel identity)
        {
            var feadbback = await work.FeadbackRepository.GetByIdAsync(id);
            if (feadbback is not null)
            {
              return  await  work.FeadbackRepository.RemoveAsync(feadbback);
            }
            throw new ArgumentNullException(ErrorKeys.NotFound);
        }
        #endregion

        #region SoftDeleteAsync

        public async Task<bool> SoftDeleteAsync(long id, FeadbackModel identify)
        {
            var feadbback = await work.FeadbackRepository.SoftDeleteAsync(id);
            return feadbback;
        }
        #endregion
    }
}
