using AutoMapper;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWork.Business.IRepositories;
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
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly AutoMapper.IMapper _mapper;
        public RoleBusiness(IUnitOfWork uow, AutoMapper.IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<ResultDTO> GetListRoles(QueryArgs args)
        {
            var userList = await _uow.GetRepository<Role>().FindByAsync<RoleDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<Role>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = userList,
                    Count = totalList
                }
            };
        }
        public async Task<ResultDTO> UpSertRole(RoleArgs args)
        {
            var resultDTO = new ResultDTO();

            if (args is null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            if (string.IsNullOrEmpty(args.Name))
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "NAME_IS_REQUIRE" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            if (string.IsNullOrEmpty(args.Code))
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "CODE_IS_REQUIRE" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {

                var checkExistUser = _uow.GetRepository<Role>().GetSingle(x => x.Name.ToLower().Equals(args.Name.ToLower()) && x.Id != args.Id);
                if (checkExistUser != null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { "ROLE_IS_EXIST" }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }

                var existsRole = _uow.GetRepository<Role>().GetSingle(x => x.Id == args.Id);
                if (existsRole == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_CURRENT_ROLE" }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                existsRole.Name = args.Name;
                existsRole.Code = args.Code;
                existsRole.IsActivated = args.IsActivated;
                _uow.GetRepository<Role>().Update(existsRole);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "UPDATE_ROLE_IS_SUCCESSFULLY" },
                    Object = _mapper.Map<RoleDTO>(existsRole)
                };
                goto Finish;
            }
            else
            {

                var checkExistUser = _uow.GetRepository<Role>().GetSingle(x => x.Name.ToLower().Equals(args.Name.ToLower()));
                if (checkExistUser != null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { "ROLE_IS_EXIST" }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                var newRole = new Role() { };
                newRole = _mapper.Map<Role>(args);
                _uow.GetRepository<Role>().Add(newRole);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "CREATE_ROLE_IS_SUCCESSFULLY" },
                    Object = _mapper.Map<RoleDTO>(newRole)
                };
                goto Finish;
            }
        Finish:
            return resultDTO;
        }
        public async Task<ResultDTO> DeleteRoleById(Guid roleId)
        {
            var resultDTO = new ResultDTO();
            var currentRole = await _uow.GetRepository<Role>().GetSingleAsync(x => x.Id == (roleId));
            #region Verify user 
            if (currentRole == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "ROLE_IS_NOT_EXIST" }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }
            #endregion
            /*currentUser.IsDeleted = true;*/
            _uow.GetRepository<Role>().Delete(currentRole);
            await _uow.CommitAsync();
            resultDTO = new ResultDTO() { Messages = new List<string> { "DELETE_ROLE_IS_SUCCESSFULLY" } };
        Finish:
            return resultDTO;
        }

        public async Task<ResultDTO> GetRoleById(Guid roleId)
        {
            var resultDTO = new ResultDTO();

            var currentUser = await _uow.GetRepository<Role>().GetSingleAsync<RoleDTO>(x => x.Id == roleId);
            #region Verify user 
            if (currentUser == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { "ACCOUNT_IS_NOT_EXIST" }, ErrorCodes = new List<int> { -1 } };
            }
            resultDTO.Object = currentUser;
            #endregion
            return resultDTO;
        }
    }
}