using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class DocumentDTO
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string ReferenceNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTimeOffset? IssueDate { get; set; }
        public DateTimeOffset? ExpectedDate { get; set; }
        public string Status { get; set; } = null!;
        public Guid RegistryId { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string ArrivalNumber { get; set; } = null!;
        public DateTimeOffset ArrivalDate { get; set; }
        public bool? IsDirect { get; set; }
        public bool? IsEmail { get; set; }
        public bool? IsFax { get; set; }
        public bool? IsPostOffice { get; set; }
        public string? SentBy { get; set; }
        public Guid? SendingDepartmentId { get; set; }
        public string? SendingDepartmentCode { get; set; }
        public string? SendingDepartmentName { get; set; }
        public DateTimeOffset? DocumentDate { get; set; }
        public string? DocumentSet { get; set; }
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
        public List<DocumentAttachmentMappingDTO> DocumentAttachmentMappings { get; set; }
        public virtual ICollection<DocumentDiscussionMappingDTO> DocumentDiscussionMappings { get; set; } 
        public virtual ICollection<DocumentForwardingDTO> DocumentForwardings { get; set; }
    }
}