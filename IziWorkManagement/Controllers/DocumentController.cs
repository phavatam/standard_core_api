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
    public class DocumentController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IDocumentBusiness _documentBusiness;
        #endregion
        public DocumentController(
                    IDocumentBusiness documentBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _documentBusiness = documentBusiness;
        }

        [HttpGet("GetCurrentReferenceNumberDocument")]
        public async Task<IActionResult> GetCurrentReferenceNumberDocument(string type)
        {
            var result = await _documentBusiness.GetCurrentReferenceNumberDocument(type);
            return Ok(result);
        }

        #region INSERT UPDATE DELETE
        [HttpPost("CreateDocument")]
        public async Task<IActionResult> CreateDocument(DocumentArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.CreateDocument(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPut("UpdateDocument")]
        public async Task<IActionResult> UpdateDocument(DocumentArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.UpdateDocument(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.DeleteById(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion

        #region GET DATA
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.GetList(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.GetById(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion

        #region Feature actions
        [HttpPost("LinkDocuments")]
        public async Task<IActionResult> LinkDocuments(LinkDocumentArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.LinkDocuments(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpGet("GetListReferenceDocumentByDocumentId")]
        public async Task<IActionResult> GetListReferenceDocumentByDocumentId(Guid DocumentId)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.GetListReferenceDocumentByDocumentId(DocumentId);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("DocumentForwarding")]
        public async Task<IActionResult> DocumentForwarding(DocumentForwardingArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.DocumentForwarding(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("DocumentAssignTask")]
        public async Task<IActionResult> DocumentAssignTask(DocumentAssignTaskArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.DocumentAssignTask(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("DocumentDiscussion")]
        public async Task<IActionResult> DocumentDiscussion(DocumentDiscussionArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _documentBusiness.DocumentDiscussion(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        #endregion
    }
}
