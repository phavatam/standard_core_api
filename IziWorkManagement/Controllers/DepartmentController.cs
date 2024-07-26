using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWork.Common.Args;
using IziWork.Common.DTO;
using IziWorkManagement.Authorize;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IDepartmentBusiness _departmentBusiness;
        #endregion
        public DepartmentController(
                    IDepartmentBusiness departmentBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _departmentBusiness = departmentBusiness;
        }

        #region INSERT - UPDATE - DELETE
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate(DepartmentArgs model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _departmentBusiness.CreateOrUpdate(model);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpDelete("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _departmentBusiness.DeleteDepartment(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
        #region GETDATA
        [HttpPost("GetListDepartment")]
        //[Authorize("ADMIN")]
        public async Task<IActionResult> GetListDepartment(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _departmentBusiness.GetListDepartment(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
        #region GETDATA
        [HttpPost("GetTreeDepartment")]
        //[Authorize("ADMIN")]
        public async Task<IActionResult> GetTreeDepartment(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _departmentBusiness.GetTreeDepartment(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion

        #region
        [HttpGet("GetListUserByDepartmentId")]
        public async Task<IActionResult> GetListUserByDepartmentId(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _departmentBusiness.GetListUserByDepartmentId(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("CreateUserInDepartment")]
        public async Task<IActionResult> CreateUserInDepartment(UserInDepartmentArgs model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _departmentBusiness.CreateUserInDepartment(model);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("UpdateUserInDepartment")]
        public async Task<IActionResult> UpdateUserInDepartment(UserInDepartmentArgs model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _departmentBusiness.UpdateUserInDepartment(model);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpDelete("DeleteUserInDepartment")]
        public async Task<IActionResult> DeleteUserInDepartment(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _departmentBusiness.DeleteUserInDepartment(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion

    }
}
