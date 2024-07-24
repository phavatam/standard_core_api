using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class SubmitDocumentArgs
    {
        public Guid Id { get; set; }
        public Guid? WorkflowTemplateId { get; set; }
        public Guid? ApproverId { get; set; }
        public string Comment { get; set; }
    }
}
