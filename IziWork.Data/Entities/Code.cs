using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Mã số
/// </summary>
public partial class Code
{
    public Guid Id { get; set; }

    public string Code1 { get; set; } = null!;

    public string? Name { get; set; }

    public string? Note { get; set; }

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
}
