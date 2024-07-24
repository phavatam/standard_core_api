using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class ProfileDTO
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? Created { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? ModifiedByFullName { get; set; }
        public virtual ICollection<ProfileAttachmentFileMappingDTO> ProfileAttachmentFileMappings { get; set; } = new List<ProfileAttachmentFileMappingDTO>();
    }
}
