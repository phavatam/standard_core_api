using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class ReferenceNumber
{
    public Guid Id { get; set; }

    public string? ModuleType { get; set; }

    public int CurrentNumber { get; set; }

    public bool IsNewYearReset { get; set; }

    public string? Formula { get; set; }

    public int CurrentYear { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }
}
