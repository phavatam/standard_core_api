using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IPermissionBusiness
    {
        void AddPerm(Guid ItemId, Guid? DepartmentId, Guid? RoleId, Guid? UserId, PermEnum perm);
        void RemovePermissonEdit(Guid ItemId);
        Task<PermEnum> GetItemPerm(Guid UserId, Guid ItemId);
    }
}
