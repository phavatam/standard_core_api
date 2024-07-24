using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Nợ xấu
/// </summary>
public partial class BadDebt
{
    public Guid Id { get; set; }

    public Guid ExplanationDetailId { get; set; }

    public Guid BadDebtParentId { get; set; }

    public int Index { get; set; }

    public string IndexName { get; set; } = null!;

    public string? TargetName { get; set; }

    public decimal? OriginalPrice { get; set; }

    public decimal? RecoverableValue { get; set; }

    public string? Debtor { get; set; }

    public Guid? FinancialAccountId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual BadDebt BadDebtParent { get; set; } = null!;

    public virtual ExplanationDetail ExplanationDetail { get; set; } = null!;

    public virtual FinancialAccount? FinancialAccount { get; set; }

    public virtual ICollection<BadDebt> InverseBadDebtParent { get; set; } = new List<BadDebt>();
}
