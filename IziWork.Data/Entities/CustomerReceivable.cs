using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Phải thu của khách hàng
/// </summary>
public partial class CustomerReceivable
{
    public Guid Id { get; set; }

    public Guid ExplanationDetailId { get; set; }

    public Guid? CustomerReceivableParentId { get; set; }

    public string TargetName { get; set; } = null!;

    public decimal? StartOfYear { get; set; }

    public decimal? EndOfYear { get; set; }

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

    public virtual CustomerReceivable? CustomerReceivableParent { get; set; }

    public virtual ExplanationDetail ExplanationDetail { get; set; } = null!;

    public virtual FinancialAccount? FinancialAccount { get; set; }

    public virtual ICollection<CustomerReceivable> InverseCustomerReceivableParent { get; set; } = new List<CustomerReceivable>();
}
