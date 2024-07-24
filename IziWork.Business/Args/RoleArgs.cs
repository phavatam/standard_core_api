using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class RoleArgs
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public string? Code { get; set; }
        public bool IsActivated { get; set; }
    }
}
