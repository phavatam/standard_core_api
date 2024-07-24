using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class ReferenceDocumentDTO
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Guid ReferenceDocId { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public virtual DocumentDTO Document { get; set; } = null!;
        public virtual DocumentDTO ReferenceDoc { get; set; } = null!;
    }
}
