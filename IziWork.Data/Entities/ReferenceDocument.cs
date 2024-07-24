using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class ReferenceDocument
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public Guid ReferenceDocId { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual Document Document { get; set; } = null!;
}
