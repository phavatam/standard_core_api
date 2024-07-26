using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Common.Args;
using IziWork.Common.DTO;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IDocumentBusiness
    {
        Task<ResultDTO> GetCurrentReferenceNumberDocument(string type);
        Task<ResultDTO> CreateDocument(DocumentArgs args);
        Task<ResultDTO> UpdateDocument(DocumentArgs args);
        Task<ResultDTO> DeleteById(Guid Id);
        Task<ResultDTO> GetList(QueryArgs args);
        Task<ResultDTO> GetById(Guid Id);
        Task<ResultDTO> LinkDocuments(LinkDocumentArgs args);
        Task<ResultDTO> GetListReferenceDocumentByDocumentId(Guid DocumentId);
        Task<ResultDTO> DocumentForwarding(DocumentForwardingArgs args);
        Task<ResultDTO> DocumentAssignTask(DocumentAssignTaskArgs args);
        Task<ResultDTO> DocumentDiscussion(DocumentDiscussionArgs args);
    }
}
