using Core.Repositories.Business.IRepositories;

using IziWork.Business.Interfaces;
using IziWork.Common.Constans;
using IziWork.Data.Entities;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using IziWork.Common.Args;
using IziWork.Common.DTO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace IziWorkManagement.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly AppSettingModel _appSettingModel;
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthenticationAPIMiddlewareOptions _options;
        private readonly List<string> IGNORE_AUTHEN = new List<string>() { "/api/User/Login", "/api/User/Logout", "/api/User/CreateUser" };

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger,
        IOptionsMonitor<AppSettingModel> optionsMonitor, IServiceProvider serviceProvider, IOptionsMonitor<AuthenticationAPIMiddlewareOptions> options)
        {
            _next = next;
            //_logger = logger;
            _serviceProvider = serviceProvider;
            _appSettingModel = optionsMonitor.CurrentValue;
            _options = options.CurrentValue;
            //_uow = uow;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {

                /*var endpoint = httpContext.GetEndpoint();
                var routePattern = (endpoint as RouteEndpoint).RoutePattern.RawText;
                if ((_options != null && _options.ExcludedPaths != null && _options.ExcludedPaths.Any() ? _options.ExcludedPaths : IGNORE_AUTHEN).Contains(routePattern))
                {
                    // Bỏ qua middleware và tiếp tục xử lý
                    await _next(httpContext);
                }
                else
                {
                    var jwt = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    var userLoginName = ValidateToken(jwt ?? "");
                    var _uow = httpContext.RequestServices.GetRequiredService<IUnitOfWork>();
                    var user = await _uow.GetRepository<User>().GetSingleAsync(x => x.LoginName.Equals(userLoginName ?? ""));
                    if (user != null && user.Id != Guid.Empty)
                    {
                        httpContext.Items["UUID"] = user.Id;
                    }
                    else
                    {
                        HandleAuthorExceptionAsync(httpContext);
                    }
                    await _next(httpContext);
                }*/
                string errorMessage = "";
                var jwt = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var userLoginName = ValidateToken(jwt ?? "", out errorMessage);
                if (!string.IsNullOrEmpty(jwt) && !string.IsNullOrEmpty(errorMessage))
                {
                    await this.ThrowErrorMessages(httpContext, errorMessage);
                } else
                {
                    var _uow = httpContext.RequestServices.GetRequiredService<IUnitOfWork>();
                    var user = await _uow.GetRepository<User>().GetSingleAsync(x => x.LoginName.Equals(userLoginName ?? ""));
                    if (user != null && user.Id != Guid.Empty)
                    {
                        httpContext.Items["UUID"] = user.Id;
                    }
                    else
                    {
                        //HandleAuthorExceptionAsync(httpContext);
                    }
                    await _next(httpContext);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private string? ValidateToken(string token, out string errorMessages)
        {
            string userLoginName = "";
            errorMessages = "";
            if (token == "")
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettingModel.SecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    //ValidateLifetime = true // Add this line to validate token expiration
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                userLoginName = jwtToken.Claims.First(x => x.Type == "LoginName").Value;
            }
            catch (SecurityTokenExpiredException)
            {
                errorMessages = MessageConst.TOKEN_EXPIRED;
            }
            catch (Exception e)
            {
                errorMessages = e.Message;
            }
            return userLoginName;
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errorResponse = new ResultDTO
            {
                ErrorCodes = new List<int> { 505 },
                Messages = new List<string> { "Đã xảy ra lỗi khi xác thực: "  + exception.Message}
            };

            //_logger.LogError("Đã xảy ra lỗi - {exception}", exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }

        private async Task HandleAuthorExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            response.StatusCode = (int) HttpStatusCode.Unauthorized;

            var errorResponse = new ResultDTO
            {
                ErrorCodes = new List<int> { 401 },
                Messages = new List<string> { "NOT_PERMISSION_ACCESS_TO_SYSTEM" }
            };

            //_logger.LogError("Đã xảy ra lỗi - {exception}", exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }

        private async Task ThrowErrorMessages(HttpContext context, string errorMessages)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            response.StatusCode = (int) HttpStatusCode.Unauthorized;

            var errorResponse = new ResultDTO
            {
                ErrorCodes = new List<int> { -1 },
                Messages = new List<string> { errorMessages }
            };

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
