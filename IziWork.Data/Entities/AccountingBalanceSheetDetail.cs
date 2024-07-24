using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Bảng cân đối kế toán chi tiết
/// </summary>
public partial class AccountingBalanceSheetDetail
{
    public Guid Id { get; set; }

    public Guid AccountingBalanceSheetId { get; set; }

    public Guid? AccountingBalanceSheetDetailParentId { get; set; }

    public int? Index { get; set; }

    public string? IndexName { get; set; }

    public string? Asset { get; set; }

    public Guid? CodeId { get; set; }

    public string? CodeName { get; set; }

    public Guid? ExplanationDetailId { get; set; }

    public double? EndOfYear { get; set; }

    public double? StartOfYear { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual AccountingBalanceSheet AccountingBalanceSheet { get; set; } = null!;

    public virtual AccountingBalanceSheetDetail? AccountingBalanceSheetDetailParent { get; set; }

    public virtual Code? Code { get; set; }

    public virtual ExplanationDetail? ExplanationDetail { get; set; }

    public virtual ICollection<AccountingBalanceSheetDetail> InverseAccountingBalanceSheetDetailParent { get; set; } = new List<AccountingBalanceSheetDetail>();
}
