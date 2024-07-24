using IziWork.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Data.Abstracts
{
    public class SoftIdEntity : ISoftIdEntity
    {
        public Guid Id { get; set; }
    }
}
