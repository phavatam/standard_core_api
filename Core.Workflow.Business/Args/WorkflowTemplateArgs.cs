using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Workflow.Business.Args
{
    public class WorkflowTemplateArgs
    {
        public Guid Id { get; set; }
        public string WorkflowName { get; set; }
        public string ItemType { get; set; }
        public bool IsActivated { get; set; }
        public int Order { get; set; }
        public string DefaultCompletedStatus { get; set; }
        public virtual ICollection<WorkflowStepArgs> WorkflowSteps { get; set; } = new List<WorkflowStepArgs>();
    }
}
