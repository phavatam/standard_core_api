using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class UserRoleDepartmentMapping
{
    public Guid Id { get; set; }

    public Guid RoleDepartmentId { get; set; }

    public Guid UserId { get; set; }

    public bool IsHeadCount { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual RoleDepartment RoleDepartment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
