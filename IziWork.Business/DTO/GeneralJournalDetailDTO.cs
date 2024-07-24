using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class GeneralJournalDetailDTO
    {
        public Guid Id { get; set; }
        public Guid GeneralJournalId { get; set; }
        public DateTimeOffset DateOfPeriod { get; set; }
        public string DocumentNo { get; set; } = null!;
        public DateTimeOffset DocumentDate { get; set; }
        public string? Description { get; set; }
        public Guid? DebitAccountId { get; set; }
        public Guid? CreditAccountId { get; set; }
        public decimal Amount { get; set; }
        public FinancialAccountDTO DebitAccount { get; set; }
        public FinancialAccountDTO CreditAccount { get; set; }
    }
}
