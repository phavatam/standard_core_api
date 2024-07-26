using IziWork.Data.Entities;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Repositories.Business.IRepositories;
using Core.Repositories.Business.Interface;
using IziWork.Common.Enums; 

namespace Core.Repositories.Business.Handlers
{
    public class PermissionBusiness : IPermissionBusiness
    {
        private readonly IUnitOfWork _uow;
        public PermissionBusiness(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void AddPerm(Guid ItemId, Guid? DepartmentId, Guid? RoleId, Guid? UserId, PermEnum perm)
        {
            var newPerm = new Permission()
            {
                ItemId = ItemId,
                DepartmentId = DepartmentId,
                RoleId = RoleId,
                UserId = UserId,
                Perm = (int)perm
            };
            _uow.GetRepository<Permission>().Add(newPerm);
        }
        public void RemovePermissonEdit(Guid ItemId)
        {
            var perEdit = _uow.GetRepository<Permission>().FindBy(x => x.ItemId == ItemId && x.Perm != null && x.Perm.Value == (int)PermEnum.Edit);
            foreach (var perm in perEdit)
            {
                perm.Perm = (int)PermEnum.View;
                _uow.GetRepository<Permission>().Update(perm);
            }
        }

        public void RemovePerm(Guid ItemId, Guid DepartmentId)
        {
            var findPermisson = _uow.GetRepository<Permission>().GetSingle(x => x.ItemId == ItemId && x.DepartmentId == DepartmentId);
            if (findPermisson != null)
            {
                _uow.GetRepository<Permission>().Delete(findPermisson);
            }
        }

        public async Task<PermEnum> GetItemPerm(Guid UserId, Guid itemId)
        {
            var perms = await _uow.GetRepository<Permission>().FindByAsync(x => x.ItemId == itemId
            && (x.UserId == UserId
                || x.Department.UserDepartmentMappings.Any(t => t.UserId == UserId && t.UserDepartmentRoleMappings.Any(j => j.RoleId == x.RoleId))));
            var right = PermEnum.None;
            foreach (var perm in perms)
            {
                right |= (PermEnum)perm.Perm;
            }
            return right;
        }
    }
}
