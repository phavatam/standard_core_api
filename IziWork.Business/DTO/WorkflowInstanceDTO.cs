using IziWork.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class WorkflowInstanceDTO
    {
        private WorkflowDataDTO _data;
        public Guid Id { get; set; }
        public string? WorkflowName { get; set; }
        public Guid TemplateId { get; set; }
        public string? WorkflowDataStr { get; set; }
        [NotMapped]
        public WorkflowDataDTO WorkflowData
        {
            get
            {

                if (_data == null)
                {
                    if (!string.IsNullOrEmpty(WorkflowDataStr))
                    {
                        _data = JsonConvert.DeserializeObject<WorkflowDataDTO>(WorkflowDataStr);
                    }
                    else
                    {
                        _data = null;
                    }
                }
                return _data;
            }
            set
            {
                WorkflowDataStr = JsonConvert.SerializeObject(value);
            }
        }
        public Guid ItemId { get; set; }
        public string? ItemReferenceNumber { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsTerminated { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public string? DefaultCompletedStatus { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? ModifiedByFullName { get; set; }
        public int? RoundNum { get; set; }
        //public virtual WorkflowTemplate Template { get; set; } = null!;
        //public virtual List<WorkflowProcessingDTO> WorkflowProcessings { get; set; } = new List<WorkflowProcessingDTO>() { };
    }
}
