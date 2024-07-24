using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class MetadataItemDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public Guid? TypeId { get; set; }
        public string? TypeCode { get; set; }
        public string? TypeName { get; set; }

        public DateTimeOffset? Created { get; set; }

        public DateTimeOffset? Modified { get; set; }

        public Guid? CreatedById { get; set; }

        public Guid? ModifiedById { get; set; }

        public bool IsDisabled { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public string? CreatedByFullName { get; set; }

        public string? ModifiedByFullName { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
