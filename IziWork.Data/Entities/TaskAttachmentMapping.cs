using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class TaskAttachmentMapping
{
    public Guid Id { get; set; }

    public Guid TaskManagementId { get; set; }

    public Guid AttachmentFileId { get; set; }

    public int? Type { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual AttachmentFile AttachmentFile { get; set; } = null!;

    public virtual TaskManagement TaskManagement { get; set; } = null!;
}
