using AutoMapper;
using Core.Repositories.Business.IRepositories;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWork.Business.ViewModel;
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
    public class AccountingBalanceSheetBusiness : IAccountingBalanceSheetBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly AutoMapper.IMapper _mapper;
        public AccountingBalanceSheetBusiness(IUnitOfWork uow, AutoMapper.IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<ResultDTO> GetList(QueryArgs args)
        {
            var listAllData = await _uow.GetRepository<FinancialAccount>().FindByAsync<FinancialAccountDTO>(x => true);
            var listData = await _uow.GetRepository<FinancialAccount>().FindByAsync<FinancialAccountDTO>(args.Predicate, args.PredicateParameters);
            List<FinancialAccountDTO> tree = new List<FinancialAccountDTO>();
            List<FinancialAccountDTO> vmLst = listData.OrderByDescending(x => x.Created).ToList();
            var highestDepartment = listData.Where(x => x.ParentFinanceAccountId == x.Id).FirstOrDefault();
            if (highestDepartment != null)
            {
                foreach (var x in vmLst) await GetChild(x.Id, x);
                tree = vmLst.Where(item => item.Id == highestDepartment.Id).ToList();
                tree.AddRange(vmLst.Where(item => !item.ParentFinanceAccountId.HasValue).ToList());
                ResultDTO result = new ResultDTO
                {
                    Object = new ArrayResultDTO
                    {
                        Data = tree,
                        Count = 1,
                    },
                };
                return result;
            }
            else
            {
                foreach(var x in vmLst) await GetChild(x.Id, x);
                tree = vmLst.Where(item => !item.ParentFinanceAccountId.HasValue).ToList();
                if (!tree.Any()) tree = vmLst;
                ResultDTO result = new ResultDTO
                {
                    Object = new ArrayResultDTO
                    {
                        Data = tree,
                        Count = 1,
                    },
                };
                return result;
            }
        }

        protected async Task GetChild(Guid Id, FinancialAccountDTO child)
        {
            var parentAccount = await _uow.GetRepository<FinancialAccount>().FindByAsync<FinancialAccountDTO>(x => x.ParentFinanceAccountId != null && x.ParentFinanceAccountId == Id);
            if (parentAccount != null)
            {
                child.Items = parentAccount.ToList();
                foreach (var acc in parentAccount)
                {
                    await GetChild(acc.Id, acc);
                };
            }
        } 

        public async Task<ResultDTO> UpSert(FinancialAccountArgs args)
        {
            var resultDTO = new ResultDTO();

            if (args is null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            if (string.IsNullOrEmpty(args.AccountName))
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "NAME_IS_REQUIRE" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            
            if (args.Id  != null && args.Id != Guid.Empty)
            {
                var exists = _uow.GetRepository<FinancialAccount>().GetSingle(x => x.Id == args.Id);
                if (exists == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_ITEM }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                var validateAccountNo = _uow.GetRepository<FinancialAccount>().GetSingle(x => x.Id != args.Id && x.AccountNo.Equals(args.AccountNo));
                if (validateAccountNo != null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.FINANCIAL.ACCOUNT_NO_IS_ALREADY_EXIST }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                exists.AccountNo = args.AccountNo;
                exists.AccountName = args.AccountName;
                exists.Description = args.Description;
                var updated = _uow.GetRepository<FinancialAccount>().Update(exists);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { MessageConst.UPDATE_SUCCESSFULLY },
                    Object = _mapper.Map<FinancialAccountDTO>(updated)
                };
                goto Finish;
            }
            else
            {
                var validateAccountNo = _uow.GetRepository<FinancialAccount>().GetSingle(x => x.AccountNo.Equals(args.AccountNo));
                if (validateAccountNo != null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.FINANCIAL.ACCOUNT_NO_IS_ALREADY_EXIST }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                var mapData = _mapper.Map<FinancialAccount>(args);
                var addNew = _uow.GetRepository<FinancialAccount>().Add(mapData);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { MessageConst.CREATED_SUCCESSFULLY },
                    Object = _mapper.Map<FinancialAccountDTO>(addNew)
                };
                goto Finish;
            }
        Finish:
            return resultDTO;
        }
        public async Task<ResultDTO> DeleteById(Guid id)
        {
            var resultDTO = new ResultDTO();
            var currentItem = await _uow.GetRepository<FinancialAccount>().GetSingleAsync(x => x.Id == id);
            #region Verify
            if (currentItem == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_ITEM }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            #endregion
            _uow.GetRepository<FinancialAccount>().Delete(currentItem);
            await _uow.CommitAsync();
            resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.DELETE_SUCCESSFULLY } };
        Finish:
            return resultDTO;
        }

        public async Task<ResultDTO> GetById(Guid id)
        {
            var resultDTO = new ResultDTO();
            var findItem = await _uow.GetRepository<FinancialAccount>().GetSingleAsync<FinancialAccountDTO>(x => x.Id == id);
            if (findItem == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_ITEM }, ErrorCodes = new List<int> { -1 } };
            }
            await GetChild(findItem.Id, findItem);
            resultDTO.Object = findItem;
            return resultDTO;
        }
    }
}