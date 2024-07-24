using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IMenuBusiness _menuBusiness;
        #endregion
        public MenuController(
                    IMenuBusiness menuBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _menuBusiness = menuBusiness;
        }
        #region  INSERT - UPDATE - DELETE
        [HttpPost("CreateOrUpdateMenu")]
        public async Task<IActionResult> CreateOrUpdateMenu(MenuArgs model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.CreateOrUpdateMenu(model);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        [HttpDelete("DeleteMenu")]
        public async Task<IActionResult> DeleteMenu(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.DeleteMenu(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion

        #region GET DATA
        [HttpPost("GetListMenuTree")]
        public async Task<IActionResult> GetListMenuTree(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.GetListMenuTree(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        [HttpPost("GetListMenu")]
        public async Task<IActionResult> GetListMenu(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.GetListMenu(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
        #region USER
        [HttpGet("GetListUserByMenuId")]
        public async Task<IActionResult> GetListUserByMenuId(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.GetListUserByMenuId(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("CreateOrUpdateUser")]
        public async Task<IActionResult> CreateOrUpdateUser(UserInMenuArgs model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.CreateOrUpdateUser(model);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpDelete("DeleteUserInMenu")]
        public async Task<IActionResult> DeleteUserInMenu(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.DeleteUserInMenu(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
        #region DEPARTMENT
        [HttpGet("GetListDepartmentByMenuId")]
        public async Task<IActionResult> GetListDepartmentByMenuId(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.GetListDepartmentByMenuId(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("CreateOrUpdateDepartment")]
        public async Task<IActionResult> CreateOrUpdateDepartment(DepartmentInMenuArgs model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.CreateOrUpdateDepartment(model);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpDelete("DeleteDepartmentInMenu")]
        public async Task<IActionResult> DeleteDepartmentInMenu(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.DeleteDepartmentInMenu(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
        #region ROLE
        [HttpGet("GetListRoleByMenuId")]
        public async Task<IActionResult> GetListRoleByMenuId(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.GetListRoleByMenuId(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("CreateOrUpdateRole")]
        public async Task<IActionResult> CreateOrUpdateRole(RoleInMenuArgs model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.CreateOrUpdateRole(model);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        [HttpDelete("DeleteRoleInMenu")]
        public async Task<IActionResult> DeleteRoleInMenu(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _menuBusiness.DeleteRoleInMenu(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion

    }
}
