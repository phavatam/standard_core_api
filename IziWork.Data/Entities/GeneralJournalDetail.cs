using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class GeneralJournalDetail
{
    public Guid Id { get; set; }

    public Guid GeneralJournalId { get; set; }

    public DateTimeOffset DateOfPeriod { get; set; }

    public string DocumentNo { get; set; } = null!;

    public DateTimeOffset DocumentDate { get; set; }

    public string? Description { get; set; }

    public Guid? DebitAccountId { get; set; }

    public Guid? CreditAccountId { get; set; }

    public decimal Amount { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual FinancialAccount? CreditAccount { get; set; }

    public virtual FinancialAccount? DebitAccount { get; set; }

    public virtual GeneralJournal GeneralJournal { get; set; } = null!;
}
