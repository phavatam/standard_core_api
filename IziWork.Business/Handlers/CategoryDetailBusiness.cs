using AutoMapper;
using Core.Repositories.Business.IRepositories;
using IziWork.Business.Args;

using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWork.Common.Args;
using IziWork.Common.Constans;
using IziWork.Common.DTO;
using IziWork.Data.Entities;
using Mapster;
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
using System.Threading.Tasks;

namespace IziWork.Business.Handlers
{
    public class CategoryDetailBusiness : ICategoryDetailBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CategoryDetailBusiness(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResultDTO> GetListCategory(QueryArgs args)
        {
            var list = await _uow.GetRepository<Category>().FindByAsync<CategoryDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<Category>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = list,
                    Count = totalList
                }
            };
        }

        #region INSERT - UPDATE - DELETE
        public async Task<ResultDTO> CreateOrUpdate(CategoryDetailArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }

            var findCategory = _uow.GetRepository<Category>().GetSingle(x => x.Type == (int)args.Type);
            if (findCategory == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CATEGORYDETAIL.NOT_FOUND_CATEGORY }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {
                var currentCategoryDetail = await _uow.GetRepository<CategoryDetail>().GetSingleAsync(y => y.Id.Equals(args.Id));
                if (currentCategoryDetail == null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { MessageConst.NOT_FOUND_ITEM } };
                }
                if (!string.IsNullOrEmpty(args.Name))
                {
                    currentCategoryDetail.Name = args.Name;
                }
                var categoryDetailUpdated = _uow.GetRepository<CategoryDetail>().Update(currentCategoryDetail);
                var mapperData = _mapper.Map<CategoryDetailDTO>(categoryDetailUpdated);
                mapperData.Type = (Common.Enums.DefineEnums.CATEGORY_TYPE) findCategory.Type;
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { MessageConst.UPDATE_SUCCESSFULLY },
                    Object = mapperData
                };
            }
            else
            {
                var data = _mapper.Map<CategoryDetail>(args);
                data.CategoryId = findCategory.Id;
                var dept = _uow.GetRepository<CategoryDetail>().Add(data);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { MessageConst.CREATED_SUCCESSFULLY },
                    Object = _mapper.Map<CategoryDetailDTO>(dept)
                };
            }

            return resultDTO;
        }
        public async Task<ResultDTO> DeleteById(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<CategoryDetail>().GetSingleAsync(y => y.Id.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { MessageConst.NOT_FOUND_ITEM } };
            }
            else
            {
                _uow.GetRepository<CategoryDetail>().Delete(existing);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { MessageConst.DELETE_SUCCESSFULLY },
                };
            }
            return resultDTO;
        }
        #endregion
        #region GET DATA
        public async Task<ResultDTO> GetListCategoryDetail(QueryArgs args)
        {
            var list = await _uow.GetRepository<CategoryDetail>().FindByAsync<CategoryDetailDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<CategoryDetail>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = list,
                    Count = totalList
                }
            };
        }


        public async Task<ResultDTO> GetById(Guid Id)
        {
            var findItem = await _uow.GetRepository<CategoryDetail>().GetSingleAsync<CategoryDetailDTO>(x => x.Id == Id);
            if (findItem == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_ITEM }, ErrorCodes = new List<int> { -1 } };
            }
            return new ResultDTO { Object = findItem };
        }
        #endregion
    }
}
