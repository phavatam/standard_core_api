using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Category
{
    public Guid Id { get; set; }

    public string? Module { get; set; }

    public int Type { get; set; }

    public string Name { get; set; } = null!;

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual ICollection<CategoryDetail> CategoryDetails { get; set; } = new List<CategoryDetail>();
}
