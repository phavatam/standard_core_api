using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Document
{
    public Guid Id { get; set; }

    public int Type { get; set; }

    public string? ReferenceNumber { get; set; }

    public string Name { get; set; } = null!;

    public DateTimeOffset? IssueDate { get; set; }

    public DateTimeOffset? ExpectedDate { get; set; }

    public Guid? RegistryId { get; set; }

    public string? Status { get; set; }

    public Guid? DocumentTypeId { get; set; }

    public string? ArrivalNumber { get; set; }

    public DateTimeOffset? ArrivalDate { get; set; }

    public bool? IsDirect { get; set; }

    public bool? IsEmail { get; set; }

    public bool? IsFax { get; set; }

    public bool? IsPostOffice { get; set; }

    public string? SentBy { get; set; }

    public Guid? SendingDepartmentId { get; set; }

    public string? SendingDepartmentCode { get; set; }

    public string? SendingDepartmentName { get; set; }

    public DateTimeOffset? DocumentDate { get; set; }

    public Guid? SecurityLevelId { get; set; }

    public Guid? UrgencyLevelId { get; set; }

    public int? BanBo { get; set; }

    public int? PageCount { get; set; }

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

    public virtual ICollection<DocumentAttachmentMapping> DocumentAttachmentMappings { get; set; } = new List<DocumentAttachmentMapping>();

    public virtual ICollection<DocumentDiscussionMapping> DocumentDiscussionMappings { get; set; } = new List<DocumentDiscussionMapping>();

    public virtual ICollection<DocumentForwarding> DocumentForwardings { get; set; } = new List<DocumentForwarding>();

    public virtual ICollection<DocumentHistory> DocumentHistories { get; set; } = new List<DocumentHistory>();

    public virtual ICollection<DocumentProfileMapping> DocumentProfileMappings { get; set; } = new List<DocumentProfileMapping>();

    public virtual CategoryDetail? DocumentType { get; set; }

    public virtual ICollection<ReceivingDepartmentDocument> ReceivingDepartmentDocuments { get; set; } = new List<ReceivingDepartmentDocument>();

    public virtual ICollection<ReferenceDocument> ReferenceDocuments { get; set; } = new List<ReferenceDocument>();

    public virtual CategoryDetail? Registry { get; set; }

    public virtual CategoryDetail? SecurityLevel { get; set; }

    public virtual ICollection<TaskManagement> TaskManagements { get; set; } = new List<TaskManagement>();

    public virtual CategoryDetail? UrgencyLevel { get; set; }
}
