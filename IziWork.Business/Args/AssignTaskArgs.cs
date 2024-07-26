using IziWork.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class AssignTaskArgs
    {
        public Guid? Id { get; set; }
        public Guid? ParentTaskId { get; set; }
        public string Name { get; set; }
        public TaskTypeEnum Type { get; set; }
        public int CriticalLevel { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public float Hour { get; set; }
        public Guid? ClassifyId { get; set; }
        public string Content { get; set; }
        public int? PercentCompleted { get; set; }
        public bool IsCompleted { get; set; }
        public List<Guid> AttachmentFileIds { get; set; } = new List<Guid> { };
        public List<TaskDepartmentMappingArgs> TaskDepartmentMappings { get; set; } = new List<TaskDepartmentMappingArgs>();
    }
}
