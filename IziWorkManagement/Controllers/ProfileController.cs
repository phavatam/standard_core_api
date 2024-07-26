using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using IziWork.Common.Args;
using IziWork.Common.DTO;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IProfileBusiness _profileBusiness;
        #endregion
        public ProfileController(
                    IProfileBusiness profileBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _profileBusiness = profileBusiness;
        }

        #region INSERT UPDATE DELETE
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate(ProfileArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _profileBusiness.CreateOrUpdate(args);
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
            var result = await _profileBusiness.DeleteById(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion

        #region GET DATA
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _profileBusiness.GetList(args);
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
            var result = await _profileBusiness.GetById(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
    }
}
