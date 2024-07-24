using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class TaskDepartmentMappingArgs
    {
        public Guid? DepartmentId { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsProcessed { get; set; }
        public bool? IsCoordinated { get; set; }
        public bool? IsViewed { get; set; }
    }
}
