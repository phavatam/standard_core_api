using Core.Repositories.Business.IRepositories;
using IziWork.Business.Interfaces.File;
using IziWork.Common.Args;
using IziWork.Common.DTO;
using IziWork.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Handlers.File
{
    public class ExcuteFileProcessing : IExcuteFileProcessing
    {
        private readonly IUnitOfWork _uow;
        public ExcuteFileProcessing(IUnitOfWork uow) {
            _uow = uow;
        }
        public IFileProcessing CreatedFileProcessing(FileProcessingType type)
        {
            switch (type)
            {
                case FileProcessingType.COMPANY:
                    return new CompanyProcessingBO(_uow);
            }
            return null;
        }
        public Task<ResultDTO> ExportAsync(FileProcessingType type, QueryArgs parameters)
        {
            var fileProcessing = CreatedFileProcessing(type);
            return fileProcessing.ExportAsync(parameters);
        }

        public async Task<ResultDTO> ImportAsync(FileProcessingType type, FileStream fileStream)
        {
            var fileProcessing = CreatedFileProcessing(type);
            return await fileProcessing.ImportAsync(fileStream);
        }

    }
}
