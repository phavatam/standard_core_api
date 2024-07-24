using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWork.Business.IRepositories;
using IziWork.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Handlers
{
    public class StatusBusiness : IStatusBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly AutoMapper.IMapper _mapper;
        public StatusBusiness(IUnitOfWork uow, IConfiguration configuration, AutoMapper.IMapper mapper)
        {
            _uow = uow;
            _configuration = configuration;
            _secretKey = (_configuration != null && _configuration["AppSettings:SecretKey"] != null) ? _configuration["AppSettings:SecretKey"].ToString() : "8qaa,AQ%UrhXY|#PRsb%!4qc8yCbh8n'Bsi{>;I7,%R#EhV@wn%+ni.g#g^h]rF~BQ_>:-F)+dC%!ST6K2";
            _mapper = mapper;
        }
        #region INSERT - UPDATE - DELETE
        public async Task<ResultDTO> CreateOrUpdate(StatusArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
            }
            
            if (args.Id != Guid.Empty)
            {
                //update
                var existing = await _uow.GetRepository<Status>().GetSingleAsync(y => y.Code.Equals(args.Code) && !y.Id.Equals(args.Id));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "STATUSCODE_IS_EXIST" } };
                }
                else
                {
                    var current = await _uow.GetRepository<Status>().GetSingleAsync(y => y.Id.Equals(args.Id));
                    if (current == null)
                    {
                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "STATUS_NOT_EXIST" } };
                    }

                    if (!string.IsNullOrEmpty(args.Code))
                    {
                        current.Code = args.Code;
                    }
                    if (!string.IsNullOrEmpty(args.Name))
                    {
                        current.Name = args.Name;
                    }
                    current.IsWorkflow = args.IsWorkflow;
                    /*current.Type = args.Type;*/
                    current.IsActive = args.IsActive;
                    var updated = _uow.GetRepository<Status>().Update(current);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "UPDATE_STATUS_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<StatusDTO>(current)
                    };
                }
            }
            else
            {
                var existing = await _uow.GetRepository<Status>().GetSingleAsync(y => y.Code.Equals(args.Code));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "STATUSCODE_IS_EXIST" } };
                }
                else
                {
                    var data = _mapper.Map<Status>(args);
                    var status = _uow.GetRepository<Status>().Add(data);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "CREATE_STATUS_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<StatusDTO>(status)
                    };
                }
            }
            return resultDTO;
        }
        public async Task<ResultDTO> DeleteStatusById(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<Status>().GetSingleAsync(y => y.Id.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "STATUS_NOT_EXIST" } };
            }
            _uow.GetRepository<Status>().Delete(existing);
            await _uow.CommitAsync();
            resultDTO = new ResultDTO()
            {
                Messages = new List<string> { "DELETE_STATUS_IS_SUCCESSFULLY" },
            };
            return resultDTO;
        }
        #endregion
        #region GET DATA
        public async Task<ResultDTO> GetListStatus(QueryArgs args)
        {
            var list = await _uow.GetRepository<Status>().FindByAsync<StatusDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<Status>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = list,
                    Count = totalList
                }
            };
        }

        public async Task<ResultDTO> GetStatusById(Guid Id)
        {
            var existStatus = await _uow.GetRepository<Status>().GetSingleAsync<StatusDTO>(x => x.Id == Id);
            if (existStatus == null)
            {
                return new ResultDTO() { Messages = new List<string> { "STATUS_IS_NOT_EXIST" }, ErrorCodes = new List<int> { -1 } };
            }
            return new ResultDTO { Object = existStatus };
        }
        #endregion
    }
}
