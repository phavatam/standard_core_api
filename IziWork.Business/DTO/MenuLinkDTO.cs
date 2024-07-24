using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class MenuLinkDTO
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; } = null!;
        public bool? IsDeleted { get; set; }

    }
}
