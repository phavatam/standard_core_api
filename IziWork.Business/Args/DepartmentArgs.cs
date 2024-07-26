using IziWork.Business.DTO;
using IziWork.Common.Enums;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class DepartmentArgs
    {
        public Guid? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public DepartmentTypeEnum Type { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? ProfileId { get; set; }
        public bool IsDeleted { get; set; }
        public string? Note { get; set; }
        public bool? HasTrackingLog { get; set; }
        public virtual ICollection<UserInDepartmentArgs> UserDepartmentMappings { get; set; } = new List<UserInDepartmentArgs>();
    }
}
