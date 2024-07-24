using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Status
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? IsDefault { get; set; }

    public bool? IsWorkflow { get; set; }

    public int Type { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }
}
