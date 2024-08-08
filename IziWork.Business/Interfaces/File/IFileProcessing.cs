using IziWork.Common.Args;
using IziWork.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces.File
{
    public interface IFileProcessing
    {
        Task<ResultDTO> ImportAsync(FileStream stream);
        Task<ResultDTO> ExportAsync(QueryArgs parameters);
    }
}
