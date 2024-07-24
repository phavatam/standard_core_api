using IziWork.Business.Args;
using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IStatusBusiness
    {
        Task<ResultDTO> CreateOrUpdate(StatusArgs args);
        Task<ResultDTO> DeleteStatusById(Guid Id);
        Task<ResultDTO> GetListStatus(QueryArgs args);
        Task<ResultDTO> GetStatusById(Guid Id);
    }
}
