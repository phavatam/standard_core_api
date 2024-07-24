using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class WorkflowStepCondition
{
    public Guid Id { get; set; }

    public Guid? WorkflowStepId { get; set; }

    public string? FieldName { get; set; }

    public string? FieldValue { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual WorkflowStep? WorkflowStep { get; set; }
}
