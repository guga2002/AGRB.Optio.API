using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Responses;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionRelatedController(ITransactionRelatedService se, ILogger<TransactionRelatedController> log) : ControllerBase
    {
        [HttpPost]
        [Route(nameof(AddChannelAsync))] 
        public async Task<IActionResult> AddChannelAsync([FromBody] ChannelModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(entity.ChannelType);

                }
                var res = await se.AddAsync(entity);
                if (res == -1) return BadRequest(entity);
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route(nameof(AddCategoryAsync))]
        public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(entity.TransactionCategory);
                }
                var res = await se.AddAsync(entity);
                return res == -1 ? Ok(res) : BadRequest(entity.TransactionCategory);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route(nameof(TransactionType))] 
        public async Task<IActionResult> TransactionType([FromBody] TransactionTypeModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(entity.TransactionName);

                }
                var res = await se.AddAsync(entity);
                return res == -1 ? Ok(res) : BadRequest(entity.TransactionName);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> AllActiveChannelAsync()
        {
            try
            {
                var res = await se.GetAllActiveAsync(new ChannelModel() { ChannelType = DefaultText.noValue }); return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> AllActiveCategoryAsync()
        {
            try
            {
                var res = await se.GetAllActiveAsync(new CategoryModel() { TransactionCategory = DefaultText.notDefined, TransactionTypeId = 0 });
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route(nameof(AllActiveTransactionAsync))]
        public async Task<IActionResult> AllActiveTransactionAsync()
        {
            try
            {
                var res = await se.GetAllActiveAsync(new TransactionTypeModel() { TransactionName = DefaultText.noText });
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route(nameof(AllChannelAsync))]
        public async Task<IActionResult> AllChannelAsync()
        {
            try
            {
                var res = await se.GetAllAsync(new ChannelModel() { ChannelType = DefaultText.notDefined });
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route(nameof(AllCategoryAsync))]
        public async Task<IActionResult> AllCategoryAsync()
        {
            try
            {
                var res = await se.GetAllAsync(new CategoryModel() { TransactionCategory = DefaultText.noText, TransactionTypeId = 0 });
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route(nameof(AllTransactionTypeAsync))]
        public async Task<IActionResult> AllTransactionTypeAsync()
        {
            try
            {
                var res = await se.GetAllAsync(new TransactionTypeModel() { TransactionName =DefaultText.notDefined});
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route(nameof(GetChannelByIdAsync))]
        public async Task<Response<ChannelModel>> GetChannelByIdAsync([FromRoute] long id)
        {
            try
            {
                var res = await se.GetByIdAsync(id, new ChannelModel() { ChannelType = DefaultText.noText });


                return Response<ChannelModel>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<ChannelModel>.Error("Error", "r");
            } 
        }

        [HttpGet]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> CategoryByIdAsync([FromRoute] long id)
        {
            try
            {
                var res = await se.GetByIdAsync(id, new CategoryModel() { TransactionCategory = DefaultText.noValue, TransactionTypeId = 0 });
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> GTransactionTypeByIdAsync([FromRoute] long id)
        {
            try
            {
                var res = await se.GetByIdAsync(id, new TransactionTypeModel() { TransactionName =DefaultText.noValue });
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpDelete]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> ChannelAsync([FromRoute] long id)
        {
            try
            {
                var res = await se.RemoveAsync(id, new ChannelModel() { ChannelType = DefaultText.noText });
                return res  ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpDelete]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> CategoryAsync([FromRoute] long id)
        {
            try
            {
                var res = await se.RemoveAsync(id, new CategoryModel() { TransactionCategory = DefaultText.notDefined, TransactionTypeId = 34, });
                return res  ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpDelete]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> TransactionTypeAsync([FromRoute] long id)
        {
            try
            {
                var res = await se.RemoveAsync(id, new TransactionTypeModel() { TransactionName =DefaultText.noValue });
                if (res)
                {
                    return Ok(res);
                }

                return BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> ChannelSoftDeleteAsync([FromRoute] long id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(ErrorKeys.BadRequest);
                }
                var res = await se.SoftDeleteAsync(id, new ChannelModel() { ChannelType =DefaultText.noValue});
                return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> CategorySoftDeleteAsync([FromRoute] long id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(ErrorKeys.BadRequest);
                }
                var res = await se.SoftDeleteAsync(id, new CategoryModel() { TransactionTypeId = 0, TransactionCategory =DefaultText.notDefined });
                return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> TransactionTypeSoftDeleteAsync([FromRoute] long id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(ErrorKeys.BadRequest);
                }
                var res = await se.SoftDeleteAsync(id, new TransactionTypeModel() { TransactionName =DefaultText.noValue });
                return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPut]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] ChannelModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(ErrorKeys.BadRequest);
                }
                var res = await se.UpdateAsync(id, entity);
                return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPut]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromRoute] long id, [FromBody] CategoryModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(ErrorKeys.BadRequest); 
                }
                var res = await se.UpdateAsync(id, entity);
                return res ? Ok(res) : BadRequest(ErrorKeys.NotFound);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPut]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> UpdateTransactionTypeAsync([FromRoute] long id, [FromBody] TransactionTypeModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(ErrorKeys.BadRequest);
                }
                var res = await se.UpdateAsync(id, entity);
                return res ? Ok(res) : BadRequest(ErrorKeys.NotFound);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }
    }
}
