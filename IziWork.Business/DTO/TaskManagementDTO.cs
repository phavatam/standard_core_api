using IziWork.Data.Entities;
using System;
using System.Collections.Generic;

namespace IziWork.Business.DTO;

public partial class TaskManagementDTO
{
    public Guid? Id { get; set; }
    public Guid? ParentTaskId { get; set; }
    public string Name { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? ProcessorId { get; set; }
    public string ProcessorCode { get; set; }
    public string ProcessorName { get; set; }
    public Guid DocumentId { get; set; }
    public int? Type { get; set; }
    public int? CriticalLevel { get; set; }
    public Guid? ClassifyId { get; set; }
    public double? Hour { get; set; }
    public int? PercentCompleted { get; set; }
    public string? Content { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public bool? IsReportRequest { get; set; }
    public bool? IsSendMail { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public Guid? CreatedById { get; set; }
    public Guid? ModifiedById { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public string? CreatedByFullName { get; set; }
    public string? ModifiedByFullName { get; set; }
    public virtual ICollection<TaskExtendDTO> TaskExtends { get; set; } = new List<TaskExtendDTO>();
    /*public virtual ICollection<TaskAttachmentMapping> TaskAttachmentMappings { get; set; } = new List<TaskAttachmentMapping>();*/
    public virtual ICollection<TaskDepartmentMappingDTO> TaskDepartmentMappings { get; set; } = new List<TaskDepartmentMappingDTO>();
}
