using IziWork.Business.DTO;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class GeneralJournalDetailArgs
    {
        public Guid? Id { get; set; }
        public Guid? GeneralJournalId { get; set; }
        public DateTimeOffset DateOfPeriod { get; set; }
        public string? DocumentNo { get; set; } = null!;
        public DateTimeOffset? DocumentDate { get; set; }
        public string? Description { get; set; }
        public Guid DebitAccountId { get; set; }
        public Guid CreditAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
