using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Data.Interface
{
    public interface IAuditableEntity : IEntity
    {
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedByFullName { get; set; }
        public string ModifiedByFullName { get; set; }
    }
}
