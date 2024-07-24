using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class AttachmentFile
{
    public Guid Id { get; set; }

    public string? FileName { get; set; }

    public string? FileDisplayName { get; set; }

    public string? FileUniqueName { get; set; }

    public int? Size { get; set; }

    public string? Type { get; set; }

    public string? Extension { get; set; }

    public string? Description { get; set; }

    public byte[]? FileAttach { get; set; }

    public string? LinkDownload { get; set; }

    public string? NameEng { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual ICollection<DocumentAttachmentMapping> DocumentAttachmentMappings { get; set; } = new List<DocumentAttachmentMapping>();

    public virtual ICollection<ProfileAttachmentFileMapping> ProfileAttachmentFileMappings { get; set; } = new List<ProfileAttachmentFileMapping>();

    public virtual ICollection<TaskAttachmentMapping> TaskAttachmentMappings { get; set; } = new List<TaskAttachmentMapping>();

    public virtual ICollection<TaskExtendAttachmentMapping> TaskExtendAttachmentMappings { get; set; } = new List<TaskExtendAttachmentMapping>();
}
