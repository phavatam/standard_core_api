using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class DocumentForwarding
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public int? Type { get; set; }

    public bool? IsSeen { get; set; }

    public Guid? ProcessorId { get; set; }

    public string? ProcessorCode { get; set; }

    public string? ProcessorName { get; set; }

    public int Action { get; set; }

    public Guid? DepartmentId { get; set; }

    public string? DepartmentCode { get; set; }

    public string? DepartmentName { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Document Document { get; set; } = null!;

    public virtual User? Processor { get; set; }
}
