using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class DocumentDiscussionMapping
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public Guid DiscussionId { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual Discussion Discussion { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;
}
