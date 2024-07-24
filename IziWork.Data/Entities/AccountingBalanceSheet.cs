using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Bảng cân đối kế toán
/// </summary>
public partial class AccountingBalanceSheet
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid CompanyInfoId { get; set; }

    public string? TemplateNo { get; set; }

    public string? CurrencyUnit { get; set; }

    public Guid? AccountingBalanceSheetTemplateId { get; set; }

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

    public virtual AccountingBalanceSheetTemplate? AccountingBalanceSheetTemplate { get; set; }
}
