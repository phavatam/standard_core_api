﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Business.DTO
{
    public class PermissionApproveDTO
    {
        public bool HasPermissionApprove { get; set; }
        public object CurrentStep { get; set; }
    }
}
