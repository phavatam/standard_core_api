using IziWork.Business.Args;
using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface ICategoryDetailBusiness
    {
        Task<ResultDTO> GetListCategory(QueryArgs args);
        Task<ResultDTO> CreateOrUpdate(CategoryDetailArgs args);
        Task<ResultDTO> DeleteById(Guid Id);
        Task<ResultDTO> GetListCategoryDetail(QueryArgs args);
        Task<ResultDTO> GetById(Guid Id);
    }
}
