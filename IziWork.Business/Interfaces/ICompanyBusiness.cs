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
    public interface ICompanyBusiness
    {
        Task<ResultDTO> GetListCompany(QueryArgs args);
        Task<ResultDTO> UpSertCompany(CompanyArgs args);
        Task<ResultDTO> DeleteCompanyById(Guid companyId);
        Task<ResultDTO> GetCompanyById(Guid companyId);
        Task<ResultDTO> UploadData(Stream stream);
    }
}
