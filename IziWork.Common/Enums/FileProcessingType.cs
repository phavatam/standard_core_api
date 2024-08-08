using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Common.Enums
{
    public enum FileProcessingType
    {
        [Description("Company")]
        COMPANY = 1,
        [Description("Financial Account")]
        FINANCIALACCOUNT = 2,
        [Description("General Journal")]
        GENERALJOURNAL = 3
    }
}
