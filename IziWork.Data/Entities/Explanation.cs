using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Thuyết minh
/// </summary>
public partial class Explanation
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public int ExplanationType { get; set; }

    public string TemplateNo { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Year { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual CompanyInfo Company { get; set; } = null!;

    public virtual ICollection<ExplanationDetail> ExplanationDetails { get; set; } = new List<ExplanationDetail>();
}
