using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Công nợ
/// </summary>
public partial class DeptManagement
{
    public Guid Id { get; set; }

    public Guid? FinanceAccountId { get; set; }

    public string? Dt { get; set; }

    public string? NameDt { get; set; }
}
