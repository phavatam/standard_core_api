using IziWork.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class VoteArgs
    {
        public Guid ItemId { get; set; }
        public VoteType Vote { get; set; }
        public string Comment { get; set; }
    }
}
