using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class AccountingBalanceSheetTemplateDetail
{
    public Guid Id { get; set; }

    public int Type { get; set; }

    public Guid AccountingBalanceSheetTemplateId { get; set; }

    public Guid? ParentId { get; set; }

    public int? Index { get; set; }

    public string? IndexName { get; set; }

    public string? Name { get; set; }

    public string? Asset { get; set; }

    public Guid? CodeId { get; set; }

    public string? CodeName { get; set; }

    public Guid? ExplanationId { get; set; }

    public string? ExplanationCode { get; set; }

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

    public virtual AccountingBalanceSheetTemplate AccountingBalanceSheetTemplate { get; set; } = null!;

    public virtual ICollection<AccountingBalanceSheetTemplateDetail> InverseParent { get; set; } = new List<AccountingBalanceSheetTemplateDetail>();

    public virtual AccountingBalanceSheetTemplateDetail? Parent { get; set; }
}
