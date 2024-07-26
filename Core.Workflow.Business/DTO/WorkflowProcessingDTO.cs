using IziWork.Common.Enums;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Workflow.Business.DTO
{
    public class WorkflowProcessingDTO
    {
        public Guid Id { get; set; }
        public Guid InstanceId { get; set; }
        public Guid? ApproverId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public Guid? AssignedToDepartmentId { get; set; }
        public int AssignedToDepartmentType { get; set; }
        public string? Approver { get; set; }
        public string? ApproverFullName { get; set; }
        public string? Outcome { get; set; }
        public string? Comment { get; set; }
        public VoteType VoteType { get; set; }
        public int StepNumber { get; set; }
        public bool IsStepCompleted { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        /*public virtual DepartmentDTO? AssignedToDepartment { get; set; }
        public virtual UserDTO? AssignedToUser { get; set; }*/
        //public virtual WorkflowInstance Instance { get; set; } = null!;
    }
}
