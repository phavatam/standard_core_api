using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Data.Interface
{
    public interface ISoftDeleteEntity : IEntity
    {
        bool? IsDeleted { get; set; }
    }
}
