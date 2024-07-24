using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class MetadataType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public DateTimeOffset? Created { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<MetadataItem> MetadataItems { get; set; } = new List<MetadataItem>();
}
