using Core.Workflow.Business.Args;
using IziWork.Common.Args;
using IziWork.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Workflow.Business.Interface
{
    public interface IWorkflowBusiness
    {
        Task<ResultDTO> CreateWorkflowTemplate(WorkflowTemplateArgs args);
        Task<ResultDTO> UpdateWorkflowTemplate(WorkflowTemplateArgs args);
        Task<ResultDTO> GetWorkflowTemplates(QueryArgs args);
        Task<ResultDTO> GetWorkflowTemplateById(Guid id);
        Task<ResultDTO> StartWorkflow(StartWorkflowArgs args);
        Task<ResultDTO> Vote(VoteArgs args);
        Task<ResultDTO> GetPermissionApproveByItemId(Guid ItemId);
        Task<ResultDTO> SubmitDocument(SubmitDocumentArgs args);
        /*Task<ResultDTO> Vote(VoteArgs args);*/
    }
}