using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class TaskExtend
{
    public Guid Id { get; set; }

    public Guid TaskManagementId { get; set; }

    public DateTimeOffset? ToDate { get; set; }

    public string? Note { get; set; }

    public int? NumberOfDaysIncurred { get; set; }

    public Guid? AssignToUserId { get; set; }

    public DateTimeOffset? ApproverExtendToDate { get; set; }

    public string? ApproverNote { get; set; }

    public bool IsApproved { get; set; }

    public bool IsCompleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual ICollection<TaskExtendAttachmentMapping> TaskExtendAttachmentMappings { get; set; } = new List<TaskExtendAttachmentMapping>();

    public virtual TaskManagement TaskManagement { get; set; } = null!;
}
