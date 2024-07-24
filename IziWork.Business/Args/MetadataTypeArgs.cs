using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class MetadataTypeArgs
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string Code { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual ICollection<MetadataArgs> MetadataItems { get; set; } = new List<MetadataArgs>();
    }
}
