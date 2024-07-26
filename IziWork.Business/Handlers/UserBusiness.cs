using AutoMapper;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;

using IziWork.Data.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using Core.Repositories.Business.IRepositories;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IziWork.Common.DTO;
using IziWork.Common.Args;
using IziWork.Common.Constans;

namespace IziWork.Business.Handlers
{
    public class UserBusiness : IUserBusiness
    {
        private readonly string KEYUSER = "US_JWT_{0}";
        private readonly int ExpireTime = 8;

        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string REGEX_EMAIL = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
        private readonly string REGEX_PHONE = @"^\d{10}$";
        private readonly AutoMapper.IMapper _mapper;
        private readonly IMemoryCache _cache;
        
        public UserBusiness(IUnitOfWork uow, IConfiguration configuration, AutoMapper.IMapper mapper, IMemoryCache cache)
        {
            _uow = uow;
            _configuration = configuration;
            _secretKey = (_configuration != null && _configuration["AppSettings:SecretKey"] != null) ? _configuration["AppSettings:SecretKey"].ToString() : "8qaa,AQ%UrhXY|#PRsb%!4qc8yCbh8n'Bsi{>;I7,%R#EhV@wn%+ni.g#g^h]rF~BQ_>:-F)+dC%!ST6K2";
            _mapper = mapper;
            _cache = cache;
        }
        public async Task<UserDTO> GetUserByLoginName(string loginName)
        {
            var userDTO = new UserDTO();
            var user = await _uow.GetRepository<User>().GetSingleAsync(x => x.LoginName.Equals(loginName));
            if (user != null)
            {
                userDTO = user.Adapt<UserDTO>();
            }
            return userDTO;
        }


