using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Responses;
using AGRB.Optio.Application.StaticFiles;
using AGRB.Optio.Domain.Custom_Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RGBA.Optio.UI.Controllers
{
    /// <summary>
    /// Controller for Transaction Related Actions
    /// </summary>
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionRelatedController : ControllerBase
    {
        private readonly ITransactionRelatedService se;
        /// Initializes a new instance of the <see cref="TransactionRelatedController"/> class.
        /// <param name="se">The Transaction related service.</param>
        public TransactionRelatedController(ITransactionRelatedService se)
        {
            this.se = se;
                
        }

        /// <summary>
        /// Add chanell to Db V2.0
        /// </summary>
        /// <returns>A response containing  ActionResult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> Channel([FromBody] ChannelModel entity)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(entity.ChannelType);

            }
            var res = await se.AddAsync(entity);
            if (res == -1) return BadRequest(entity);
            return Ok(res);
        }

        /// <summary>
        ///Get all  Category from db V2.0
        /// </summary>
        /// <returns>A response containing a lsit of Category model </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> Category([FromBody] CategoryModel entity)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(entity.TransactionCategory);
            }
            var res = await se.AddAsync(entity);
            return res == -1 ? Ok(res) : BadRequest(entity.TransactionCategory);
        }

        /// <summary>
        ///Insert Transaction Type V2.0
        /// </summary>
        /// <returns>A response containing a ActionResult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> TransactionType([FromBody] TransactionTypeModel entity)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(entity.TransactionName);

            }
            var res = await se.AddAsync(entity);
            return res == -1 ? Ok(res) : BadRequest(entity.TransactionName);
        }

        /// <summary>
        ///Get all  ActiveChanells V2.0
        /// </summary>
        /// <returns>A response containing a lsit of Active CHanells  </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> ActiveChannel()
        {
            var res = await se.GetAllActiveAsync(new ChannelModel() { ChannelType = DefaultText.NoValue }); 
            return Ok(res);
        }

        /// <summary>
        ///Get all active  Categories V2.0
        /// </summary>
        /// <returns>A response containing a lsit of active categories </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> ActiveCategories()
        {
            var res = await se.GetAllActiveAsync(new CategoryModel() { TransactionCategory = DefaultText.NotDefined, TransactionTypeId = 0 });
            return Ok(res);
        }

        /// <summary>
        ///Get all  active transaction types V1.0
        /// </summary>
        /// <returns>A response containing a lsit of active transaction types </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ActiveTransactions()
        {
            var res = await se.GetAllActiveAsync(new TransactionTypeModel() { TransactionName = DefaultText.NoText });
            return Ok(res);
        }

        /// <summary>
        ///Get all  Chanells V2.0
        /// </summary>
        /// <returns>A response containing a lsit of Chanells </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> Channels()
        {
            var res = await se.GetAllAsync(new ChannelModel() { ChannelType = DefaultText.NotDefined });
            return Ok(res);
        }

        /// <summary> 
        ///Get all  Active Categories V2.0
        /// </summary>
        /// <returns>A response containing a lsit of active categories </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> AllCategoryAsync()
        {
            var res = await se.GetAllAsync(new CategoryModel() { TransactionCategory = DefaultText.NoText, TransactionTypeId = 0 });
            return Ok(res);
        }

        /// <summary>
        ///Get all  Transaction types V2.0
        /// </summary>
        /// <returns>A response containing a lsit of Transaction types </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> AllTransactionTypes()
        {
            var res = await se.GetAllAsync(new TransactionTypeModel() { TransactionName = DefaultText.NotDefined });
            return Ok(res);
        }

        /// <summary>
        ///Get Chanells by Id V2.0
        /// </summary>
        /// <returns>A response containing chanell model </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<ChannelModel>> ChannelById([FromRoute] long id)
        {
            var res = await se.GetByIdAsync(id, new ChannelModel() { ChannelType = DefaultText.NoText });

            return Response<ChannelModel>.Ok(res);
        }

        /// <summary>
        ///Get Category by Id V2.0
        /// </summary>
        /// <returns>A response containing Category model </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> CategoryById([FromRoute] long id)
        {
            var res = await se.GetByIdAsync(id, new CategoryModel() { TransactionCategory = DefaultText.NoValue, TransactionTypeId = 0 });
            return Ok(res);
        }

        /// <summary>
        ///Get TransactionType by Id V2.0
        /// </summary>
        /// <returns>A response containing TransactionType model </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> TransactionTypeById([FromRoute] long id)
        {
            var res = await se.GetByIdAsync(id, new TransactionTypeModel() { TransactionName = DefaultText.NoValue });
            return Ok(res);
        }

        /// <summary>
        ///Delete Chanell from db V2.0
        /// </summary>
        /// <returns>A response containing action result  </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpDelete]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> ChannelAsync([FromRoute] long id)
        {
            var res = await se.RemoveAsync(id, new ChannelModel() { ChannelType = DefaultText.NoText });
            return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
        }

        /// <summary>
        ///Delete category V2.0
        /// </summary>
        /// <returns>A response containing a Actionresult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpDelete]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> DeleteCategory([FromRoute] long id)
        {
            var res = await se.RemoveAsync(id, new CategoryModel() { TransactionCategory = DefaultText.NotDefined, TransactionTypeId = 34, });
            return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
        }

        /// <summary>
        ///Delete Transaction Type V2.0
        /// </summary>
        /// <returns>A response containing a Actionresult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpDelete]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> TransactionType([FromRoute] long id)
        {
            var res = await se.RemoveAsync(id, new TransactionTypeModel() { TransactionName = DefaultText.NoValue });
            if (res)
            {
                return Ok(res);
            }
            return BadRequest(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Delete chanells soft V1.0
        /// </summary>
        /// <returns>A response containing a Actionresult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ChannelSoftDeleteAsync([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(ErrorKeys.BadRequest);
            }
            var res = await se.SoftDeleteAsync(id, new ChannelModel() { ChannelType = DefaultText.NoValue });
            return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// soft delete category V1.0
        /// </summary>
        /// <returns>A response containing a Actionresult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CategorySoftDelete([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(ErrorKeys.BadRequest);
            }
            var res = await se.SoftDeleteAsync(id, new CategoryModel() { TransactionTypeId = 0, TransactionCategory = DefaultText.NotDefined });
            return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// soft delete Transaction Types V1.0
        /// </summary>
        /// <returns>A response containing a Actionresult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> TransactionTypeSoftDelete([FromRoute] long id)
        {
            var res = await se.SoftDeleteAsync(id, new TransactionTypeModel() { TransactionName = DefaultText.NoValue });
            return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Update chanell details V2.0
        /// </summary>
        /// <returns>A response containing a Actionresult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPut]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] ChannelModel entity)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(ErrorKeys.BadRequest);
            }
            var res = await se.UpdateAsync(id, entity);
            return res ? Ok(res) : BadRequest(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Update Category Details V2.0
        /// </summary>
        /// <returns>A response containing a Actionresult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPut]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> UpdateCategory([FromRoute] long id, [FromBody] CategoryModel entity)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(ErrorKeys.BadRequest);
            }
            var res = await se.UpdateAsync(id, entity);
            return res ? Ok(res) : BadRequest(ErrorKeys.NotFound);
        }


        /// <summary>
        /// Update TransactionType V2.0
        /// </summary>
        /// <returns>A response containing a Actionresult </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPut]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> UpdateTransactionType([FromRoute] long id, [FromBody] TransactionTypeModel entity)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(ErrorKeys.BadRequest);
            }
            var res = await se.UpdateAsync(id, entity);
            return res ? Ok(res) : BadRequest(ErrorKeys.NotFound);
        }
    }
}
