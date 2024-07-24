using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class MetadataTypeDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string Code { get; set; }

        public DateTimeOffset? Created { get; set; }

        public Guid? CreatedById { get; set; }

        public Guid? ModifiedById { get; set; }

        public DateTimeOffset? Modified { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public string? CreatedByFullName { get; set; }

        public string? ModifiedByFullName { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual ICollection<MetadataItemDTO> MetadataItems { get; set; } = new List<MetadataItemDTO>();
    }
}
