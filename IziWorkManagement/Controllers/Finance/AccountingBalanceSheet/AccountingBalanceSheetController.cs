using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWorkManagement.Authorize;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IziWorkManagement.Controllers.Finance.AccountingBalanceSheet
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingBalanceSheetController : ControllerBase
    {
        #region Variables
        private readonly IAccountingBalanceSheetBusiness _accountingBalanceSheetBusiness;
        #endregion
        public AccountingBalanceSheetController(IAccountingBalanceSheetBusiness accountingBalanceSheetBusiness)
        {
            _accountingBalanceSheetBusiness = accountingBalanceSheetBusiness;
        }
        #region INSERT - UPDATE - DELETE

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(QueryArgs args)
        {
            var result = await _accountingBalanceSheetBusiness.GetList(args);
            return Ok(result);
        }

        [HttpPost("UpSert")]
        public async Task<IActionResult> UpSert(FinancialAccountArgs args)
        {
            var result = await _accountingBalanceSheetBusiness.UpSert(args);
            return Ok(result);
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById(Guid Id)
        {
            var result = await _accountingBalanceSheetBusiness.DeleteById(Id);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _accountingBalanceSheetBusiness.GetById(Id);
            return Ok(result);
        }
        #endregion
    }
}
