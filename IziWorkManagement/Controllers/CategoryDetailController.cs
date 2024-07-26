using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWork.Common.Args;
using IziWork.Common.DTO;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryDetailController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly ICategoryDetailBusiness _categoryDetailBusiness;
        #endregion
        public CategoryDetailController(
                    ICategoryDetailBusiness categoryDetailBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _categoryDetailBusiness = categoryDetailBusiness;
        }

        [HttpPost("GetListCategory")]
        public async Task<IActionResult> GetListCategory(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _categoryDetailBusiness.GetListCategory(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        #region INSERT UPDATE DELETE
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate(CategoryDetailArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _categoryDetailBusiness.CreateOrUpdate(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _categoryDetailBusiness.DeleteById(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion

        #region GET DATA
        [HttpPost("GetListCategoryDetail")]
        public async Task<IActionResult> GetListCategoryDetail(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _categoryDetailBusiness.GetListCategoryDetail(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _categoryDetailBusiness.GetById(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
    }
}
