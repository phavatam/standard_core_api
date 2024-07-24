using IziWork.Business.DTO;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.ViewModel
{
    public class DepartmentTreeViewModel
    {
        public DepartmentTreeViewModel()
        {
            Items = new HashSet<DepartmentTreeViewModel>();
        }
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int Type { get; set; }

        public Guid JobGradeId { get; set; }

        public Guid? ParentId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Modified { get; set; }

        public Guid? CreatedById { get; set; }

        public Guid ModifiedById { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public string? CreatedByFullName { get; set; }

        public string? ModifiedByFullName { get; set; }

        public string? Note { get; set; }

        public bool? HasTrackingLog { get; set; }
        public int? Grade { get; set; }
        public string? GradeCaption { get; set; }
        public string? GradeTitle { get; set; }
        public string? ParentCode { get; set; }
        public string? ParentName { get; set; }

        public IEnumerable<DepartmentTreeViewModel> Items { get; set; }
        public virtual ICollection<UserDepartmentMappingDTO> UserDepartmentMappings { get; set; } = new List<UserDepartmentMappingDTO>();
    }
}
