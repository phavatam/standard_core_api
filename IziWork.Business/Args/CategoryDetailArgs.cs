
using IziWork.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class CategoryDetailArgs
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public DefineEnums.CATEGORY_TYPE Type { get; set; }
        public string Name { get; set; }
    }
}
