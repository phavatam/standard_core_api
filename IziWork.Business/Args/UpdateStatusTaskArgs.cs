﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class UpdateStatusTaskArgs
    {
        public Guid TaskManagementId { get; set; }
        public string Status { get; set; }
    }
}
