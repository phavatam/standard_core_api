using Core.Repositories.Business.IRepositories;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces.File;
using IziWork.Common.Args;
using IziWork.Common.DTO;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Handlers.File
{
    public class CompanyProcessingBO : BaseExcelProcessing, IExcelProcessingBO
    {
        protected override string Json => "export-mapping.json";
        protected override string JsonGroupName => "CompanyInfo";
        public CompanyProcessingBO(IUnitOfWork uow) : base(uow)
        {

        }
        public async Task<ResultDTO> ExportAsync(QueryArgs parameters)
        {
            var fieldMappings = ReadConfigurationFromFile();
            var headers = fieldMappings.Select(y => y.DisplayName);
            
            DataTable tbl = new DataTable();
            foreach (var headerItem in headers)
            {
                tbl.Columns.Add(headerItem);
            }
            
            var headcounts = await _uow.GetRepository<CompanyInfo>().FindByAsync<CompanyInfoImportFileDTO>(parameters.Predicate, parameters.PredicateParameters);
            if (headcounts.Any())
            {
                headcounts = headcounts.OrderBy(x => x.Created).ThenByDescending(y => y.Modified);
                for (int rowNum = 0; rowNum < headcounts.Count(); rowNum++)
                {
                    DataRow row = tbl.Rows.Add();
                    var data = headcounts.ElementAt(rowNum);
                    for (int j = 0; j < fieldMappings.Count; j++)
                    {
                        var fieldMapping = fieldMappings[j];
                        var value = data.GetType().GetProperty(fieldMapping.Name).GetValue(data);
                        HandleCommonType(row, value, j, fieldMapping);
                    }
                }
            }
            else
            {
                return new ResultDTO { ErrorCodes = { 1003 }, Messages = { "No Data" } };
            }
            var creatingExcelFileReslult = ExportExcel(tbl);
            if (creatingExcelFileReslult == null)
            {
                return new ResultDTO { ErrorCodes = { 1003 }, Messages = { "No Data" } };
            }
            return new ResultDTO { Object = creatingExcelFileReslult };

        }
        public Task<ResultDTO> ImportAsync(FileStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
