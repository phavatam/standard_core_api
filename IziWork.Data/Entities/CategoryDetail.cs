using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class CategoryDetail
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Document> DocumentDocumentTypes { get; set; } = new List<Document>();

    public virtual ICollection<Document> DocumentRegistries { get; set; } = new List<Document>();

    public virtual ICollection<Document> DocumentSecurityLevels { get; set; } = new List<Document>();

    public virtual ICollection<Document> DocumentUrgencyLevels { get; set; } = new List<Document>();

    public virtual ICollection<TaskManagement> TaskManagements { get; set; } = new List<TaskManagement>();
}
