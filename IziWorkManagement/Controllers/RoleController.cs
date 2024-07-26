using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
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
    public class RoleController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IRoleBusiness _roleBusiness;
        #endregion
        public RoleController(
                    IRoleBusiness roleBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _roleBusiness = roleBusiness;
        }
        #region INSERT - UPDATE - DELETE

        [HttpPost("GetListRoles")]
        public async Task<IActionResult> GetListRoles(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _roleBusiness.GetListRoles(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("UpsertRole")]
        public async Task<IActionResult> UpSertRole(RoleArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _roleBusiness.UpSertRole(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpDelete("DeleteRoleById")]
        public async Task<IActionResult> DeleteRoleById(Guid userId)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _roleBusiness.DeleteRoleById(userId);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
    }
}
