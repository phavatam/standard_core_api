using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Nhật ký chung
/// </summary>
public partial class GeneralJournal
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

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

    public virtual CompanyInfo Company { get; set; } = null!;

    public virtual ICollection<GeneralJournalDetail> GeneralJournalDetails { get; set; } = new List<GeneralJournalDetail>();
}
