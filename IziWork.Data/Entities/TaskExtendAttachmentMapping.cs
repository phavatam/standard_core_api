using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class TaskExtendAttachmentMapping
{
    public Guid Id { get; set; }

    public Guid? TaskExtendId { get; set; }

    public Guid? AttachmentFileId { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual AttachmentFile? AttachmentFile { get; set; }

    public virtual TaskExtend? TaskExtend { get; set; }
}
