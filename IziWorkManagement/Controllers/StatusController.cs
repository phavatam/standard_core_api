using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IStatusBusiness _statusBusiness;
        #endregion
        public StatusController(
                    IStatusBusiness statusBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _statusBusiness = statusBusiness;
        }

        #region INSERT UPDATE DELETE
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate(StatusArgs model)
        {
            var result = await _statusBusiness.CreateOrUpdate(model);
            return Ok(result);
        }

        [HttpDelete("DeleteStatusById")]
        public async Task<IActionResult> DeleteStatusById(Guid Id)
        {
            var result = await _statusBusiness.DeleteStatusById(Id);
            return Ok(result);
        }
        #endregion

        #region GET DATA
        [HttpPost("GetListStatus")]
        public async Task<IActionResult> GetListStatus(QueryArgs args)
        {
            var result = await _statusBusiness.GetListStatus(args);
            return Ok(result);
        }
        [HttpGet("GetStatusById")]
        public async Task<IActionResult> GetStatusById(Guid Id)
        {
            var result = await _statusBusiness.GetStatusById(Id);
            return Ok(result);
        }
        #endregion
    }
}
