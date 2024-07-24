using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class MenuRoleMapping
{
    public Guid Id { get; set; }

    public Guid MenuId { get; set; }

    public Guid RoleId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public bool? IsActivated { get; set; }

    public virtual Menu Menu { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
