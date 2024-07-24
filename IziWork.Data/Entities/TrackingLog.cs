using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class TrackingLog
{
    public Guid Id { get; set; }

    public int Action { get; set; }

    public Guid? ItemId { get; set; }

    public string? ItemType { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }
}
