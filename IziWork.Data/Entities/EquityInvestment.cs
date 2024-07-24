using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Đầu tư góp vốn vào đơn vị khác (chi tiết theo từng khoản đầu tư theo tỷ lệ vốn nắm giữ và tỷ lệ quyền biểu quyết)
/// </summary>
public partial class EquityInvestment
{
    public Guid Id { get; set; }

    public Guid ExplanationDetailId { get; set; }

    public Guid? EquityInvestmentParentId { get; set; }

    public int Index { get; set; }

    public string IndexName { get; set; } = null!;

    /// <summary>
    /// Giá gốc
    /// </summary>
    public decimal? StartOfYearOriginalPrice { get; set; }

    /// <summary>
    /// Giá trị hợp lý
    /// </summary>
    public decimal? StartOfYearFairValue { get; set; }

    /// <summary>
    /// Dự phòng
    /// </summary>
    public decimal? StartOfYearProvision { get; set; }

    /// <summary>
    /// Giá gốc
    /// </summary>
    public decimal? EndOfYearOriginalPrice { get; set; }

    /// <summary>
    /// Giá trị hợp lý
    /// </summary>
    public decimal? EndOfYearFairValue { get; set; }

    /// <summary>
    /// Dự phòng
    /// </summary>
    public decimal? EndOfYearProvision { get; set; }

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

    public virtual EquityInvestment? EquityInvestmentParent { get; set; }

    public virtual ExplanationDetail ExplanationDetail { get; set; } = null!;

    public virtual FinancialAccount? FinancialAccount { get; set; }

    public virtual ICollection<EquityInvestment> InverseEquityInvestmentParent { get; set; } = new List<EquityInvestment>();
}
