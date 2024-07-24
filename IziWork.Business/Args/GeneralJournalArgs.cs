using IziWork.Business.DTO;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class GeneralJournalArgs
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Year { get; set; } = null!;
        public virtual List<GeneralJournalDetailArgs> GeneralJournalDetails { get; set; } = new List<GeneralJournalDetailArgs>();
    }
}
