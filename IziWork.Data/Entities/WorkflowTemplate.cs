﻿using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class WorkflowTemplate
{
    public Guid Id { get; set; }

    public string? WorkflowName { get; set; }

    public string? ItemType { get; set; }

    public bool IsActivated { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public int Order { get; set; }

    public string? StartWorkflowButton { get; set; }

    public string? DefaultCompletedStatus { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();

    public virtual ICollection<WorkflowStep> WorkflowSteps { get; set; } = new List<WorkflowStep>();
}
