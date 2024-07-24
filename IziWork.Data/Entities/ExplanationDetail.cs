using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Chi tiết thuyết minh
/// </summary>
public partial class ExplanationDetail
{
    public Guid Id { get; set; }

    public Guid ExplanationId { get; set; }

    public Guid? ExplanationDetailParentId { get; set; }

    public int Index { get; set; }

    public string IndexName { get; set; } = null!;

    public string? Name { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual ICollection<AccountingBalanceSheetDetail> AccountingBalanceSheetDetails { get; set; } = new List<AccountingBalanceSheetDetail>();

    public virtual ICollection<BadDebt> BadDebts { get; set; } = new List<BadDebt>();

    public virtual ICollection<CustomerReceivable> CustomerReceivables { get; set; } = new List<CustomerReceivable>();

    public virtual ICollection<EquityInvestment> EquityInvestments { get; set; } = new List<EquityInvestment>();

    public virtual Explanation Explanation { get; set; } = null!;

    public virtual ICollection<ExplanationAddionalInformationTradingSecurity> ExplanationAddionalInformationTradingSecurities { get; set; } = new List<ExplanationAddionalInformationTradingSecurity>();

    public virtual ICollection<ExplanationAdditionalInformationMoney> ExplanationAdditionalInformationMoneys { get; set; } = new List<ExplanationAdditionalInformationMoney>();

    public virtual ICollection<HeldToMaturityInvestment> HeldToMaturityInvestments { get; set; } = new List<HeldToMaturityInvestment>();

    public virtual ICollection<MissingAsset> MissingAssets { get; set; } = new List<MissingAsset>();

    public virtual ICollection<OtherReceivable> OtherReceivables { get; set; } = new List<OtherReceivable>();
}
