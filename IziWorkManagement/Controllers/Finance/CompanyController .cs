using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWorkManagement.Authorize;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using IziWork.Common.Args;
using IziWork.Common.DTO;

namespace IziWorkManagement.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly ICompanyBusiness _companyBusiness;
        private readonly ILogger<AttachmentFileBusiness> _logger;
        private string _uploadedFilesFolder = null;
        #endregion
        public CompanyController(
                    ICompanyBusiness companyBusiness,
                    IWebHostEnvironment env,
                    IOptionsMonitor<AppSettingModel> optionsMonitor,
                    ILogger<AttachmentFileBusiness> logger)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _companyBusiness = companyBusiness;
            _uploadedFilesFolder = Path.Combine(env.ContentRootPath, "Attachments");
            _logger = logger;
        }
        #region INSERT - UPDATE - DELETE

        [HttpPost("GetListCompany")]
        public async Task<IActionResult> GetListCompany(QueryArgs args)
        {
            var result = await _companyBusiness.GetListCompany(args);
            return Ok(result);
        }

        [HttpPost("UpSertCompany")]
        public async Task<IActionResult> UpSertCompany(CompanyArgs args)
        {
            var result = await _companyBusiness.UpSertCompany(args);
            return Ok(result);
        }

        [HttpDelete("DeleteCompanyById")]
        public async Task<IActionResult> DeleteCompanyById(Guid companyId)
        {
            var result = await _companyBusiness.DeleteCompanyById(companyId);
            return Ok(result);
        }

        [HttpGet("GetCompanyById")]
        public async Task<IActionResult> GetCompanyById(Guid companyId)
        {
            var result = await _companyBusiness.GetCompanyById(companyId);
            return Ok(result);
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Import()
        {
            try
            {
                Directory.CreateDirectory(_uploadedFilesFolder); // Make sure the folder exists
                MemoryStream content = new MemoryStream();
                //var test = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                var test = new StreamContent(Request.Body);
                await test.CopyToAsync(content);
                var files = Request.Form.Files;
                await Request.Body.CopyToAsync(content);
                var result = await _companyBusiness.UploadData(content);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at: Import ", ex.Message);
                return Ok(new ResultDTO { ErrorCodes = { 1001 }, Messages = { "Something went wrong!" } });
            }
        }
    }
}
