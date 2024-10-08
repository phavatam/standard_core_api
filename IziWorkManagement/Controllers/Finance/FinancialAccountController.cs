﻿using IziWork.Business.Args;
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
    public class FinancialAccountController : ControllerBase
    {
        #region Variables
        private readonly IFinancialAccountBusiness _financialBusiness;
        #endregion
        public FinancialAccountController(
                    IFinancialAccountBusiness financialBusiness)
        {
            _financialBusiness = financialBusiness;
        }
        #region INSERT - UPDATE - DELETE

        [HttpPost("GetTreeList")]
        public async Task<IActionResult> GetTreeList(QueryArgs args)
        {
            var result = await _financialBusiness.GetTreeList(args);
            return Ok(result);
        }

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(QueryArgs args)
        {
            var result = await _financialBusiness.GetList(args);
            return Ok(result);
        }

        [HttpPost("UpSert")]
        public async Task<IActionResult> UpSert(FinancialAccountArgs args)
        {
            var result = await _financialBusiness.UpSert(args);
            return Ok(result);
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById(Guid Id)
        {
            var result = await _financialBusiness.DeleteById(Id);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _financialBusiness.GetById(Id);
            return Ok(result);
        }
        #endregion
    }
}
