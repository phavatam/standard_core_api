using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IziWork.Business.Constans.CONST;

namespace IziWork.Business.Args
{
    public class StatusArgs
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsWorkflow { get; set; }
        // public int Type { get; set; }
        public bool IsActive { get; set; }
    }
}
