using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IziWorkManagement.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralJournalController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IGeneralJournalBusiness _generalJournalBusiness;
        #endregion
        public GeneralJournalController(
                    IGeneralJournalBusiness generalJournalBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _generalJournalBusiness = generalJournalBusiness;
        }

        [HttpPost("GetListGeneralJournal")]
        public async Task<IActionResult> GetListGeneralJournal(QueryArgs args)
        {
            var result = await _generalJournalBusiness.GetListGeneralJournal(args);
            return Ok(result);
        }

        [HttpPost("CreateOrUpdateGeneralJournal")]
        public async Task<IActionResult> CreateOrUpdateGeneralJournal(GeneralJournalArgs args)
        {
            var result = await _generalJournalBusiness.CreateOrUpdateGeneralJournal(args);
            return Ok(result);
        }

        [HttpPost("CreateOrUpdateOneRowGeneralJournalDetail")]
        public async Task<IActionResult> CreateOrUpdateOneRowGeneralJournalDetail(GeneralJournalDetailArgs args)
        {
            var result = await _generalJournalBusiness.CreateOrUpdateOneRowGeneralJournalDetail(args);
            return Ok(result);
        }

        [HttpDelete("DeleteGeneralJournal")]
        public async Task<IActionResult> DeleteGeneralJournal(Guid Id)
        {
            var result = await _generalJournalBusiness.DeleteGeneralJournal(Id);
            return Ok(result);
        }

        [HttpDelete("DeleteGeneralJournalDetail")]
        public async Task<IActionResult> DeleteGeneralJournalDetail(Guid Id)
        {
            var result = await _generalJournalBusiness.DeleteGeneralJournalDetail(Id);
            return Ok(result);
        }
    }
}
