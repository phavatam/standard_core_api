using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class RoleDepartment
{
    public Guid Id { get; set; }

    public Guid DepartmentId { get; set; }

    public Guid RoleId { get; set; }

    public DateTimeOffset? Created { get; set; }

    public Guid? CreatedById { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? ModifiedId { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<UserRoleDepartmentMapping> UserRoleDepartmentMappings { get; set; } = new List<UserRoleDepartmentMapping>();
}
