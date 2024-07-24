using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class TaskDepartmentMapping
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid TaskManagementId { get; set; }

    public bool? IsProcessed { get; set; }

    public bool? IsCoordinated { get; set; }

    public bool? IsViewed { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual Department? Department { get; set; }

    public virtual TaskManagement TaskManagement { get; set; } = null!;

    public virtual User? User { get; set; }
}
