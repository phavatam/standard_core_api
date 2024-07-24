using IziWork.Business.Args;
using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IGeneralJournalBusiness
    {
        Task<ResultDTO> GetListGeneralJournal(QueryArgs args);
        Task<ResultDTO> CreateOrUpdateGeneralJournal(GeneralJournalArgs args);
        Task<ResultDTO> CreateOrUpdateOneRowGeneralJournalDetail(GeneralJournalDetailArgs args);
        Task<ResultDTO> DeleteGeneralJournal(Guid Id);
        Task<ResultDTO> DeleteGeneralJournalDetail(Guid Id);
    }
}