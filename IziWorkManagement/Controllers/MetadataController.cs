using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using IziWork.Common.Args;
using IziWork.Common.DTO;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly IMetadataBusiness _metadataBusiness;
        #endregion
        public MetadataController(
                    IMetadataBusiness metadataBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor)
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _metadataBusiness = metadataBusiness;
        }
        #region INSERT - UPDATE - DELETE
        [HttpPost("CreateOrUpdateType")]
        public async Task<IActionResult> CreateOrUpdateType(MetadataTypeArgs model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _metadataBusiness.CreateOrUpdateType(model);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        [HttpDelete("DeleteMetadataType")]
        public async Task<IActionResult> DeleteMetadataType(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _metadataBusiness.DeleteMetadataType(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }

        [HttpPost("CreateOrUpdateItem")]
        public async Task<IActionResult> CreateOrUpdateItem(MetadataArgs model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _metadataBusiness.CreateOrUpdateItem(model);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        [HttpDelete("DeleteMetadataItem")]
        public async Task<IActionResult> DeleteMetadataItem(Guid Id)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _metadataBusiness.DeleteMetadataItem(Id);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
        #region GET DATA
        [HttpPost("GetListMetadataType")]
        public async Task<IActionResult> GetListMetadataType(QueryArgs args)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _metadataBusiness.GetListMetadataType(args);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        [HttpGet("GetMetadataItemByType")]
        public async Task<IActionResult> GetMetadataItemByType(Guid TypeId)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _metadataBusiness.GetMetadataItemByType(TypeId);
            if (resultDTO != null)
            {
                resultDTO.Object = result;
            }
            return Ok(result);
        }
        #endregion
    }
}
