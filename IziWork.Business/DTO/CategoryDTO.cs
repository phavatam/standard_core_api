
using IziWork.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class CategoryDetailDTO
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public DefineEnums.CATEGORY_TYPE Type { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? Created { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? ModifiedByFullName { get; set; }
    }
}
