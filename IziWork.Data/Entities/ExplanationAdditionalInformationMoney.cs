using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Chi phí khác về tiền
/// </summary>
public partial class ExplanationAdditionalInformationMoney
{
    public Guid Id { get; set; }

    public Guid ExplanationDetailId { get; set; }

    public Guid? ExplanationAdditionalInformationMoneyParentId { get; set; }

    public string TargetName { get; set; } = null!;

    public int Index { get; set; }

    public string IndexName { get; set; } = null!;

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

    public virtual ExplanationAdditionalInformationMoney? ExplanationAdditionalInformationMoneyParent { get; set; }

    public virtual ExplanationDetail ExplanationDetail { get; set; } = null!;

    public virtual FinancialAccount? FinancialAccount { get; set; }

    public virtual ICollection<ExplanationAdditionalInformationMoney> InverseExplanationAdditionalInformationMoneyParent { get; set; } = new List<ExplanationAdditionalInformationMoney>();
}
