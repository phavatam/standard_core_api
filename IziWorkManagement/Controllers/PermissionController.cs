using Core.Repositories.Business.Interface;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWorkManagement.Authorize;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using IziWork.Common.Args;
using IziWork.Common.DTO;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IPermissionBusiness _permission;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion
        public PermissionController(
                    IPermissionBusiness permission,
                    IOptionsMonitor<AppSettingModel> optionsMonitor,
                    IHttpContextAccessor httpContextAccessor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _permission = permission;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetPerm")]
        public async Task<IActionResult> GetPerm(Guid ItemId)
        {
            ResultDTO resultDTO = new ResultDTO();
            var currentUserId = (_httpContextAccessor.HttpContext?.Items["UUID"] as Guid?) ?? Guid.Empty;
            var result = await _permission.GetItemPerm(currentUserId, ItemId);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(resultDTO);
        }
    }
}
