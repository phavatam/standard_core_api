using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class WorkflowStep
{
    public Guid Id { get; set; }

    public Guid WorkflowTemplateId { get; set; }

    public string? StepName { get; set; }

    public int StepNumber { get; set; }

    public string? SuccessVote { get; set; }

    public string? FailureVote { get; set; }

    public int DueDateNumber { get; set; }

    public Guid? AssignToDepartmentId { get; set; }

    public Guid? AssignToUserId { get; set; }

    public Guid? StatusId { get; set; }

    public bool IsSign { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual Department? AssignToDepartment { get; set; }

    public virtual User? AssignToUser { get; set; }

    public virtual ICollection<WorkflowRole> WorkflowRoles { get; set; } = new List<WorkflowRole>();

    public virtual WorkflowTemplate WorkflowTemplate { get; set; } = null!;
}
