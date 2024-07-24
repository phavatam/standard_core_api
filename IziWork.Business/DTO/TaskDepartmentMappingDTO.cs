using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class TaskDepartmentMappingDTO
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Guid TaskManagementId { get; set; }
        public bool? IsProcessed { get; set; }
        public bool? IsCoordinated { get; set; }
        public bool? IsViewed { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? ModifiedByFullName { get; set; }
    }
}
