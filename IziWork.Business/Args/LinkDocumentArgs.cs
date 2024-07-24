using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class LinkDocumentArgs
    {
        public Guid DocumentId { get; set; }
        public List<Guid> ReferenceDocumentIds { get; set; }
    }
}
