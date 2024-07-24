using IziWork.Business.Args;
using IziWork.Business.Enums;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;

namespace IziWork.Business.DTO;

public partial class WorkflowStepDTO
{
    public Guid Id { get; set; }
    public string? StepName { get; set; }
    public int StepNumber { get; set; }
    public string? SuccessVote { get; set; }
    public int DueDateNumber { get; set; }
    public Guid StatusId { get; set; }
    public bool IsSign { get; set; }
    public Guid? AssignToDepartmentId { get; set; }
    public Guid? AssignToUserId { get; set; }
    public virtual List<WorkflowRoleDTO> WorkflowRoles { get; set; } = new List<WorkflowRoleDTO>();
}
