using IziWork.Data.Entities;
using System;
using System.Collections.Generic;

namespace Core.Workflow.Business.DTO;

public partial class WorkflowRoleDTO
{
    public Guid Id { get; set; }
    public Guid WorkflowStepId { get; set; }
    public Guid RoleId { get; set; }
}
