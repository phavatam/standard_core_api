using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Data.Interface
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTimeOffset? Created { get; set; }
        DateTimeOffset? Modified { get; set; }
    }
}
