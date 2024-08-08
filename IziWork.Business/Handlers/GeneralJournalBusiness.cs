using IziWork.Business.Args;

using IziWork.Business.DTO;
using IziWork.Business.Interfaces;

using IziWork.Data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repositories.Business.IRepositories;
using IziWork.Common.Constans;
using IziWork.Common.Args;
using IziWork.Common.DTO;

namespace IziWork.Business.Handlers
{
    public class GeneralJournalBusiness : IGeneralJournalBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly AutoMapper.IMapper _mapper;
        public GeneralJournalBusiness(IUnitOfWork uow, IConfiguration configuration, AutoMapper.IMapper mapper)
        {
            _uow = uow;
            _configuration = configuration;
            _secretKey = (_configuration != null && _configuration["AppSettings:SecretKey"] != null) ? _configuration["AppSettings:SecretKey"].ToString() : "8qaa,AQ%UrhXY|#PRsb%!4qc8yCbh8n'Bsi{>;I7,%R#EhV@wn%+ni.g#g^h]rF~BQ_>:-F)+dC%!ST6K2";
            _mapper = mapper;
        }
        public async Task<ResultDTO> GetListGeneralJournal(QueryArgs args)
        {
            var list = await _uow.GetRepository<GeneralJournal>().FindByAsync<GeneralJournalDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var count = await _uow.GetRepository<GeneralJournal>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = list,
                    Count = count
                }
            };
        }

        public async Task<ResultDTO> CreateOrUpdateGeneralJournal(GeneralJournalArgs args)
        {
            var dataReturn = new ResultDTO();
            if (args == null)
            {
                dataReturn.Messages = new List<string>() { MessageConst.NOT_FOUND_PARAM };
                dataReturn.ErrorCodes = new List<int>() { -1 };
                goto Finish;
            }

            if (args.Id != null && args.Id.Value != Guid.Empty)
            {
                var findGeneralJournal = await _uow.GetRepository<GeneralJournal>().FindByIdAsync(args.Id.Value);
                if (findGeneralJournal == null)
                {
                    dataReturn.Messages = new List<string>() { MessageConst.NOT_FOUND_ITEM };
                    dataReturn.ErrorCodes = new List<int>() { -1 };
                    goto Finish;
                }
                if (args.CompanyId != Guid.Empty)
                {
                    var findCompany = _uow.GetRepository<CompanyInfo>().FindById(args.CompanyId);
                    if (findCompany == null)
                    {
                        dataReturn.Messages = new List<string>() { MessageConst.NOT_FOUND_COMPANY };
                        dataReturn.ErrorCodes = new List<int>() { -1 };
                        goto Finish;
                    }
                    findGeneralJournal.CompanyId = findCompany.Id;
                }
                if (!string.IsNullOrEmpty(args.Year))
                {
                    findGeneralJournal.Year = args.Year;
                }

                var updatedGeneralJournal = _uow.GetRepository<GeneralJournal>().Update(findGeneralJournal);
                if (args.GeneralJournalDetails != null && args.GeneralJournalDetails.Any())
                {
                    var oldGeneralJounalDetail = _uow.GetRepository<GeneralJournalDetail>().FindBy(x => x.GeneralJournalId == updatedGeneralJournal.Id);
                    if (oldGeneralJounalDetail != null)
                    {
                        _uow.GetRepository<GeneralJournalDetail>().Delete(oldGeneralJounalDetail);
                    }
                    foreach(var itemGDD in args.GeneralJournalDetails)
                    {
                        if (itemGDD.DebitAccountId == Guid.Empty)
                        {
                            dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.DEBIT_ACCOUNT_IS_REQUIRED };
                            dataReturn.ErrorCodes = new List<int>() { -1 };
                            goto Finish;
                        }
                        var checkDebitAccount = _uow.GetRepository<FinancialAccount>().GetSingle(x => x.Id == itemGDD.DebitAccountId);
                        if (checkDebitAccount == null)
                        {
                            dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.DEBIT_ACCOUNT_IS_REQUIRED };
                            dataReturn.ErrorCodes = new List<int>() { -1 };
                            goto Finish;
                        }

                        if (itemGDD.CreditAccountId == Guid.Empty)
                        {
                            dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.CREDIT_ACCOUNT_IS_REQUIRED };
                            dataReturn.ErrorCodes = new List<int>() { -1 };
                            goto Finish;
                        }

                        var checkCredit = _uow.GetRepository<FinancialAccount>().GetSingle(x => x.Id == itemGDD.CreditAccountId);
                        if (checkCredit == null)
                        {
                            dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.CREDIT_ACCOUNT_IS_NOT_EXIST };
                            dataReturn.ErrorCodes = new List<int>() { -1 };
                            goto Finish;
                        }
                        var addNewGDD = _mapper.Map<GeneralJournalDetail>(itemGDD);
                        addNewGDD.GeneralJournalId = updatedGeneralJournal.Id;
                        _uow.GetRepository<GeneralJournalDetail>().Add(addNewGDD);
                    }
                }
                dataReturn.Object = _mapper.Map<GeneralJournalDTO>(updatedGeneralJournal);
                await _uow.CommitAsync();
                dataReturn.Messages = new List<string>() { MessageConst.UPDATE_SUCCESSFULLY };
            } else
            {
                if (args.CompanyId == Guid.Empty)
                {
                    dataReturn.Messages = new List<string>() { MessageConst.NOT_FOUND_COMPANY };
                    dataReturn.ErrorCodes = new List<int>() { -1 };
                    goto Finish;
                }

                var findCompany = _uow.GetRepository<CompanyInfo>().FindById(args.CompanyId);
                if (findCompany == null)
                {
                    dataReturn.Messages = new List<string>() { MessageConst.NOT_FOUND_COMPANY };
                    dataReturn.ErrorCodes = new List<int>() { -1 };
                    goto Finish;
                }
                if (string.IsNullOrEmpty(args.Year))
                {
                    dataReturn.Messages = new List<string>() { MessageConst.REQUIRED_YEAR };
                    dataReturn.ErrorCodes = new List<int>() { -1 };
                    goto Finish;
                }
                var mapGeneralJournal = new GeneralJournal() { };
                mapGeneralJournal.CompanyId = findCompany.Id;
                mapGeneralJournal.Year = args.Year;
                var addNewGD = _uow.GetRepository<GeneralJournal>().Add(mapGeneralJournal);
                if (args.GeneralJournalDetails != null && args.GeneralJournalDetails.Any())
                {
                    foreach (var itemGDD in args.GeneralJournalDetails)
                    {
                        if (itemGDD.DebitAccountId == Guid.Empty)
                        {
                            dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.DEBIT_ACCOUNT_IS_REQUIRED };
                            dataReturn.ErrorCodes = new List<int>() { -1 };
                            goto Finish;
                        }
                        var checkDebitAccount = _uow.GetRepository<FinancialAccount>().GetSingle(x => x.Id == itemGDD.DebitAccountId);
                        if (checkDebitAccount == null)
                        {
                            dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.DEBIT_ACCOUNT_IS_REQUIRED };
                            dataReturn.ErrorCodes = new List<int>() { -1 };
                            goto Finish;
                        }

                        if (itemGDD.CreditAccountId == Guid.Empty)
                        {
                            dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.CREDIT_ACCOUNT_IS_REQUIRED };
                            dataReturn.ErrorCodes = new List<int>() { -1 };
                            goto Finish;
                        }

                        var checkCredit = _uow.GetRepository<FinancialAccount>().GetSingle(x => x.Id == itemGDD.CreditAccountId);
                        if (checkCredit == null)
                        {
                            dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.CREDIT_ACCOUNT_IS_NOT_EXIST };
                            dataReturn.ErrorCodes = new List<int>() { -1 };
                            goto Finish;
                        }
                        var addNewGDD = _mapper.Map<GeneralJournalDetail>(itemGDD);
                        addNewGDD.GeneralJournalId = addNewGD.Id;
                        _uow.GetRepository<GeneralJournalDetail>().Add(addNewGDD);
                    }
                }
                dataReturn.Object = _mapper.Map<GeneralJournalDTO>(addNewGD);
                await _uow.CommitAsync();
            }

            Finish:
            return dataReturn;
        }

        public async Task<ResultDTO> CreateOrUpdateOneRowGeneralJournalDetail(GeneralJournalDetailArgs args)
        {
            var dataReturn = new ResultDTO();

            if (args == null)
            {
                dataReturn.Messages = new List<string>() { MessageConst.NOT_FOUND_PARAM };
                dataReturn.ErrorCodes = new List<int>() { -1 };
                goto Finish;
            }

            if (args.DebitAccountId == Guid.Empty)
            {
                dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.DEBIT_ACCOUNT_IS_REQUIRED };
                dataReturn.ErrorCodes = new List<int>() { -1 };
                goto Finish;
            }
            var checkDebitAccount = _uow.GetRepository<FinancialAccount>().GetSingle(x => x.Id == args.DebitAccountId);
            if (checkDebitAccount == null)
            {
                dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.DEBIT_ACCOUNT_IS_REQUIRED };
                dataReturn.ErrorCodes = new List<int>() { -1 };
                goto Finish;
            }

            if (args.CreditAccountId == Guid.Empty)
            {
                dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.CREDIT_ACCOUNT_IS_REQUIRED };
                dataReturn.ErrorCodes = new List<int>() { -1 };
                goto Finish;
            }

            var checkCredit = _uow.GetRepository<FinancialAccount>().GetSingle(x => x.Id == args.CreditAccountId);
            if (checkCredit == null)
            {
                dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.CREDIT_ACCOUNT_IS_NOT_EXIST };
                dataReturn.ErrorCodes = new List<int>() { -1 };
                goto Finish;
            }

            if (args.Amount == 0)
            {
                dataReturn.Messages = new List<string>() { MessageConst.FINANCIAL.AMOUNT_IS_REQUIRED };
                dataReturn.ErrorCodes = new List<int>() { -1 };
                goto Finish;
            }

            if (args.Id != null && args.Id.Value != Guid.Empty) {
                var findGeneralJournalDetail = await _uow.GetRepository<GeneralJournalDetail>().FindByIdAsync(args.Id.Value);
                if (findGeneralJournalDetail == null)
                {
                    dataReturn.Messages = new List<string>() { MessageConst.NOT_FOUND_ITEM };
                    dataReturn.ErrorCodes = new List<int>() { -1 };
                    goto Finish;
                }
                if (!string.IsNullOrEmpty(args.DocumentNo)) {
                    findGeneralJournalDetail.DocumentNo = args.DocumentNo;
                }
                if (args.DocumentDate != null && args.DocumentDate.HasValue)
                {
                    findGeneralJournalDetail.DocumentDate = args.DocumentDate.Value;
                }
                if (!string.IsNullOrEmpty(args.Description))
                {
                    findGeneralJournalDetail.Description = args.Description;
                }
                findGeneralJournalDetail.Amount = args.Amount;
                findGeneralJournalDetail.CreditAccountId = args.CreditAccountId;
                findGeneralJournalDetail.DebitAccountId = args.DebitAccountId;
                findGeneralJournalDetail.DateOfPeriod = args.DateOfPeriod;
                var updatedGDD = _uow.GetRepository<GeneralJournalDetail>().Update(findGeneralJournalDetail);
                dataReturn.Object = _mapper.Map<GeneralJournalDetailDTO>(updatedGDD);
                await _uow.CommitAsync();
                dataReturn.Messages = new List<string>() { MessageConst.UPDATE_SUCCESSFULLY };
            } else
            {
                var mapData = _mapper.Map<GeneralJournalDetail>(args);
                var addGDD = _uow.GetRepository<GeneralJournalDetail>().Add(mapData);
                dataReturn.Object = _mapper.Map<GeneralJournalDetailDTO>(addGDD);
                await _uow.CommitAsync();
                dataReturn.Messages = new List<string>() { MessageConst.CREATED_SUCCESSFULLY };
            }
        Finish:
            return dataReturn;
        }

        public async Task<ResultDTO> DeleteGeneralJournal(Guid Id)
        {
            var dataReturn = new ResultDTO();
            var findGeneralJournal = await _uow.GetRepository<GeneralJournal>().FindByIdAsync(Id);
            if (findGeneralJournal == null)
            {
                dataReturn.Messages = new List<string>() { MessageConst.NOT_FOUND_ITEM };
                dataReturn.ErrorCodes = new List<int>() { -1 };
                goto Finish;
            }
            _uow.GetRepository<GeneralJournal>().Delete(findGeneralJournal);
            await _uow.CommitAsync();
            dataReturn.Messages = new List<string>() { MessageConst.DELETE_SUCCESSFULLY };
        Finish:
            return dataReturn;
        }

        public async Task<ResultDTO> DeleteGeneralJournalDetail(Guid Id)
        {
            var dataReturn = new ResultDTO();
            var findGeneralJournalDetail = await _uow.GetRepository<GeneralJournalDetail>().FindByIdAsync(Id);
            if (findGeneralJournalDetail == null)
            {
                dataReturn.Messages = new List<string>() { MessageConst.NOT_FOUND_ITEM};
                dataReturn.ErrorCodes = new List<int>() { -1 };
                goto Finish;
            }
            _uow.GetRepository<GeneralJournalDetail>().Delete(findGeneralJournalDetail);
            await _uow.CommitAsync();
            dataReturn.Messages = new List<string>() { MessageConst.DELETE_SUCCESSFULLY };
        Finish:
            return dataReturn;
        }
    }
}
