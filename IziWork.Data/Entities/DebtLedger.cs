using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Công nợ
/// </summary>
public partial class DebtLedger
{
    public Guid Id { get; set; }

    public Guid CompanyInfoId { get; set; }

    public string? TemplateNo { get; set; }

    public string Name { get; set; } = null!;

    public string Year { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual CompanyInfo CompanyInfo { get; set; } = null!;

    public virtual ICollection<DebtLedgerDetail> DebtLedgerDetails { get; set; } = new List<DebtLedgerDetail>();
}
