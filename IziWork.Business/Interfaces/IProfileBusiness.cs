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
    public interface IProfileBusiness
    {
        Task<ResultDTO> CreateOrUpdate(ProfileArgs args);
        Task<ResultDTO> DeleteById(Guid Id);
        Task<ResultDTO> GetList(QueryArgs args);
        Task<ResultDTO> GetById(Guid Id);
    }
}
