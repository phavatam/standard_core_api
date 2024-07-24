using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.ViewModel
{
    public class MenuTreeViewModel
    {
        public MenuTreeViewModel()
        {
            InverseParent = new HashSet<MenuTreeViewModel>();
        }
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; } = null!;

        public Guid? ParentId { get; set; }

        public Guid? GroupId { get; set; }

        public int? Location { get; set; }
        public string? Url { get; set; }

        public string? IconUrl { get; set; }
        public string? VnName { get; set; }


        public virtual MetadataItemDTO Group { get; set; } = null!;

        public virtual MenuDTO? Parent { get; set; }
        public virtual ICollection<MenuTreeViewModel> InverseParent { get; set; } = new List<MenuTreeViewModel>();

        public ICollection<MenuDepartmentMappingDTO> MenuDepartmentMappings { get; set; } = new List<MenuDepartmentMappingDTO>();

        public ICollection<MenuRoleMappingDTO> MenuRoleMappings { get; set; } = new List<MenuRoleMappingDTO>();

        public ICollection<MenuUserMappingDTO> MenuUserMappings { get; set; } = new List<MenuUserMappingDTO>();
    }
}
