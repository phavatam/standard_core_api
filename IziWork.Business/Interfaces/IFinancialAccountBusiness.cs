using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Common.Args;
using IziWork.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IFinancialAccountBusiness
    {
        Task<ResultDTO> GetTreeList(QueryArgs args);
        Task<ResultDTO> GetList(QueryArgs args);
        Task<ResultDTO> UpSert(FinancialAccountArgs args);
        Task<ResultDTO> DeleteById(Guid id);
        Task<ResultDTO> GetById(Guid id);
    }
}
