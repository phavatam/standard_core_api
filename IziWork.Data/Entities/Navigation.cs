using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Navigation
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public Guid? NavigationParentId { get; set; }

    public int? Type { get; set; }

    public bool? IsBlank { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsActived { get; set; }

    public DateTimeOffset? Created { get; set; }

    public Guid? CreatedById { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? ModifiedId { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Navigation> InverseNavigationParent { get; set; } = new List<Navigation>();

    public virtual Navigation? NavigationParent { get; set; }
}
