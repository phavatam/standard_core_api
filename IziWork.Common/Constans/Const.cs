using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Common.Constans
{
    public static class CONST
    {
        public static readonly string _KeyCORS = "";
        public static class ROLE
        {
            public static readonly string ACCOUNTING = "Accounting";
            public static readonly Guid ACCOUNTING_ID = new Guid("e1a8f721-4092-b2e8-b3e3-f78f6c1b1d0d");
            public static readonly string RECORDSCLERK = "RecordsClerk";
            public static readonly Guid RECORDSCLERK_ID = new Guid("e2a8f726-4098-b2e7-b3e3-f77f6c1b2d9d");
            public static readonly string MEMBER = "Member";
            public static readonly Guid MEMBER_ID = new Guid("e5a8f784-5f93-b8e3-4998-f66f7c2b2d7d");
            public static readonly string HOD = "HOD";
            public static readonly Guid HOD_ID = new Guid("e2a8f726-4998-b8e7-b8e3-f77f6c1b2d9d");
        }

        public static class WORKFLOW
        {
            public static readonly Guid PERSONAL_APPROVAL = new Guid("e5a8f784-4998-b6e7-6f93-f66f6c2b2d7d");
        }
    }
}
