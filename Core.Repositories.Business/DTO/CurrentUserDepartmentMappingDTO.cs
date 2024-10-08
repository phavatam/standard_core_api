﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Business.DTO
{
    public class CurrentUserDepartmentMappingDTO
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserLoginName { get; set; }
        public bool IsHeadCount { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? ModifiedByFullName { get; set; }
        public List<CurrentUserDepartmentRoleMappingDTO> UserDepartmentRoleMappings { get; set; } = new List<CurrentUserDepartmentRoleMappingDTO>();
        public List<Guid> RoleIds { get; set; }
    }
}
