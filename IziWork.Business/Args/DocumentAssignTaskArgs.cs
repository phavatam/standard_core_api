using IziWork.Business.DTO;
using IziWork.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class DocumentDiscussionArgs
    {
        public Guid DocumentId { get; set; }
        public Guid? ParentDiscussionId { get; set; }
        public string Message { get; set; }
    }
}