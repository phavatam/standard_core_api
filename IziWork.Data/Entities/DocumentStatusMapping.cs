using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class DocumentStatusMapping
{
    public Guid Id { get; set; }

    public Guid? DocumentId { get; set; }

    public int? StatusId { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual Status? Status { get; set; }
}
