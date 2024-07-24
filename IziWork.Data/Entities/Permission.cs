using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Permission
{
    public Guid Id { get; set; }

    public Guid ItemId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid? RoleId { get; set; }

    public int? Perm { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual Department? Department { get; set; }
}
