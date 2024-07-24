using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class DiscussionDTO
    {
        public Guid Id { get; set; }
        public Guid? ParentDiscussionId { get; set; }
        public string? Message { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? ModifiedByFullName { get; set; }
        //public virtual ICollection<DocumentDiscussionMappingDTO> DocumentDiscussionMappings { get; set; } = new List<DocumentDiscussionMappingDTO>();
        public virtual DiscussionDTO? ParentDiscussion { get; set; }
    }
}
