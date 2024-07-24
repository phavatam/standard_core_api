using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class DocumentHistory
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public Guid? ProcessorId { get; set; }

    public string? ProcessorCode { get; set; }

    public string? ProcessorName { get; set; }

    public string? Action { get; set; }

    public Guid? DepartmentId { get; set; }

    public string? DepartmentCode { get; set; }

    public string? DepartmentName { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual Document Document { get; set; } = null!;
}
