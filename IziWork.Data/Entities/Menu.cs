using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Menu
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public int? Location { get; set; }

    public Guid? GroupId { get; set; }

    public string? Url { get; set; }

    public string? IconUrl { get; set; }

    public string? VnName { get; set; }

    public bool? IsActivated { get; set; }

    public virtual MetadataItem? Group { get; set; }

    public virtual ICollection<Menu> InverseParent { get; set; } = new List<Menu>();

    public virtual ICollection<MenuDepartmentMapping> MenuDepartmentMappings { get; set; } = new List<MenuDepartmentMapping>();

    public virtual ICollection<MenuRoleMapping> MenuRoleMappings { get; set; } = new List<MenuRoleMapping>();

    public virtual ICollection<MenuUserMapping> MenuUserMappings { get; set; } = new List<MenuUserMapping>();

    public virtual Menu? Parent { get; set; }
}
