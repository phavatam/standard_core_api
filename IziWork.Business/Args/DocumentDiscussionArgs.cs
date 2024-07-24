using IziWork.Business.DTO;
using IziWork.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class DocumentAssignTaskArgs
    {
        public Guid DocumentId { get; set; }
        public Guid ProcessorId { get; set; }
        public TaskAssignTypeEnum AssignType { get; set; }
        public List<TaskDepartmentMappingArgs> TaskDepartmentMappings { get; set; } = new List<TaskDepartmentMappingArgs>();
        public string? Content { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public bool? IsReportRequest { get; set; }
        public bool? IsSendMail { get; set; }
        public bool IsCompleted { get; set; }
        public List<Guid> AttachmentFileIds { get; set; } = new List<Guid> { };
    }
}