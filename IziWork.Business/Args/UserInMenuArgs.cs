﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class UserInMenuArgs
    {
        public Guid? Id { get; set; }
        public Guid MenuId { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
    }
}
