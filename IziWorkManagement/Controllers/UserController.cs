using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWorkManagement.Authorize;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public static string _uploadedFilesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Attachments");
        private readonly AppSettingModel _appSettingModel;
        private readonly IUserBusiness _userBusiness;
        private readonly ILogger<UserController> _logger;

        public UserController(
            ILogger<UserController> loggerW,
            IUserBusiness userBusiness,
            IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _logger = loggerW;
            _appSettingModel = optionsMonitor.CurrentValue;
            _userBusiness = userBusiness;
        }

        #region LOGIN/LOGOUT
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest userLoginRequest)
        {
            // get one user by login Name
            //var currentUser = (Guid) Request.HttpContext.Items["UUID"]!;
            //string clientIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _userBusiness.Login(userLoginRequest);
            if (result.IsSuccess)
            {
                Response.Cookies.Append("jwt", result.Object.ToString(), new CookieOptions
                {
                    HttpOnly = true
                });
                return Ok(new ResultDTO
                {
                    Object = result.Object,
                    Messages = new List<string>() { "LOGIN_SUCCESSFULLY" }
                });
            } 
            return Ok(result);
        }
        [HttpGet("VerifyUser")]
        public async Task<IActionResult> VerifyUser()
        {
            ResultDTO resultDTO = new ResultDTO();
            string loginName = string.Empty;
            var jwt = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(jwt))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(jwt);
                    loginName = token.Claims.FirstOrDefault(c => c.Type == "LoginName")?.Value;
                } catch (Exception e)
                { }
            }

            if (string.IsNullOrEmpty(loginName))
            {
                resultDTO = new ResultDTO() { ErrorCodes = new List<int>() { -1 }, Messages = new List<string>() { "TOKEN_IS_INVALID" }    };
            }
            resultDTO = await _userBusiness.VerifyLogin(loginName, jwt);
            return Ok(resultDTO);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordArgs args)
        {
            ResultDTO resultDTO = await _userBusiness.ChangePassword(args);
            return Ok(resultDTO);
        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new
            {
                message = "SIGN_OUT_SUCCESSFULLY",
                Success = false,
                Fail = true
            });
        }

        #endregion

        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            ResultDTO resultDTO = new ResultDTO();
            var currentUserId = (HttpContext.Items["UUID"] as Guid?) ?? Guid.Empty;
            if (currentUserId == Guid.Empty)
            {
                return BadRequest("Invalid User");
            }
            var result = await _userBusiness.GetUserById(currentUserId);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(resultDTO);
        }

        [HttpPost("GetListUser")]
        //[Authorize("ADMIN")]
        public async Task<IActionResult> GetListUsers(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _userBusiness.GetListUsers(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("CreateUser")]
        /*[Authorize("ADMIN")]*/
        public async Task<IActionResult> CreateUser(UserCRUDRequest userRegistRequest)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _userBusiness.InsertUser(userRegistRequest);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPut("UpdateUser")]
        //[Authorize("ADMIN")]
        public async Task<IActionResult> UpdateUser(UserCRUDRequest userRegistRequest)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _userBusiness.UpdateUser(userRegistRequest);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        [HttpDelete("DeleteUserById")]
        //[Authorize("ADMIN")]
        public async Task<IActionResult> DeleteUserById(Guid userId)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _userBusiness.DeleteUserById(userId);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("Import")]
        public async Task<IActionResult> Import(Guid deptId, Guid periodId, string sapCodes, bool visibleSubmit, Guid? divisionId = null)
        {
            try
            {
                Directory.CreateDirectory(_uploadedFilesFolder); // Make sure the folder exists
                MemoryStream content = new MemoryStream();
                /*var test = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                await test.CopyToAsync(content);*/
                await Request.Body.CopyToAsync(content);
                //var result = await _target.UploadData(arg, content);
                return Ok(new ResultDTO { ErrorCodes = { 1001 }, Messages = { "Something went wrong!" } });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at: Import ", ex.Message);
                return Ok(new ResultDTO { ErrorCodes = { 1001 }, Messages = { "Something went wrong!" } });
            }
        }
        [HttpPost("PostFileWithData")]
        public async Task<IActionResult> PostFileWithData()
        {
            //var root = HttpContext.Current.Server.MapPath("~/App_Data/Uploadfiles");
            if (!Request.HasFormContentType)
            {
                return BadRequest();
            }

            Directory.CreateDirectory(_uploadedFilesFolder);
            var formCollection = await Request.ReadFormAsync();
            var model = formCollection["argData"];
            if (model.Count == 0)
            {
                return BadRequest();
            }
            else
            {

            }
            //TODO: Do something with the JSON data.  
            var file = formCollection.Files[0];
            var byteArray = new byte[file.Length];
            using (var stream = new MemoryStream(byteArray))
            {
                await file.CopyToAsync(stream);
            }
            MemoryStream content = new MemoryStream(byteArray);
            //var res = await _target.UploadData(arg, content);
            //var res = await _target.UploadData(arg, content);
            return Ok(new ResultDTO { ErrorCodes = { 1001 }, Messages = { "Something went wrong!" } });
        }
    }
}