        public async Task<ResultDTO> GetListUsers(QueryArgs args)
        {
            var userList = await _uow.GetRepository<User>().FindByAsync<UserDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<User>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = userList,
                    Count = totalList
                }
            };
        }

        public async Task<ResultDTO> InsertUser(UserCRUDRequest args)
        {
            var resultDTO = new ResultDTO();

            if (args is null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            if (string.IsNullOrEmpty(args.LoginName))
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "LOGINNAME_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            var existUser = _uow.GetRepository<User>().GetSingle(x => x.LoginName.Equals(args.LoginName));
            if (existUser != null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "LOGIN_NAME_IS_ALREADY_EXIST" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            if (string.IsNullOrEmpty(args.FullName))
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "FULLNAME_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            if (string.IsNullOrEmpty(args.Password))
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "PASSWORD_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            } else
            {
                var validatePassword = ValidatePassword(args.Password); 
                if (!string.IsNullOrEmpty(validatePassword))
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { validatePassword }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
            }

            if (!string.IsNullOrEmpty(args.Email))
            {
                if (!Regex.IsMatch(args.Email, REGEX_EMAIL))
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { "EMAIL_IS_INVALID" }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
            }

            if (!string.IsNullOrEmpty(args.Phone))
            {
                if (!Regex.IsMatch(args.Phone, REGEX_PHONE))
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { "PHONE_IS_INVALID" }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
            }

            UserDTO currentUser = await this.GetUserByLoginName(args.LoginName);
            #region Verify user 
            if (currentUser?.Id != Guid.Empty)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "ACCOUNT_IS_ALREADY_EXIST" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            #endregion
            if (currentUser?.Id == Guid.Empty)
            {
                // save user
                /*var addUser = new User()
                {
                    Id = Guid.NewGuid(),
                    FullName = args?.Fullname,
                    LoginName = (args is not null ? args.LoginName : ""),
                    Password = BCrypt.Net.BCrypt.HashPassword(args?.Password + _secretKey),
                    Gender = (int) args.Gender,
                    Phone = args.Phone,
                    Email = args.Email,
                    IsActivated = true,
                    Type = (int) args.Type,
                    Role = (int) args.Role,
                    IsBlocked = false
                };*/
                var addUser = _mapper.Map<User>(args);
                addUser.Password = BCrypt.Net.BCrypt.HashPassword(args?.Password + _secretKey);
                /*var addUser = args.Adapt<User>();*/
                var user = _uow.GetRepository<User>().Add(addUser);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "CREATE_USER_IS_SUCCESSFULLY" },
                    Object = _mapper.Map<UserDTO>(user)
                };
            }
            Finish:
            return resultDTO;
        }

        public async Task<ResultDTO> UpdateUser(UserCRUDRequest args)
        {
            var resultDTO = new ResultDTO();

            if (args is null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            if (args.Id == Guid.Empty)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "ID_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            var currentUser = await _uow.GetRepository<User>().GetSingleAsync(x => x.Id.Equals(args.Id));
            if (currentUser == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "ACCOUNT_IS_NOT_EXIST" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            /* if (string.IsNullOrEmpty(args.FullName))
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "FULLNAME_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }*/

            if (!string.IsNullOrEmpty(args.Email))
            {
                if (!Regex.IsMatch(args.Email, REGEX_EMAIL))
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { "EMAIL_IS_INVALID" }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
            }

            if (!string.IsNullOrEmpty(args.Phone))
            {
                if (!Regex.IsMatch(args.Phone, REGEX_PHONE))
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { "PHONE_IS_INVALID" }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
            }

            if (!string.IsNullOrEmpty(args.FullName))
            {
                currentUser.FullName = args.FullName;
            }
            if (!string.IsNullOrEmpty(args.LoginName))
            {
                var existUser = _uow.GetRepository<User>().GetSingle(x => x.LoginName.Equals(args.LoginName) && x.Id != currentUser.Id);
                if (existUser != null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { "LOGIN_NAME_IS_ALREADY_EXIST" }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                currentUser.LoginName = args.LoginName;
            }
            if (!string.IsNullOrEmpty(args.Phone))
            {
                currentUser.Phone = args.Phone;
            }
            if (!string.IsNullOrEmpty(args.Email))
            {
                currentUser.Email = args.Email;
            }
            if (args.Gender != null && args.Gender.HasValue)
            {
                currentUser.Gender = (int) args.Gender;
            }
            if (args.IsActivated != null && args.IsActivated.HasValue)
            {
                currentUser.IsActivated = args.IsActivated.Value;
            }
            if (args.Type != null && args.Type.HasValue)
            {
                currentUser.Type = (int)args.Type;
            }
            if (args.IsBlocked != null && args.IsBlocked.HasValue)
            {
                currentUser.IsBlocked = args.IsBlocked;
            }
            var user = _uow.GetRepository<User>().Update(currentUser);
            await _uow.CommitAsync();
            resultDTO = new ResultDTO() { 
                Messages = new List<string> { "UPDATE_USER_IS_SUCCESSFULLY" }, 
                Object = _mapper.Map<UserDTO>(user)
            };
            Finish:
            return resultDTO;
        }

        public async Task<ResultDTO> DeleteUserById(Guid userId)
        {
            var resultDTO = new ResultDTO();

            var currentUser = await _uow.GetRepository<User>().GetSingleAsync(x => x.Id == (userId));
            #region Verify user 
            if (currentUser == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "ACCOUNT_IS_NOT_EXIST" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            #endregion
            /*currentUser.IsDeleted = true;*/
            _uow.GetRepository<User>().Delete(currentUser);
            await _uow.CommitAsync();
            resultDTO = new ResultDTO() { Messages = new List<string> { "DELETE_USER_IS_SUCCESSFULLY" }};
            Finish:
            return resultDTO;
        }
        public async Task<ResultDTO> GetUserById(Guid userId)
        {
            var resultDTO = new ResultDTO();

            var currentUser = await _uow.GetRepository<User>().GetSingleAsync<UserDTO>(x => x.Id == userId);
            #region Verify user 
            if (currentUser == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "ACCOUNT_IS_NOT_EXIST" }, ErrorCodes = new List<int> { -1 } };
            }
            resultDTO.Object = currentUser;
            #endregion
            return resultDTO;
        }
        private string GetNameMemoryCacheUserId(Guid userId)
        {
            return string.Format(KEYUSER, userId);
        }

        public async Task<ResultDTO> Login(UserLoginRequest args)
        {
            var resultDTO = new ResultDTO();
            var currentUser = await _uow.GetRepository<User>().GetSingleAsync(x => x.LoginName.Equals(args.LoginName));
            if (currentUser == null)
            {
                return new ResultDTO() { Messages = new List<string> { "USERNAME_IS_NOT_EXISTS" }, ErrorCodes = new List<int> { -1 } };
            }

            var inDepartmentHasHeadCount = await _uow.GetRepository<UserDepartmentMapping>().GetSingleAsync(x => x.UserId == currentUser.Id && x.IsHeadCount);
            if (inDepartmentHasHeadCount == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.HAS_NOT_BEEN_ASSIGNED_DEPARTMENT }, ErrorCodes = new List<int> { -1 } };
            }

            #region Verify user 
            if (currentUser.IsBlocked.HasValue && currentUser.IsBlocked.Value)
            {
                return new ResultDTO() { Messages = new List<string> { "ACCOUNT_HAS_BEEN_BLOCKED" }, ErrorCodes = new List<int> { -1 } };
            }

            if (!BCrypt.Net.BCrypt.Verify(args.Password + _secretKey, currentUser?.Password))
            {
                return new ResultDTO() { Messages = new List<string> { "PASSWORD_INCORRECT" }, ErrorCodes = new List<int> { -1 } };
            }
            #endregion
            var token = GenerateToken(currentUser);
            var nameMemoryCache = GetNameMemoryCacheUserId(currentUser.Id);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(ExpireTime), // Thiết lập thời gian hết hạn tuyệt đối là 8 giờ
                SlidingExpiration = TimeSpan.FromHours(ExpireTime) // Thiết lập thời gian hết hạn trượt là 8 giờ
            };

            _cache.Set(nameMemoryCache, token.AccessToken, cacheEntryOptions); // Save token in memory cache
            resultDTO.Object = token.AccessToken;
            return resultDTO;
        }

        public async Task<ResultDTO> ChangePassword(ChangePasswordArgs args)
        {
            var resultDTO = new ResultDTO();
            var currentUser = await _uow.GetRepository<User>().GetSingleAsync(x => x.LoginName.Equals(args.LoginName));
            if (currentUser == null)
            {
                return new ResultDTO() { Messages = new List<string> { "USERNAME_IS_NOT_EXISTS" }, ErrorCodes = new List<int> { -1 } };
            }
            #region Verify user 
            if (currentUser.IsBlocked.HasValue && currentUser.IsBlocked.Value)
            {
                return new ResultDTO() { Messages = new List<string> { "ACCOUNT_HAS_BEEN_BLOCKED" }, ErrorCodes = new List<int> { -1 } };
            }

            if (string.IsNullOrEmpty(args.CurrentPassword))
            {
                return new ResultDTO() { Messages = new List<string> { "CURRENT_PASSWORD_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }

            if (string.IsNullOrEmpty(args.NewPassword))
            {
                return new ResultDTO() { Messages = new List<string> { "NEW_PASSWORD_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            } else
            {
                var validatePassword = ValidatePassword(args.NewPassword);
                if (!string.IsNullOrEmpty(validatePassword))
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { validatePassword }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
            }

            if (string.IsNullOrEmpty(args.ConfirmNewPassword))
            {
                return new ResultDTO() { Messages = new List<string> { "CONFIRM_PASSWORD_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }

            if (!args.NewPassword.Equals(args.ConfirmNewPassword))
            {
                return new ResultDTO() { Messages = new List<string> { "CONFIRM_PASSWORD_IS_NOT_MATCH" }, ErrorCodes = new List<int> { -1 } };
            }

            if (!BCrypt.Net.BCrypt.Verify(args.CurrentPassword + _secretKey, currentUser?.Password))
            {
                return new ResultDTO() { Messages = new List<string> { "PASSWORD_INCORRECT" }, ErrorCodes = new List<int> { -1 } };
            }
            #endregion

            currentUser.Password = BCrypt.Net.BCrypt.HashPassword(args?.NewPassword + _secretKey);
            _uow.GetRepository<User>().Update(currentUser);
            await _uow.CommitAsync();

            // send email to email of user
            resultDTO.Object = _mapper.Map<UserDTO>(currentUser);
            Finish:
            return resultDTO;
        }

        public async Task<ResultDTO> VerifyLogin(string loginName, string jwtToken)
        {
            var resultDTO = new ResultDTO();
            var currentUser = await _uow.GetRepository<User>().GetSingleAsync(x => x.LoginName.Equals(loginName));
            if (currentUser == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "USERNAME_IS_NOT_EXISTS" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            if (currentUser.IsBlocked.HasValue && currentUser.IsBlocked.Value)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "ACCOUNT_HAS_BEEN_BLOCKED" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);

            // Kiểm tra tính hợp lệ của jwtToken
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            SecurityToken validatedToken;
            try
            {
                jwtTokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                // jwtToken đã hết hạn
                resultDTO = new ResultDTO() { Messages = new List<string> { "TOKEN_HAS_EXPIRED" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            catch (SecurityTokenException)
            {
                // jwtToken không hợp lệ
                resultDTO = new ResultDTO() { Messages = new List<string> { "TOKEN_IS_INVALID" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            var nameMemoryCache = GetNameMemoryCacheUserId(currentUser.Id);
            var jwtTokenMemoryCache = _cache.Get<string>(nameMemoryCache);
            if (!string.IsNullOrEmpty(jwtTokenMemoryCache) && !jwtToken.Equals(jwtTokenMemoryCache))
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "TOKEN_IS_INVALID" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            } else
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "TOKEN_VALID" } };
                goto Finish;
            }
            Finish:
            return resultDTO;
        }

        #region GENARATE TOKEN
        private TokenModel GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("FullName", user.FullName ?? ""),
                    new Claim("Role", user.Role.HasValue ? user.Role.Value.ToString() : ""),
                    new Claim("LoginName", user.LoginName),
                    new Claim("Email", user.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(ExpireTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        private static string GenerateRefreshToken()
        {
            var random = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);
            return Convert.ToBase64String(random);
        }
        private string ValidatePassword(string password)
        {
            string returnValue = null;
            if (string.IsNullOrEmpty(password) || password.Length <= 10)
            {
                returnValue = "PASSWORD_MUST_BE_AT_LEAST_10_CHARACTERS";
                goto Finish;
            }

            // Check if the password contains at least one special character
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
            {
                returnValue = "PASSWORD_MUST_CONTAIN_1_SPECIAL_CHARACTER";
                goto Finish;
            }

            // Check if the password contains at least one digit
            if (!Regex.IsMatch(password, @"\d"))
            {
                returnValue = "PASSWORD_MUST_CONTAIN_1_DIGIT";
                goto Finish;
            }
            Finish:
            return returnValue;
        }
        #endregion
    }
}
