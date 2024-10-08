﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class UserDepartmentRoleMappingDTO
    {
        public Guid Id { get; set; }
        public Guid? UserDepartmentMappingId { get; set; }
        public Guid? RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool IsActivated { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? ModifiedByFullName { get; set; }
    }
}
