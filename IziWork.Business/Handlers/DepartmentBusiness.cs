using AutoMapper;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWork.Business.IRepositories;
using IziWork.Business.ViewModel;
using IziWork.Data.Entities;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IziWork.Business.Handlers
{
    public class DepartmentBusiness : IDepartmentBusiness
    {

        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly AutoMapper.IMapper _mapper;
        public DepartmentBusiness(IUnitOfWork uow, IConfiguration configuration, AutoMapper.IMapper mapper)
        {
            _uow = uow;
            _configuration = configuration;
            _secretKey = (_configuration != null && _configuration["AppSettings:SecretKey"] != null) ? _configuration["AppSettings:SecretKey"].ToString() : "8qaa,AQ%UrhXY|#PRsb%!4qc8yCbh8n'Bsi{>;I7,%R#EhV@wn%+ni.g#g^h]rF~BQ_>:-F)+dC%!ST6K2";
            _mapper = mapper;
        }

        #region INSERT - UPDATE - DELETE
        public async Task<ResultDTO> CreateOrUpdate(DepartmentArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
            }
            if (string.IsNullOrEmpty(args.Code))
            {
                return new ResultDTO() { Messages = new List<string> { "CODE_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (string.IsNullOrEmpty(args.Name))
            {
                return new ResultDTO() { Messages = new List<string> { "NAME_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Type < 0)
            {
                return new ResultDTO() { Messages = new List<string> { "TYPE_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.UserDepartmentMappings != null && args.UserDepartmentMappings.Count > 0)
            {
                foreach (var mapping in args.UserDepartmentMappings)
                {
                    if (mapping.UserId == Guid.Empty)
                    {
                        return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IS_REQUIRE" } };
                    }
                    if (mapping.RoleIds != null && !mapping.RoleIds.Any())
                    {
                        return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "ROLE_IS_REQUIRE" } };
                    }
                    var findUser = _uow.GetRepository<User>().GetSingle(x => x.Id == mapping.UserId);
                    if (findUser == null)
                    {
                        return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IS_NOT_EXIST" } };
                    }

                    var findRole = _uow.GetRepository<Role>().FindBy(x => !mapping.RoleIds.Contains(x.Id));
                    if (findRole == null || !findRole.Any())
                    {
                        return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "ROLE_IS_NOT_EXIST" } };
                    }
                }

            }

            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {

                var existing = await _uow.GetRepository<Department>().GetSingleAsync(y => y.Code.Equals(args.Code) && !y.Id.Equals(args.Id));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "DEPARTMENT_EXISTED" } };
                }
                else
                {
                    var currentDept = await _uow.GetRepository<Department>().GetSingleAsync(y => y.Id.Equals(args.Id));
                    if (currentDept == null)
                    {
                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "DEPARTMENT_NOT_EXISTED" } };
                    }
                    if (args.UserDepartmentMappings != null && args.UserDepartmentMappings.Count > 0)
                    {
                        foreach (var mapping in args.UserDepartmentMappings)
                        {
                            var findExistUserInDepartment = await _uow.GetRepository<UserDepartmentMapping>().FindByAsync(x => x.Id != mapping.Id &&
                            x.DepartmentId == args.Id && x.UserId == mapping.UserId && x.UserDepartmentRoleMappings.Any(y => y.RoleId.HasValue && mapping.RoleIds.Contains(y.RoleId.Value)));
                            if (findExistUserInDepartment.Any())
                            {
                                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IN_DEPARTMENT_IS_EXIST" } };
                            }
                            if (mapping.IsDeleted != null && !mapping.IsDeleted.Value)
                            {
                                if (mapping.Id != null && mapping.Id.Value != Guid.Empty)
                                {
                                    var currentUserInDepartment = await _uow.GetRepository<UserDepartmentMapping>().GetSingleAsync(x => x.Id == mapping.Id);
                                    if (currentUserInDepartment == null)
                                    {
                                        return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IN_DEPARTMENT_NOT_EXISTS" } };
                                    }
                                    currentUserInDepartment.UserId = mapping.UserId;
                                    currentUserInDepartment.DepartmentId = mapping.DepartmentId.Value;
                                    _uow.GetRepository<UserDepartmentMapping>().Update(currentUserInDepartment);
                                    var findUserDepartmentRoleMapping = await _uow.GetRepository<UserDepartmentRoleMapping>().FindByAsync(x => x.UserDepartmentMappingId == currentUserInDepartment.Id);
                                    if (findUserDepartmentRoleMapping != null)
                                    {
                                        foreach (var role in findUserDepartmentRoleMapping)
                                        {
                                            _uow.GetRepository<UserDepartmentRoleMapping>().Delete(role);
                                        }
                                    }
                                    foreach (var roleId in mapping.RoleIds)
                                    {
                                        var newUserDepartmentRole = new UserDepartmentRoleMapping()
                                        {
                                            UserDepartmentMappingId = currentUserInDepartment.Id,
                                            RoleId = roleId,
                                        };
                                        _uow.GetRepository<UserDepartmentRoleMapping>().Add(newUserDepartmentRole);
                                    }
                                }
                                else
                                {
                                    var findUserInDepartment = new UserDepartmentMapping()
                                    {
                                        DepartmentId = currentDept.Id,
                                        UserId = mapping.UserId,
                                    };
                                    _uow.GetRepository<UserDepartmentMapping>().Add(findUserInDepartment);

                                    foreach (var roleId in mapping.RoleIds)
                                    {
                                        var newUserDepartmentRole = new UserDepartmentRoleMapping()
                                        {
                                            UserDepartmentMappingId = findUserInDepartment.Id,
                                            RoleId = roleId,
                                        };
                                        _uow.GetRepository<UserDepartmentRoleMapping>().Add(newUserDepartmentRole);
                                    }
                                }
                            }
                            else
                            {
                                if (mapping.Id != null && mapping.Id.Value != Guid.Empty)
                                {
                                    var currentUserInDepartment = await _uow.GetRepository<UserDepartmentMapping>().GetSingleAsync(x => x.Id == mapping.Id);
                                    if (currentUserInDepartment == null)
                                    {
                                        return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IN_DEPARTMENT_NOT_EXISTS" } };
                                    }
                                    var findUserDepartmentRoleMapping = await _uow.GetRepository<UserDepartmentRoleMapping>().FindByAsync(x => x.UserDepartmentMappingId == currentUserInDepartment.Id);
                                    if (findUserDepartmentRoleMapping != null)
                                    {
                                        foreach (var role in findUserDepartmentRoleMapping)
                                        {
                                            _uow.GetRepository<UserDepartmentRoleMapping>().Delete(role);
                                        }
                                    }
                                    _uow.GetRepository<UserDepartmentMapping>().Delete(currentUserInDepartment);
                                }
                            }
                        }
                    }
                    //update dept
                    if (!string.IsNullOrEmpty(args.Code))
                    {
                        currentDept.Code = args.Code;
                    }
                    if (!string.IsNullOrEmpty(args.Name))
                    {
                        currentDept.Name = args.Name;
                    }
                    if (!string.IsNullOrEmpty(args.Note))
                    {
                        currentDept.Note = args.Note;
                    }
                    currentDept.ProfileId = args.ProfileId;
                    currentDept.ParentId = args.ParentId;
                    var dept = _uow.GetRepository<Department>().Update(currentDept);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "UPDATE_DEPARTMENT_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<DepartmentDTO>(currentDept)
                    };
                }
            }
            else
            {
                var existing = await _uow.GetRepository<Department>().GetSingleAsync(y => y.Code.Equals(args.Code));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "CREATE_DEPARTMENT_IS_EXIST" } };
                }
                var dept = new Department();
                dept.Code = args.Code;
                dept.Name = args.Name;
                dept.Note = args.Note;
                dept.ProfileId = args.ProfileId;
                dept.ParentId = args.ParentId;
                dept.Type = (int) args.Type;
                _uow.GetRepository<Department>().Add(dept);
                if (dept != null)
                {
                    if (args.UserDepartmentMappings != null && args.UserDepartmentMappings.Count > 0)
                    {
                        foreach (var mapping in args.UserDepartmentMappings)
                        {
                            if (mapping.IsDeleted != null && !mapping.IsDeleted.Value)
                            {
                                var findUserInDepartment = new UserDepartmentMapping()
                                {
                                    DepartmentId = dept.Id,
                                    UserId = mapping.UserId,
                                };
                                _uow.GetRepository<UserDepartmentMapping>().Add(findUserInDepartment);

                                foreach (var roleId in mapping.RoleIds)
                                {
                                    var newUserDepartmentRole = new UserDepartmentRoleMapping()
                                    {
                                        UserDepartmentMappingId = findUserInDepartment.Id,
                                        RoleId = roleId,
                                    };
                                    _uow.GetRepository<UserDepartmentRoleMapping>().Add(newUserDepartmentRole);
                                }
                            }
                        }
                    }
                }
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "CREATE_DEPARTMENT_IS_SUCCESSFULLY" },
                    Object = _mapper.Map<DepartmentDTO>(dept)
                };
            }
            return resultDTO;
        }
        public async Task<ResultDTO> DeleteDepartment(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<Department>().GetSingleAsync(y => y.Id.Equals(Id));
            var listChild = await _uow.GetRepository<Department>().AnyAsync(y => !y.Id.Equals(Id) && y.ParentId.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "DEPARTMENT_NOT_EXIST" } };
            }
            else if (listChild)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "DEPARTMENT_HAS_CHILD_DEPT" } };
            }
            else
            {
                _uow.GetRepository<Department>().Delete(existing);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "DELETE_DEPARTMENT_IS_SUCCESSFULLY" },
                    Object = _mapper.Map<DepartmentDTO>(existing)
                };
            }
            return resultDTO;
        }
        #endregion

        #region GET DATA
        public async Task<ResultDTO> GetListDepartment(QueryArgs args)
        {
            var deptList = await _uow.GetRepository<Department>().FindByAsync<DepartmentDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = deptList.Count();
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = deptList,
                    Count = totalList
                }
            };
        }

        public async Task<ResultDTO> GetTreeDepartment(QueryArgs args)
        {
            var lstDepartment = await _uow.GetRepository<Department>().FindByAsync<DepartmentTreeViewModel>(args.Predicate, args.PredicateParameters);
            List<DepartmentTreeViewModel> departmentTree = new List<DepartmentTreeViewModel>();
            List<DepartmentTreeViewModel> vmLstDepartment = lstDepartment.OrderByDescending(x => x.Grade).ToList();
            var highestDepartment = lstDepartment.Where(x => x.ParentId == x.Id).FirstOrDefault();
            if (highestDepartment != null)
            {
                vmLstDepartment.ForEach(x =>
                {
                    x.Items = vmLstDepartment.Where(y => y.ParentId == x.Id && y.Id != x.Id).OrderByDescending(k => k.Name).ToList();
                }
                );

                departmentTree = vmLstDepartment.Where(item => item.Id == highestDepartment.Id).ToList();
                departmentTree.AddRange(vmLstDepartment.Where(item => !item.ParentId.HasValue).ToList());
                ResultDTO result = new ResultDTO
                {
                    Object = new ArrayResultDTO
                    {
                        Data = departmentTree,
                        Count = 1,
                    },
                };
                return result;
            }
            else
            {
                vmLstDepartment.ForEach(x =>
                x.Items = vmLstDepartment.Where(y => y.ParentId == x.Id).OrderByDescending(k => k.Name).ToList()

                );
                departmentTree = vmLstDepartment.Where(item => !item.ParentId.HasValue).ToList();
                if (!departmentTree.Any())
                {
                    departmentTree = vmLstDepartment;
                }
                ResultDTO result = new ResultDTO
                {
                    Object = new ArrayResultDTO
                    {
                        Data = departmentTree,
                        Count = 1,
                    },
                };
                return result;
            }
        }
        #endregion

        #region CRUD User In Department
        public async Task<ResultDTO> GetListUserByDepartmentId(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var findUserInDepartment = await _uow.GetRepository<UserDepartmentMapping>().FindByAsync<UserDepartmentMappingDTO>(x => x.DepartmentId == Id);
            if (findUserInDepartment != null)
            {
                foreach (var item in findUserInDepartment)
                {
                    var findRole = _uow.GetRepository<UserDepartmentRoleMapping>()
                                           .FindBy<UserDepartmentRoleMappingDTO>(x => x.UserDepartmentMappingId == item.Id)
                                           .ToList();
                    item.UserDepartmentRoleMappings = findRole;
                }
                resultDTO.Object = findUserInDepartment;
            }
            return resultDTO;
        }

        private void ValidateCRUUserInDepartment(UserInDepartmentArgs args, out ResultDTO resultDTO)
        {
            resultDTO = new ResultDTO() { };
            if (args == null)
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "PAYLOAD_IS_REQUIRE" } };
            }

            if (args.DepartmentId == Guid.Empty)
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "DEPARTMENT_IS_REQUIRE" } };
            }

            if (args.UserId == Guid.Empty)
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IS_REQUIRE" } };
            }
            if (args.RoleIds != null && !args.RoleIds.Any())
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "ROLE_IS_REQUIRE" } };
            }
            var findDepartment = _uow.GetRepository<Department>().GetSingle(x => x.Id == args.DepartmentId);
            if (findDepartment == null)
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "DEPARTMENT_IS_NOT_EXIST" } };
            }

            var findUser = _uow.GetRepository<User>().GetSingle(x => x.Id == args.UserId);
            if (findUser == null)
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IS_NOT_EXIST" } };
            }

            var findRole = _uow.GetRepository<Role>().FindBy(x => !args.RoleIds.Contains(x.Id));
            if (findRole == null || !findRole.Any())
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "ROLE_IS_NOT_EXIST" } };
            }
        }

        public async Task<ResultDTO> CreateUserInDepartment(UserInDepartmentArgs args)
        {
            var resultDTO = new ResultDTO() { };
            this.ValidateCRUUserInDepartment(args, out resultDTO);
            if (resultDTO.Messages.Any())
            {
                goto Finish;
            }

            var findExistUserInDepartment = await _uow.GetRepository<UserDepartmentMapping>().FindByAsync(x =>
            x.DepartmentId == args.DepartmentId && x.UserId == args.UserId && x.UserDepartmentRoleMappings.Any(y => y.RoleId.HasValue && args.RoleIds.Contains(y.RoleId.Value)));
            if (findExistUserInDepartment.Any())
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IN_DEPARTMENT_IS_EXIST" } };
                goto Finish;
            }

            var findUserInDepartment = await _uow.GetRepository<UserDepartmentMapping>().GetSingleAsync(x =>
            x.DepartmentId == args.DepartmentId && x.UserId == args.UserId);
            if (findUserInDepartment == null)
            {
                findUserInDepartment = new UserDepartmentMapping()
                {
                    DepartmentId = args.DepartmentId.Value,
                    UserId = args.UserId,
                };
                _uow.GetRepository<UserDepartmentMapping>().Add(findUserInDepartment);
            }

            foreach (var roleId in args.RoleIds)
            {
                var newUserDepartmentRole = new UserDepartmentRoleMapping()
                {
                    UserDepartmentMappingId = findUserInDepartment.Id,
                    RoleId = roleId,
                };
                _uow.GetRepository<UserDepartmentRoleMapping>().Add(newUserDepartmentRole);
            }

            await _uow.CommitAsync();

            resultDTO = new ResultDTO()
            {
                Messages = new List<string>() { "CREATE_USER_IN_DEPARTMENT_IS_SUCCESSFULLY" },
                Object = _mapper.Map<UserDepartmentMappingDTO>(findUserInDepartment)
            };
        Finish:
            return resultDTO;
        }

        public async Task<ResultDTO> UpdateUserInDepartment(UserInDepartmentArgs args)
        {
            var resultDTO = new ResultDTO();
            this.ValidateCRUUserInDepartment(args, out resultDTO);
            if (resultDTO.Messages.Any())
            {
                goto Finish;
            }

            if (args.Id == Guid.Empty)
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "ID_IS_REQUIRE" } };
                goto Finish;
            }

            var currentUserInDepartment = await _uow.GetRepository<UserDepartmentMapping>().GetSingleAsync(x => x.Id == args.Id);
            if (currentUserInDepartment == null)
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IN_DEPARTMENT_NOT_EXISTS" } };
            }

            var findExistUserInDepartment = await _uow.GetRepository<UserDepartmentMapping>().FindByAsync(x => x.Id != args.Id &&
            x.DepartmentId == args.DepartmentId && x.UserId == args.UserId && x.UserDepartmentRoleMappings.Any(y => y.RoleId.HasValue && args.RoleIds.Contains(y.RoleId.Value)));
            if (findExistUserInDepartment.Any())
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IN_DEPARTMENT_IS_EXIST" } };
                goto Finish;
            }

            currentUserInDepartment.UserId = args.UserId;
            currentUserInDepartment.DepartmentId = args.DepartmentId.Value;
            _uow.GetRepository<UserDepartmentMapping>().Update(currentUserInDepartment);
            var findUserDepartmentRoleMapping = await _uow.GetRepository<UserDepartmentRoleMapping>().FindByAsync(x => x.UserDepartmentMappingId == currentUserInDepartment.Id);
            if (findUserDepartmentRoleMapping != null)
            {
                foreach (var role in findUserDepartmentRoleMapping)
                {
                    _uow.GetRepository<UserDepartmentRoleMapping>().Delete(role);
                }
            }
            foreach (var roleId in args.RoleIds)
            {
                var newUserDepartmentRole = new UserDepartmentRoleMapping()
                {
                    UserDepartmentMappingId = currentUserInDepartment.Id,
                    RoleId = roleId,
                };
                _uow.GetRepository<UserDepartmentRoleMapping>().Add(newUserDepartmentRole);
            }
            await _uow.CommitAsync();
            resultDTO = new ResultDTO()
            {
                Messages = new List<string>() { "UPATE_USER_IN_DEPARTMENT_IS_SUCCESSFULLY" },
                Object = _mapper.Map<UserDepartmentMappingDTO>(currentUserInDepartment)
            };
        Finish:
            return resultDTO;
        }

        public async Task<ResultDTO> DeleteUserInDepartment(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var findUserDepartmentMapping = await _uow.GetRepository<UserDepartmentMapping>().GetSingleAsync(x => x.Id == Id);
            if (findUserDepartmentMapping == null)
            {
                resultDTO = new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IN_DEPARTMENT_NOT_EXISTS" } };
                goto Finish;
            }
            _uow.GetRepository<UserDepartmentMapping>().Delete(findUserDepartmentMapping);

            var findUserDepartmentRoleMapping = await _uow.GetRepository<UserDepartmentRoleMapping>().GetSingleAsync(x => x.UserDepartmentMappingId == Id);
            if (findUserDepartmentRoleMapping != null)
            {
                _uow.GetRepository<UserDepartmentRoleMapping>().Delete(findUserDepartmentRoleMapping);
            }
            await _uow.CommitAsync();
            resultDTO = new ResultDTO()
            {
                Messages = new List<string>() { "DELETE_USER_IN_DEPARTMENT_IS_SUCCESSFULLY" },
                Object = true
            };
        Finish:
            return resultDTO;
        }
        #endregion
    }
}
