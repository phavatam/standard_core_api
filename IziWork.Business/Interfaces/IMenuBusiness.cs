using IziWork.Business.Args;
using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IMenuBusiness
    {
        Task<ResultDTO> CreateOrUpdateMenu(MenuArgs args);
        Task<ResultDTO> DeleteMenu(Guid Id);
        Task<ResultDTO> GetListMenu(QueryArgs args);
        Task<ResultDTO> GetListMenuTree(QueryArgs args);
        Task<ResultDTO> GetListUserByMenuId(Guid Id);
        Task<ResultDTO> CreateOrUpdateUser(UserInMenuArgs args);
        Task<ResultDTO> DeleteUserInMenu(Guid Id);
        Task<ResultDTO> GetListDepartmentByMenuId(Guid Id);
        Task<ResultDTO> CreateOrUpdateDepartment(DepartmentInMenuArgs args);
        Task<ResultDTO> DeleteDepartmentInMenu(Guid Id);
        Task<ResultDTO> GetListRoleByMenuId(Guid Id);
        Task<ResultDTO> CreateOrUpdateRole(RoleInMenuArgs args);
        Task<ResultDTO> DeleteRoleInMenu(Guid Id);

    }
}
