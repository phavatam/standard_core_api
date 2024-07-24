using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Department
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public Guid? ParentId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public string? Note { get; set; }

    public bool? HasTrackingLog { get; set; }

    public Guid? ProfileId { get; set; }

    public virtual ICollection<DocumentForwarding> DocumentForwardings { get; set; } = new List<DocumentForwarding>();

    public virtual ICollection<Department> InverseParent { get; set; } = new List<Department>();

    public virtual ICollection<MenuDepartmentMapping> MenuDepartmentMappings { get; set; } = new List<MenuDepartmentMapping>();

    public virtual Department? Parent { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual Profile? Profile { get; set; }

    public virtual ICollection<ReceivingDepartmentDocument> ReceivingDepartmentDocuments { get; set; } = new List<ReceivingDepartmentDocument>();

    public virtual ICollection<TaskDepartmentMapping> TaskDepartmentMappings { get; set; } = new List<TaskDepartmentMapping>();

    public virtual ICollection<UserDepartmentMapping> UserDepartmentMappings { get; set; } = new List<UserDepartmentMapping>();

    public virtual ICollection<WorkflowProcessing> WorkflowProcessings { get; set; } = new List<WorkflowProcessing>();

    public virtual ICollection<WorkflowStep> WorkflowSteps { get; set; } = new List<WorkflowStep>();
}
