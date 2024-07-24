using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IWorkflowBusiness _workflowBusiness;
        #endregion
        public WorkflowController(
                    IWorkflowBusiness workflowBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _workflowBusiness = workflowBusiness;
        }
        [HttpPost("GetWorkflowTemplates")]
        public async Task<IActionResult> GetWorkflowTemplates(QueryArgs args)
        {
            return Ok(await _workflowBusiness.GetWorkflowTemplates(args));
        }
        [HttpPost("CreateWorkflowTemplate")]
        public async Task<IActionResult> CreateWorkflowTemplate(WorkflowTemplateArgs args)
        {
            ResultDTO result = await _workflowBusiness.CreateWorkflowTemplate(args);
            return Ok(result);
        }
        [HttpPut("UpdateWorkflowTemplate")]
        public async Task<IActionResult> UpdateWorkflowTemplate(WorkflowTemplateArgs args)
        {
            return Ok(await _workflowBusiness.UpdateWorkflowTemplate(args));
        }
        [HttpGet("GetWorkflowTemplateById")]
        public async Task<IActionResult> GetWorkflowTemplateById(Guid Id)
        {
            return Ok(await _workflowBusiness.GetWorkflowTemplateById(Id));
        }

        [HttpPost("StartWorkflow")]
        public async Task<IActionResult> StartWorkflow(StartWorkflowArgs args)
        {
            ResultDTO result = await _workflowBusiness.StartWorkflow(args);
            return Ok(result);
        }
        [HttpPost("Vote")]
        public async Task<ResultDTO> Vote(VoteArgs args)
        {
            return await _workflowBusiness.Vote(args);
        }

        [HttpGet("GetPermissionApproveByItemId")]
        public async Task<ResultDTO> GetPermissionApproveByItemId(Guid itemId)
        {
            return await _workflowBusiness.GetPermissionApproveByItemId(itemId);
        }

        #region Document
        [HttpPost("SubmitDocument")]
        public async Task<ResultDTO> SubmitDocument(SubmitDocumentArgs args)
        {
            return await _workflowBusiness.SubmitDocument(args);
        }
        #endregion
    }
}