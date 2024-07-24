using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagementController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly ITaskManagementBusiness _taskManagementBusiness;
        #endregion
        public TaskManagementController(
                    ITaskManagementBusiness taskManagementBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _taskManagementBusiness = taskManagementBusiness;
        }
        [HttpPost("GetListTasks")]
        public async Task<IActionResult> GetListTasks(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _taskManagementBusiness.GetListTasks(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("AssignTask")]
        public async Task<IActionResult> AssignTask(AssignTaskArgs args)
        {
            var result = await _taskManagementBusiness.AssignTask(args);
            return Ok(result);
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(AssignTaskArgs args)
        {
            var result = await _taskManagementBusiness.CreateTask(args);
            return Ok(result);
        }

        [HttpPost("UpdateTask")]
        public async Task<IActionResult> UpdateTask(AssignTaskArgs args)
        {
            var result = await _taskManagementBusiness.UpdateTask(args);
            return Ok(result);
        }

        [HttpGet("GetTaskByParentTaskId")]
        public async Task<IActionResult> GetTaskByParentTaskId(Guid parentTaskId)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _taskManagementBusiness.GetTaskByParentTaskId(parentTaskId);
            if (resultDTO != null) resultDTO.Object = result;
            return Ok(resultDTO);
        }

        [HttpPost("ExtendTasks")]
        public async Task<IActionResult> ExtendTasks(ExtendTaskArgs args)
        {
            var result = await _taskManagementBusiness.ExtendTasks(args);
            return Ok(result);
        }

        [HttpPost("ApproveExtendTask")]
        public async Task<IActionResult> ApproveExtendTask(ApproveExtendTaskArgs args)
        {
            var result = await _taskManagementBusiness.ApproveExtendTask(args);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var taskDTO = await _taskManagementBusiness.GetById(Id);
            resultDTO.Object = taskDTO;
            return Ok(resultDTO);
        }

        [HttpPost("UpdateStatusTask")]
        public async Task<IActionResult> UpdateStatusTask(UpdateStatusTaskArgs args)
        {
            var taskDTO = await _taskManagementBusiness.UpdateStatusTask(args);
            return Ok(taskDTO);
        }

        [HttpPost("GetTaskManagementHistories")]
        public async Task<IActionResult> GetTaskManagementHistories(QueryArgs args)
        {
            var taskDTO = await _taskManagementBusiness.GetTaskManagementHistories(args);
            return Ok(taskDTO);
        }
    }
}