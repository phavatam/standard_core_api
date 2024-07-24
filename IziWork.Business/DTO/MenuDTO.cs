using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class MenuDTO
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; } = null!;

        public Guid? ParentId { get; set; }

        public Guid? GroupId { get; set; }

        public int? Location { get; set; }

        public string? Url { get; set; }

        public string? IconUrl { get; set; }
        public string? VnName { get; set; }


        public virtual MetadataItemDTO Group { get; set; } = null!;

        public virtual MenuLinkDTO? Parent { get; set; }
        public virtual ICollection<MenuLinkDTO> InverseParent { get; set; } = new List<MenuLinkDTO>();

        public ICollection<MenuDepartmentMappingDTO> MenuDepartmentMappings { get; set; } = new List<MenuDepartmentMappingDTO>();

        public ICollection<MenuRoleMappingDTO> MenuRoleMappings { get; set; } = new List<MenuRoleMappingDTO>();

        public ICollection<MenuUserMappingDTO> MenuUserMappings { get; set; } = new List<MenuUserMappingDTO>();
    }
}
