using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class DocumentDiscussionMappingDTO
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Guid DiscussionId { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public virtual DiscussionDTO Discussion { get; set; } = null!;
    }
}
