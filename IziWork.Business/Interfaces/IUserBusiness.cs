using IziWork.Business.Args;
using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IUserBusiness
    {
        Task<ResultDTO> Login(UserLoginRequest args);
        Task<ResultDTO> ChangePassword(ChangePasswordArgs args);
        Task<UserDTO> GetUserByLoginName(string loginName);
        Task<ResultDTO> VerifyLogin(string userId, string JWTToken);
        Task<ResultDTO> GetListUsers(QueryArgs args);
        Task<ResultDTO> InsertUser(UserCRUDRequest args);
        Task<ResultDTO> UpdateUser(UserCRUDRequest args);
        Task<ResultDTO> DeleteUserById(Guid userId);
        Task<ResultDTO> GetUserById(Guid userId);
    }
}
