using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class WorkflowInstance
{
    public Guid Id { get; set; }

    public string? WorkflowName { get; set; }

    public Guid TemplateId { get; set; }

    public string? WorkflowDataStr { get; set; }

    public Guid ItemId { get; set; }

    public string? ItemReferenceNumber { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsTerminated { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public string? DefaultCompletedStatus { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual WorkflowTemplate Template { get; set; } = null!;

    public virtual ICollection<WorkflowProcessing> WorkflowProcessings { get; set; } = new List<WorkflowProcessing>();
}
