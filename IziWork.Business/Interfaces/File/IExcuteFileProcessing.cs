using IziWork.Common.Args;
using IziWork.Common.DTO;
using IziWork.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces.File
{
    public interface IExcuteFileProcessing
    {
        Task<ResultDTO> ImportAsync(FileProcessingType type, FileStream fileStream);
        Task<ResultDTO> ExportAsync(FileProcessingType type, QueryArgs parameters);
    }
}
