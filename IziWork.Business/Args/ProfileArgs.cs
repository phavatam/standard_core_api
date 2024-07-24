using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class ProfileArgs
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Guid> AttachmentFileIds { get; set; } = new List<Guid>();
    }
}
