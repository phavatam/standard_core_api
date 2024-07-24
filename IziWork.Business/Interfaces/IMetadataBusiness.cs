using IziWork.Business.Args;
using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IMetadataBusiness
    {
        Task<ResultDTO> CreateOrUpdateType(MetadataTypeArgs args);
        Task<ResultDTO> DeleteMetadataType(Guid Id);
        Task<ResultDTO> CreateOrUpdateItem(MetadataArgs args);
        Task<ResultDTO> DeleteMetadataItem(Guid Id);
        Task<ResultDTO> GetListMetadataType(QueryArgs args);
        Task<ResultDTO> GetMetadataItemByType(Guid TypeId);
    }
}
