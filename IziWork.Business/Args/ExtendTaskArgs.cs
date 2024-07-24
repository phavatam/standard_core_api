using IziWork.Business.Enums;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class ExtendTaskArgs
    {
        public Guid TaskManagementId { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public string Note { get; set; }
        public int NumberOfDaysIncurred { get; set; }
        public List<Guid> AttachmentFileIds { get; set; }
    }
}