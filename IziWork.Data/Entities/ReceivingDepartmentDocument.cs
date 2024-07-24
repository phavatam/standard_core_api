using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class ReceivingDepartmentDocument
{
    public Guid Id { get; set; }

    public Guid DepartmentId { get; set; }

    public Guid DocumentId { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;
}
