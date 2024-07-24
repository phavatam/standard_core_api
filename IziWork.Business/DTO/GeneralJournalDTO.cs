using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class GeneralJournalDTO
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Year { get; set; } = null!;
        public virtual ICollection<GeneralJournalDetailDTO> GeneralJournalDetails { get; set; } = new List<GeneralJournalDetailDTO>();
    }
}
