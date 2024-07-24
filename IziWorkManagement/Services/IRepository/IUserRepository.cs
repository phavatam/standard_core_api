using IziWorkManagement.Controllers.Response;

namespace IziWorkManagement.Services.IRepository
{
    public interface IUserRepository
    {
        #region CRUD TABLE USER
        Task<ResultDTO> InsertUser(UserDto newUser);
        #endregion
        #region Login And Regist Account
        UserDto GetUserByLoginName(string loginName);
        UserDto GetUserByEmail(string email);
        #endregion
    }
}
