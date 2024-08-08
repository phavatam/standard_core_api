using AutoMapper;
using Core.Repositories.Business.Helpers;
using Core.Repositories.Business.IRepositories;
using IziWork.Business.Args;

using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWork.Common.Args;
using IziWork.Common.Constans;
using IziWork.Common.DTO;
using IziWork.Data.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IziWork.Business.Handlers
{
    public class CompanyBusiness : ICompanyBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly AutoMapper.IMapper _mapper;
        public CompanyBusiness(IUnitOfWork uow, AutoMapper.IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<ResultDTO> GetListCompany(QueryArgs args)
        {
            var userList = await _uow.GetRepository<CompanyInfo>().FindByAsync<CompanyInfoImportFileDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<CompanyInfo>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = userList,
                    Count = totalList
                }
            };
        }
        public async Task<ResultDTO> UpSertCompany(CompanyArgs args)
        {
            var resultDTO = new ResultDTO();

            if (args is null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            if (string.IsNullOrEmpty(args.CompanyName))
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "NAME_IS_REQUIRE" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            
            if (args.Id != null && args.Id.Value != Guid.Empty)
            {
                /*var checkExistCompany = _uow.GetRepository<Role>().GetSingle(x => x.Name.ToLower().Equals(args.co.ToLower()) && x.Id != args.Id);
                if (checkExistCompany != null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { "ROLE_IS_EXIST" }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }*/
                var existsCompany = _uow.GetRepository<CompanyInfo>().GetSingle(x => x.Id == args.Id);
                if (existsCompany == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_COMPANY }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                existsCompany.TaxNo = args.TaxNo;
                existsCompany.CompanyName = args.CompanyName;
                existsCompany.Address = args.Address;
                existsCompany.WardId = args.WardId;
                existsCompany.ProvinceId = args.ProvinceId;
                existsCompany.FullAddress = args.FullAddress;
                existsCompany.PhoneNumber = args.PhoneNumber;
                existsCompany.ReportingPeriod = args.ReportingPeriod;
                existsCompany.ReportingDate = args.ReportingDate;
                existsCompany.ReportingDateInWords = args.ReportingDateInWords;
                existsCompany.PreparedByName = args.PreparedByName;
                existsCompany.AccountantId = args.AccountantId;
                existsCompany.AccountantName = args.TaxNo;
                existsCompany.CeoId = args.CeoId;
                existsCompany.CeoName = args.CeoName;
                existsCompany.PositionName = args.PositionName;
                existsCompany.BusinessSector = args.BusinessSector;
                existsCompany.RegulatoryAgency = args.RegulatoryAgency;
                existsCompany.AccountingMethod = args.AccountingMethod;
                existsCompany.ReportingPeriodAbbreviation = args.ReportingPeriodAbbreviation;
                existsCompany.OwnershipForm = args.OwnershipForm;
                existsCompany.TotalEmployees = args.TotalEmployees;
            //existsCompany = _mapper.Map<CompanyInfo>(args);
                var updatedCompany = _uow.GetRepository<CompanyInfo>().Update(existsCompany);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { MessageConst.UPDATE_SUCCESSFULLY },
                    Object = _mapper.Map<CompanyInfoImportFileDTO>(updatedCompany)
                };
                goto Finish;
            }
            else
            {
                var mapDataCompany = _mapper.Map<CompanyInfo>(args);
                var addNewCompany = _uow.GetRepository<CompanyInfo>().Add(mapDataCompany);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { MessageConst.CREATED_SUCCESSFULLY },
                    Object = _mapper.Map<CompanyArgs>(addNewCompany)
                };
                goto Finish;
            }
        Finish:
            return resultDTO;
        }
        public async Task<ResultDTO> DeleteCompanyById(Guid companyId)
        {
            var resultDTO = new ResultDTO();
            var currentItem = await _uow.GetRepository<CompanyInfo>().GetSingleAsync(x => x.Id == (companyId));
            #region Verify user 
            if (currentItem == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_COMPANY }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            #endregion
            _uow.GetRepository<CompanyInfo>().Delete(currentItem);
            await _uow.CommitAsync();
            resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.DELETE_SUCCESSFULLY } };
        Finish:
            return resultDTO;
        }

        public async Task<ResultDTO> GetCompanyById(Guid companyId)
        {
            var resultDTO = new ResultDTO();
            var findItem = await _uow.GetRepository<CompanyInfo>().GetSingleAsync<CompanyInfoImportFileDTO>(x => x.Id == companyId);
            if (findItem == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_COMPANY }, ErrorCodes = new List<int> { -1 } };
            }
            resultDTO.Object = findItem;
            return resultDTO;
        }

        public List<CompanyInfoImportFileDTO> ReadDataFromStream(Stream stream)
        {
            var listData = new List<CompanyInfoImportFileDTO>();
            using (var pck = new ExcelPackage(stream))
            {
                ExcelWorkbook WorkBook = pck.Workbook;
                ExcelWorksheet workSheet = WorkBook.Worksheets.ElementAt(0);
                for (var rowNumber = 4; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                {
                    CompanyInfoImportFileDTO data = new CompanyInfoImportFileDTO();
                    var objdate = workSheet.Cells[rowNumber, 4, rowNumber, workSheet.Dimension.End.Column][$"B{rowNumber}"];
                    //Fixed #200
                    string dateValueFormat = objdate is null ? string.Empty : objdate.Text;

                    if (!string.IsNullOrEmpty(dateValueFormat))
                    {
                        DateTime dateValue;
                        if (DateTime.TryParse(dateValueFormat, out dateValue))
                        {
                            dateValueFormat = dateValue.ToString("dd/MM/yyyy");
                        }
                    }
                    if (Utilities.IsValidDatetimeFormat(dateValueFormat))
                    {
                        //data.DateOfOT = Utilities.ConvertStringToDatetime(dateValueFormat);
                    }
                    data.LineOfExcel = rowNumber.ToString();
                    data.CompanyName = workSheet.Cells[rowNumber, 2, rowNumber, workSheet.Dimension.End.Column][$"A{rowNumber}"].Text.ToSAPFormatString();
                    /*data.From = workSheet.Cells[rowNumber, 5, rowNumber, workSheet.Dimension.End.Column][$"C{rowNumber}"].Text;
                    data.To = workSheet.Cells[rowNumber, 6, rowNumber, workSheet.Dimension.End.Column][$"D{rowNumber}"].Text;
                    data.DateOfInLieu = workSheet.Cells[rowNumber, 6, rowNumber, workSheet.Dimension.End.Column][$"E{rowNumber}"].Text;*/
                    listData.Add(data);
                }
            }
            return listData;
        }

        public async Task<ResultDTO> UploadData(Stream stream)
        {
            var result = new ResultDTO();
            var data = new ArrayResultDTO();
            var errorSAPCode = new List<string>();
            List<CompanyInfoImportFileDTO> overtimeDetails = new List<CompanyInfoImportFileDTO>();
            var dataFromFile = ReadDataFromStream(stream);
            if (dataFromFile.Count > 0)
            {
                var groupSapcodes = dataFromFile.GroupBy(x => x.CompanyName);
                foreach (var group in groupSapcodes)
                {
                    foreach (var item in group)
                    {
                        if (string.IsNullOrWhiteSpace(item.CompanyName))
                        {
                            result.ErrorCodes.Add(1006);
                            result.Messages.Add(item.LineOfExcel);
                        }
                        else
                        {
                            
                        }
                    }
                }
                data.Data = overtimeDetails;
                data.Count = overtimeDetails.Count;
            }
            else
            {
                result.ErrorCodes.Add(1001);
                result.Messages.Add("OVERTIME_APPLICATION_VALIDATE_IMPORT_FILE");
            }
            result.Object = data;
            return result;
        }


    }
}