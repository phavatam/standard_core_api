using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class DocumentArgs
    {
        public Guid? Id { get; set; }
        public bool IsSaveDraft { get; set; }
        public string ReferenceNumber { get; set; }
        public List<Guid>? ReceivingDepartments { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; } = null!;
        public DateTimeOffset? IssueDate { get; set; }
        public DateTimeOffset? ExpectedDate { get; set; }
        public Guid? RegistryId { get; set; }
        public Guid? DocumentTypeId { get; set; }
        public string? ArrivalNumber { get; set; } = null!;
        public DateTimeOffset? ArrivalDate { get; set; }
        public bool? IsDirect { get; set; }
        public bool? IsEmail { get; set; }
        public bool? IsFax { get; set; }
        public bool? IsPostOffice { get; set; }
        public string? SentBy { get; set; }
        public Guid? SendingDepartmentId { get; set; }
        public DateTimeOffset? DocumentDate { get; set; }
        public Guid? SecurityLevelId { get; set; }
        public Guid? UrgencyLevelId { get; set; }
        public int? BanBo { get; set; }
        public int? PageCount { get; set; }
        public string? Description { get; set; }
        public List<Guid>? AttachmentFileIds { get; set; } = new List<Guid>();
        /*public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public virtual ICollection<DocumentAttachmentMapping> DocumentAttachmentMappings { get; set; } = new List<DocumentAttachmentMapping>();
        public virtual ICollection<DocumentDiscussionMapping> DocumentDiscussionMappings { get; set; } = new List<DocumentDiscussionMapping>();
        public virtual ICollection<DocumentForwarding> DocumentForwardings { get; set; } = new List<DocumentForwarding>();
        public virtual ICollection<DocumentProfileMapping> DocumentProfileMappings { get; set; } = new List<DocumentProfileMapping>();
        public virtual ICollection<ReceivingDepartmentDocument> ReceivingDepartmentDocuments { get; set; } = new List<ReceivingDepartmentDocument>();*/
    }
}
