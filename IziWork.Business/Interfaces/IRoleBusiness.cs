using IziWork.Business.Args;
using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IRoleBusiness
    {
        Task<ResultDTO> GetListRoles(QueryArgs args);
        Task<ResultDTO> UpSertRole(RoleArgs args);
        Task<ResultDTO> DeleteRoleById(Guid roleId);
        Task<ResultDTO> GetRoleById(Guid roleId);
    }
}
