using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class FinancialAccountArgs
    {
        public Guid? Id { get; set; }

        public Guid? ParentFinanceAccountId { get; set; }

        public string AccountNo { get; set; } = null!;

        public string? AccountName { get; set; }

        public string? Description { get; set; }
    }
}
