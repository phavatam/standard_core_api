using Core.Repositories.Business.IRepositories;
using IziWork.Common.Enums;
using IziWork.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IziWorkManagement.Authorize
{
    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly string _permission;
        private readonly IUnitOfWork _uow;

        public AuthorizeActionFilter(string permission, IUnitOfWork uow)
        {
            _permission = permission;
            _uow = uow;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUserId = (context.HttpContext.Items["UUID"] as Guid?) ?? Guid.Empty;
            if (currentUserId != Guid.Empty)
            {
                var currentUser = _uow.GetRepository<User>().GetSingle(x => x.Id == currentUserId);
                if (currentUser != null)
                {
                    if (_permission == "ADMIN")
                    {
                        bool isAuthorized = CheckAdminPermission(currentUser.Role.Value);
                        if (!isAuthorized)
                        {
                            context.Result = new JsonResult(new
                            {
                                Success = false,
                                Fail = true,
                                Message = ""
                            })
                            { StatusCode = StatusCodes.Status401Unauthorized };
                        }
                    }
                    if (_permission == "USER")
                    {
                        bool isAuthorized = CheckUserPermission(currentUser.Role.Value);

                        if (!isAuthorized)
                        {
                            context.Result = new JsonResult(new
                            {
                                Success = false,
                                Fail = true,
                                Message = ""
                            })
                            { StatusCode = StatusCodes.Status401Unauthorized };
                        }
                    }
                }
                else
                {
                    context.Result = new JsonResult(new
                    {
                        Fail = true,
                        Message = "NOT_PERMISSION_ACCESS_TO_SYSTEM"
                    })
                    { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            else
            {
                context.Result = new JsonResult(new
                {
                    Fail = true,
                    Message = "NOT_PERMISSION_ACCESS_TO_SYSTEM"
                })
                { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
        private bool CheckAdminPermission(int role)
        {
            return (((RoleEnum)role) & RoleEnum.Administrator) == RoleEnum.Administrator;
        }
        private bool CheckUserPermission(int role)
        {
            return (((RoleEnum)role) & RoleEnum.Member) == RoleEnum.Member;
        }
    }
}
