using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Discussion
{
    public Guid Id { get; set; }

    public Guid? ParentDiscussionId { get; set; }

    public string? Message { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual ICollection<DocumentDiscussionMapping> DocumentDiscussionMappings { get; set; } = new List<DocumentDiscussionMapping>();

    public virtual ICollection<Discussion> InverseParentDiscussion { get; set; } = new List<Discussion>();

    public virtual Discussion? ParentDiscussion { get; set; }
}
