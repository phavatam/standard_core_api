using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Business.DTO
{
    public class CurrentUserDTO
    {
        public Guid Id { get; set; }
        public string LoginName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public List<CurrentUserDepartmentMappingDTO> UserDepartmentMappingDTO { get; set; }
    }
}
