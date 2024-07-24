using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class WorkflowRole
{
    public Guid Id { get; set; }

    public Guid WorkflowStepId { get; set; }

    public Guid RoleId { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual WorkflowStep WorkflowStep { get; set; } = null!;
}
