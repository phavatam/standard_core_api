using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string? FullName { get; set; }

    public string LoginName { get; set; } = null!;

    public string? Password { get; set; }

    public int? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool IsActivated { get; set; }

    public int? Type { get; set; }

    public int? Role { get; set; }

    public bool? IsBlocked { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual ICollection<DocumentForwarding> DocumentForwardings { get; set; } = new List<DocumentForwarding>();

    public virtual ICollection<MenuUserMapping> MenuUserMappings { get; set; } = new List<MenuUserMapping>();

    public virtual ICollection<Navigation> Navigations { get; set; } = new List<Navigation>();

    public virtual ICollection<TaskDepartmentMapping> TaskDepartmentMappings { get; set; } = new List<TaskDepartmentMapping>();

    public virtual ICollection<TaskManagement> TaskManagements { get; set; } = new List<TaskManagement>();

    public virtual ICollection<UserDepartmentMapping> UserDepartmentMappings { get; set; } = new List<UserDepartmentMapping>();

    public virtual ICollection<WorkflowProcessing> WorkflowProcessings { get; set; } = new List<WorkflowProcessing>();

    public virtual ICollection<WorkflowStep> WorkflowSteps { get; set; } = new List<WorkflowStep>();
}
