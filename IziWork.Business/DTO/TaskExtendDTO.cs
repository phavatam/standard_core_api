using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class TaskExtendDTO
    {
        public Guid Id { get; set; }

        public Guid TaskManagementId { get; set; }

        public DateTimeOffset? ToDate { get; set; }

        public string? Note { get; set; }

        public int? NumberOfDaysIncurred { get; set; }

        public Guid? AssignToUserId { get; set; }

        public bool? IsApproved { get; set; }
        public bool IsCompleted { get; set; }

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
