using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class WorkflowProcessing
{
    public Guid Id { get; set; }

    public Guid InstanceId { get; set; }

    public Guid ItemId { get; set; }

    public string? RequestedUserName { get; set; }

    public Guid? RequestedUserId { get; set; }

    public Guid? RequestedDepartmentId { get; set; }

    public string? RequestedDepartmentName { get; set; }

    public string? ReferenceNumber { get; set; }

    public Guid? ApproverId { get; set; }

    public Guid? AssignedToUserId { get; set; }

    public Guid? AssignedToDepartmentId { get; set; }

    public Guid? AssignedToRoleId { get; set; }

    public string? Approver { get; set; }

    public string? ApproverFullName { get; set; }

    public string? Outcome { get; set; }

    public string? Comment { get; set; }

    public int VoteType { get; set; }

    public int StepNumber { get; set; }

    public bool IsStepCompleted { get; set; }

    public DateTimeOffset DueDate { get; set; }

    public string? ItemType { get; set; }

    public string? Status { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual Department? AssignedToDepartment { get; set; }

    public virtual User? AssignedToUser { get; set; }

    public virtual WorkflowInstance Instance { get; set; } = null!;
}
