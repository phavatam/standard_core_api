using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class DocumentForwardingArgs
    {
        public Guid DocumentId { get; set; }
        public Guid? ProcessorId { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}
