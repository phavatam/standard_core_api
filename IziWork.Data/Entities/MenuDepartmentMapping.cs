using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class MenuDepartmentMapping
{
    public Guid Id { get; set; }

    public Guid MenuId { get; set; }

    public Guid DepartmentId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Menu Menu { get; set; } = null!;
}
