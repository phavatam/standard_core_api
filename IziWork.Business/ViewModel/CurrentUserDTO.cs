using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.ViewModel
{
    public class CurrentUserDTO
    {
        public Guid Id { get; set; }
        public string LoginName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public List<UserDepartmentMappingDTO> UserDepartmentMappingDTO { get; set; }
    }
}
