using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class TaskManagementHistoryDTO
    {
        public Guid Id { get; set; }
        public Guid? TaskManagementId { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public DateTimeOffset? ExtendToDate { get; set; }
        public int? PercentCompleted { get; set; }
        public string? Note { get; set; }
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
