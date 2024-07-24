using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class UserInDepartmentArgs
    {
        public Guid? Id { get; set; }
        public Guid? DepartmentId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public bool? IsDeleted { get; set; }
        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
