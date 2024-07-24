using IziWork.Business.DTO;
using IziWork.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class WorkflowStepArgs
    {
        public string? StepName { get; set; }
        public int StepNumber { get; set; }
        public string? SuccessVote { get; set; }
        public string? FailureVote { get; set; }
        public int DueDateNumber { get; set; }
        public Guid? AssignToUserId { get; set; }
        public Guid? AssignToDepartmentId { get; set; }
        public Guid StatusId { get; set; }
        public bool IsSign { get; set; }
        public virtual ICollection<WorkflowRoleArgs> WorkflowRoles { get; set; } = new List<WorkflowRoleArgs>();
    }
}
