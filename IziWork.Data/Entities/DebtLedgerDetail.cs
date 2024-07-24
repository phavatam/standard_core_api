using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class DebtLedgerDetail
{
    public Guid Id { get; set; }

    public Guid DebtLedgerId { get; set; }

    /// <summary>
    /// Đối tượng
    /// </summary>
    public string Dt { get; set; } = null!;

    public string NameDt { get; set; } = null!;

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

    public virtual DebtLedger DebtLedger { get; set; } = null!;
}
