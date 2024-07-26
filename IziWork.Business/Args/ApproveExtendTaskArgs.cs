using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class ApproveExtendTaskArgs
    {
        public Guid TaskManagementId { get; set; }
        public DateTimeOffset? ApproverExtendToDate { get; set; }
        public string? ApproverNote { get; set; }
        public bool IsApproved { get; set; }
    }
}