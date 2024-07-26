using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Workflow.Business.Args
{
    public class StartWorkflowArgs
    {
        public Guid WorkflowTemplateId { get; set; }
        public Guid ItemId { get; set; }
        public string Comment { get; set; } = "";
    }
}
