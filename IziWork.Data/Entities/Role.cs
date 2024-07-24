using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Role
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsActivated { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual ICollection<MenuRoleMapping> MenuRoleMappings { get; set; } = new List<MenuRoleMapping>();

    public virtual ICollection<UserDepartmentRoleMapping> UserDepartmentRoleMappings { get; set; } = new List<UserDepartmentRoleMapping>();

    public virtual ICollection<WorkflowRole> WorkflowRoles { get; set; } = new List<WorkflowRole>();
}
