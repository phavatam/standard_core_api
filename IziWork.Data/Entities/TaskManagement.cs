using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class TaskManagement
{
    public Guid Id { get; set; }

    public Guid? ParentTaskId { get; set; }

    public string? Name { get; set; }

    public string? ReferenceNumber { get; set; }

    public Guid? ProcessorId { get; set; }

    public int? AssignType { get; set; }

    public Guid? DocumentId { get; set; }

    public int? Type { get; set; }

    public int? CriticalLevel { get; set; }

    public Guid? ClassifyId { get; set; }

    public string? Content { get; set; }

    public DateTimeOffset? FromDate { get; set; }

    public DateTimeOffset? ToDate { get; set; }

    public double? Hour { get; set; }

    public int? PercentCompleted { get; set; }

    public bool? IsReportRequest { get; set; }

    public string? Status { get; set; }

    public bool? IsSendMail { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual CategoryDetail? Classify { get; set; }

    public virtual Document? Document { get; set; }

    public virtual ICollection<TaskManagement> InverseParentTask { get; set; } = new List<TaskManagement>();

    public virtual TaskManagement? ParentTask { get; set; }

    public virtual User? Processor { get; set; }

    public virtual ICollection<TaskAttachmentMapping> TaskAttachmentMappings { get; set; } = new List<TaskAttachmentMapping>();

    public virtual ICollection<TaskDepartmentMapping> TaskDepartmentMappings { get; set; } = new List<TaskDepartmentMapping>();

    public virtual ICollection<TaskExtend> TaskExtends { get; set; } = new List<TaskExtend>();

    public virtual ICollection<TaskManagementHistory> TaskManagementHistories { get; set; } = new List<TaskManagementHistory>();
}
