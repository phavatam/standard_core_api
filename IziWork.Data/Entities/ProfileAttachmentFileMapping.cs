using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class ProfileAttachmentFileMapping
{
    public Guid Id { get; set; }

    public Guid ProfileId { get; set; }

    public Guid AttachmentFileId { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual AttachmentFile AttachmentFile { get; set; } = null!;

    public virtual Profile Profile { get; set; } = null!;
}
