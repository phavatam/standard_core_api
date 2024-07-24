using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class AccountingBalanceSheetTemplate
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? CurrencyUnit { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual ICollection<AccountingBalanceSheetTemplateDetail> AccountingBalanceSheetTemplateDetails { get; set; } = new List<AccountingBalanceSheetTemplateDetail>();

    public virtual ICollection<AccountingBalanceSheet> AccountingBalanceSheets { get; set; } = new List<AccountingBalanceSheet>();
}
