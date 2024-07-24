using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Enums
{
    [Flags]
    public enum PermEnum
    {
        None = 0,
        View = 1,
        Edit = 2,
        Delete = 4,
        Full = View | Edit | Delete
    }
}
