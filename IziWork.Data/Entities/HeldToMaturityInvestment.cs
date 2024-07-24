using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Đầu tư nắm giữ đến ngày đáo hạn
/// </summary>
public partial class HeldToMaturityInvestment
{
    public Guid Id { get; set; }

    public Guid ExplanationDetailId { get; set; }

    public Guid? HeldToMaturityInvestmentParentId { get; set; }

    public string TargetName { get; set; } = null!;

    public int Index { get; set; }

    public string IndexName { get; set; } = null!;

    /// <summary>
    /// Giá gốc
    /// </summary>
    public decimal? StartOfYearOriginalPrice { get; set; }

    /// <summary>
    /// Giá gốc
    /// </summary>
    public decimal? StartOfYearValue { get; set; }

    /// <summary>
    /// Giá gốc
    /// </summary>
    public decimal? EndOfYearOriginalPrice { get; set; }

    /// <summary>
    /// Giá gốc
    /// </summary>
    public decimal? EndOfYearValue { get; set; }

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

    public virtual ExplanationDetail ExplanationDetail { get; set; } = null!;

    public virtual FinancialAccount? FinancialAccount { get; set; }

    public virtual HeldToMaturityInvestment? HeldToMaturityInvestmentParent { get; set; }

    public virtual ICollection<HeldToMaturityInvestment> InverseHeldToMaturityInvestmentParent { get; set; } = new List<HeldToMaturityInvestment>();
}
