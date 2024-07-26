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
        #endregion
        public CompanyController(
                    ICompanyBusiness companyBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _companyBusiness = companyBusiness;
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
    }
}
