using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.CustomExtensions
{
    public class GlobalData
    {
        private GlobalData()
        {
        }
        private static readonly Lazy<GlobalData> lazy = new Lazy<GlobalData>(() => new GlobalData());
        public static GlobalData Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        public Dictionary<string, ReferenceNumber> Refs { get; set; }
        public object GlobalLock = new object();
    }
}
