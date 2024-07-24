using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class WardMasterDatum
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public virtual ICollection<CompanyInfo> CompanyInfos { get; set; } = new List<CompanyInfo>();
}
