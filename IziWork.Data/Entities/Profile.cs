using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Profile
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<DocumentProfileMapping> DocumentProfileMappings { get; set; } = new List<DocumentProfileMapping>();

    public virtual ICollection<ProfileAttachmentFileMapping> ProfileAttachmentFileMappings { get; set; } = new List<ProfileAttachmentFileMapping>();
}
