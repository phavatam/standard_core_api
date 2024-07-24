using Dapper;
using IziWorkManagement.Controllers.Response;
using IziWorkManagement.Data;
using IziWorkManagement.Data.DTOs;
using IziWorkManagement.Data.Entity;
using IziWorkManagement.Services.IRepository;
using Mapster;
using System.Data;

namespace IziWorkManagement.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        #region Variables
        public DataContext _DbContext;
        #endregion

        #region Constructors
        public UserRepository(DataContext DbContext)
        {
            _DbContext = DbContext;
        }
        #endregion

        #region CRUD TABLE USER
        public async Task<ResultDTO> InsertUser(UserDto newUser)
        {
            var query = @"INSERT INTO [dbo].[Users]
                           ([Id]
                           ,[Fullname]
                           ,[LoginName]
                           ,[Password])
                     VALUES
                           (@Id
                           ,@Fullname
                           ,@LoginName
                           ,@Password)";
            using var connection = _DbContext.CreateConnection();
            connection.Open();
            using var tran = connection.BeginTransaction();
            var response = new ResultDTO();
            try
            {
                var user = new User();
                user = newUser.Adapt<User>();

                await connection.ExecuteAsync(query, user, tran);
                tran.Commit();

                response = new ResultDTO()
                {
                    Messages = new List<string> { "Thêm mới thành công !" }
                };
                return response;

            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
        }
        #endregion

        #region Login And Regist Account
        public UserDto GetUserByLoginName(string loginName)
        {
            var query = "select * from [dbo].[Users] where LoginName = @LoginName";
            using var con = _DbContext.CreateConnection();
            User user = con.QueryFirstOrDefault<User>(query, new { loginName });
            var userDto = new UserDto();
            if (user != null)
            {
                userDto = user.Adapt<UserDto>();
                return userDto;
            }
            return userDto;
        }
        public UserDto GetUserByEmail(string email)
        {
            var query = "select * from [dbo].[Users] where Email = @Email";
            using var con = _DbContext.CreateConnection();
            User user = con.QueryFirstOrDefault<User>(query, new { email });
            var userDto = new UserDto();
            if (user != null)
            {
                userDto = user.Adapt<UserDto>();
                return userDto;
            }
            return userDto;
        }
        #endregion
    }
}
