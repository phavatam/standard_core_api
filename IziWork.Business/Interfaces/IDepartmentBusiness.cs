using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Common.Args;
using IziWork.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IDepartmentBusiness
    {
        Task<ResultDTO> CreateOrUpdate(DepartmentArgs args);
        Task<ResultDTO> DeleteDepartment(Guid Id);
        Task<ResultDTO> GetListDepartment(QueryArgs args);
        Task<ResultDTO> GetTreeDepartment(QueryArgs args);

        #region User In Department
        Task<ResultDTO> GetListUserByDepartmentId(Guid DepartmentId);
        Task<ResultDTO> CreateUserInDepartment(UserInDepartmentArgs args);
        Task<ResultDTO> UpdateUserInDepartment(UserInDepartmentArgs args);
        Task<ResultDTO> DeleteUserInDepartment(Guid Id);
        #endregion

    }
}
