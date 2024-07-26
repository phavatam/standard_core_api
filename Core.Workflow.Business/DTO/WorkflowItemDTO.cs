using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Workflow.Business.DTO
{
    public class WorkflowItemDTO
    {
        public Guid Id { get; set; }
        public Guid ItemId
        {
            get
            {
                return Id;
            }
        }
        public string Type { get; set; }
        public string Status { get; set; }
        public string ReferenceNumber { get; set; }
        public Guid CreatedById { get; set; }
    }
}
