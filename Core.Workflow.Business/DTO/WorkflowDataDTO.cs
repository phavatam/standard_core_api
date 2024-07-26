using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Workflow.Business.DTO
{
    public class WorkflowDataDTO
    {
        public Guid Id { get; set; }
        public string? WorkflowName { get; set; }
        public string? ItemType { get; set; }
        public bool IsActivated { get; set; }
        public int Order { get; set; }
        public string? StartWorkflowButton { get; set; }
        public string? DefaultCompletedStatus { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public virtual IList<WorkflowStepDTO> WorkflowSteps { get; set; } = new List<WorkflowStepDTO>();
    }
}
