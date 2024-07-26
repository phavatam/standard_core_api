using AutoMapper;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;

using IziWork.Business.ViewModel;
using IziWork.Data.Entities;
using Mapster;
using Microsoft.Extensions.Configuration;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Core.Repositories.Business.IRepositories;
using IziWork.Common.DTO;
using IziWork.Common.Args;

namespace IziWork.Business.Handlers
{
    public class MenuBusiness : IMenuBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly AutoMapper.IMapper _mapper;
        public MenuBusiness(IUnitOfWork uow, IConfiguration configuration, AutoMapper.IMapper mapper)
        {
            _uow = uow;
            _configuration = configuration;
            _secretKey = (_configuration != null && _configuration["AppSettings:SecretKey"] != null) ? _configuration["AppSettings:SecretKey"].ToString() : "8qaa,AQ%UrhXY|#PRsb%!4qc8yCbh8n'Bsi{>;I7,%R#EhV@wn%+ni.g#g^h]rF~BQ_>:-F)+dC%!ST6K2";
            _mapper = mapper;
        }
        #region  INSERT - UPDATE - DELETE
        public async Task<ResultDTO> CreateOrUpdateMenu(MenuArgs args)
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
            if (string.IsNullOrEmpty(args.VnName))
            {
                return new ResultDTO() { Messages = new List<string> { "VNNAME_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.InverseParent != null)
            {
                foreach (var mappingChildVali in args.InverseParent)
                {
                    if (mappingChildVali.Id.Equals(args.Id))
                    {
                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "MENU_NOTIN_MENU_CURRENT" } };
                    }
                }
            }
            if (args.ParentId != null && args.ParentId.HasValue && args.ParentId.Value != Guid.Empty)
            {
                if (args.ParentId.Equals(args.Id))
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "MENU_NOTIN_MENU_CURRENT" } };
                }
            }
            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {
                //update
                var existing = await _uow.GetRepository<Menu>().GetSingleAsync(y => y.Code.Equals(args.Code) && !y.Id.Equals(args.Id));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "MENU_EXISTED" } };
                }
                if (args.MenuDepartmentMappings != null)
                {
                    foreach (var mappingDeptVali in args.MenuDepartmentMappings)
                    {
                        var existingRole = await _uow.GetRepository<MenuDepartmentMapping>().GetSingleAsync(y => y.MenuId.Equals(args.Id) && y.DepartmentId.Equals(mappingDeptVali.DepartmentId) && !y.Id.Equals(mappingDeptVali.Id));
                        if (existingRole != null)
                        {
                            return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "DEPARTMENT_IN_MENU_EXISTED" } };
                        }
                    }
                }
                if (args.MenuUserMappings != null)
                {
                    foreach (var mappingUserVali in args.MenuUserMappings)
                    {
                        var existingUser = await _uow.GetRepository<MenuUserMapping>().GetSingleAsync(y => y.MenuId.Equals(args.Id) && y.UserId.Equals(mappingUserVali.UserId) && !y.Id.Equals(mappingUserVali.Id));
                        if (existingUser != null)
                        {
                            return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "USER_IN_MENU_EXISTED" } };
                        }

                    }
                }
                if (args.MenuRoleMappings != null)
                {
                    foreach (var mappingRoleVali in args.MenuRoleMappings)
                    {
                        var existingRole = await _uow.GetRepository<MenuRoleMapping>().GetSingleAsync(y => y.MenuId.Equals(args.Id) && y.RoleId.Equals(mappingRoleVali.RoleId) && !y.Id.Equals(mappingRoleVali.Id));
                        if (existingRole != null)
                        {
                            return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "ROLE_IN_MENU_EXISTED" } };
                        }
                    }
                }
                if (args.InverseParent != null)
                {
                    foreach (var mappingChildVali in args.InverseParent)
                    {
                        var existingRole = await _uow.GetRepository<Menu>().GetSingleAsync(y => y.ParentId.Equals(args.Id) && y.Id.Equals(mappingChildVali.Id));
                        if (existingRole != null)
                        {
                            return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "MENU_IN_MENU_EXISTED" } };
                        }
                    }
                }
                var currentMeta = await _uow.GetRepository<Menu>().GetSingleAsync(y => y.Id.Equals(args.Id));
                if (currentMeta == null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "MENU_NOT_EXIST" } };
                }
                if (!string.IsNullOrEmpty(args.Code))
                {
                    currentMeta.Code = args.Code;
                }
                if (!string.IsNullOrEmpty(args.Name))
                {
                    currentMeta.Name = args.Name;
                }
                if (args.GroupId != Guid.Empty)
                {
                    currentMeta.GroupId = args.GroupId;
                }
                if (args.ParentId != Guid.Empty)
                {
                    currentMeta.ParentId = args.ParentId;
                }
                if (!string.IsNullOrEmpty(args.Url))
                {
                    currentMeta.Url = args.Url;
                }
                if (!string.IsNullOrEmpty(args.IconUrl))
                {
                    currentMeta.IconUrl = args.IconUrl;
                }
                if (!string.IsNullOrEmpty(args.VnName))
                {
                    currentMeta.VnName = args.VnName;
                }
                var menu = _uow.GetRepository<Menu>().Update(currentMeta);
                if (menu != null)
                {
                    if (args.MenuDepartmentMappings != null)
                    {
                        foreach (var mappingDept in args.MenuDepartmentMappings)
                        {
                            if (mappingDept.IsDeleted != null && !mappingDept.IsDeleted.Value)
                            {

                                if (mappingDept.Id != null && mappingDept.Id.Value != Guid.Empty)
                                {
                                    MenuDepartmentMapping dept = await _uow.GetRepository<MenuDepartmentMapping>().GetSingleAsync(y => y.Id.Equals(mappingDept.Id.Value));
                                    dept.DepartmentId = mappingDept.DepartmentId;
                                    _uow.GetRepository<MenuDepartmentMapping>().Update(dept);
                                }
                                else
                                {
                                    MenuDepartmentMapping dept = new MenuDepartmentMapping();
                                    dept.MenuId = menu.Id;
                                    dept.DepartmentId = mappingDept.DepartmentId;
                                    _uow.GetRepository<MenuDepartmentMapping>().Add(dept);
                                }
                            }
                            else
                            {
                                if (mappingDept.Id != null && mappingDept.Id != Guid.Empty)
                                {
                                    var dataDelete = await _uow.GetRepository<MenuDepartmentMapping>().GetSingleAsync(y => y.Id.Equals(mappingDept.Id.Value));
                                    _uow.GetRepository<MenuDepartmentMapping>().Delete(dataDelete);

                                }

                            }
                        }
                    }
                    if (args.MenuUserMappings != null)
                    {
                        foreach (var mappingUser in args.MenuUserMappings)
                        {
                            if (mappingUser.IsDeleted != null && !mappingUser.IsDeleted.Value)
                            {
                                if (mappingUser.Id != null && mappingUser.Id != Guid.Empty)
                                {
                                    MenuUserMapping user = await _uow.GetRepository<MenuUserMapping>().GetSingleAsync(y => y.Id.Equals(mappingUser.Id.Value));
                                    user.UserId = mappingUser.UserId;
                                    _uow.GetRepository<MenuUserMapping>().Update(user);
                                }
                                else
                                {
                                    MenuUserMapping user = new MenuUserMapping();
                                    user.MenuId = menu.Id;
                                    user.UserId = mappingUser.UserId;
                                    _uow.GetRepository<MenuUserMapping>().Add(user);
                                }
                            }
                            else
                            {
                                if (mappingUser.Id != null && mappingUser.Id != Guid.Empty)
                                {
                                    var dataDelete2 = await _uow.GetRepository<MenuUserMapping>().GetSingleAsync(y => y.Id.Equals(mappingUser.Id));
                                    _uow.GetRepository<MenuUserMapping>().Delete(dataDelete2);
                                }

                            }
                        }

                    }
                    if (args.MenuRoleMappings != null)
                    {
                        foreach (var mappingRole in args.MenuRoleMappings)
                        {
                            if (mappingRole.IsDeleted != null && !mappingRole.IsDeleted.Value)
                            {

                                if (mappingRole.Id != null && mappingRole.Id != Guid.Empty)
                                {
                                    MenuRoleMapping role = await _uow.GetRepository<MenuRoleMapping>().GetSingleAsync(y => y.Id.Equals(mappingRole.Id.Value));
                                    role.RoleId = mappingRole.RoleId;
                                    _uow.GetRepository<MenuRoleMapping>().Update(role);
                                }
                                else
                                {
                                    MenuRoleMapping role = new MenuRoleMapping();
                                    role.MenuId = menu.Id;
                                    role.RoleId = mappingRole.RoleId;
                                    _uow.GetRepository<MenuRoleMapping>().Add(role);
                                }
                            }
                            else
                            {
                                if (mappingRole.Id != null && mappingRole.Id != Guid.Empty)
                                {
                                    var dataDelete3 = await _uow.GetRepository<MenuRoleMapping>().GetSingleAsync(y => y.Id.Equals(mappingRole.Id));
                                    _uow.GetRepository<MenuRoleMapping>().Delete(dataDelete3);
                                }

                            }
                        }

                    }
                    if (args.InverseParent != null)
                    {
                        foreach (var mappingChild in args.InverseParent)
                        {
                            if (mappingChild.IsDeleted == null || (mappingChild.IsDeleted != null && !mappingChild.IsDeleted.Value))
                            {

                                if (mappingChild.Id != null && mappingChild.Id != Guid.Empty)
                                {
                                    Menu child = await _uow.GetRepository<Menu>().GetSingleAsync(y => y.Id.Equals(mappingChild.Id.Value));
                                    child.ParentId = args.Id;
                                    _uow.GetRepository<Menu>().Update(child);
                                }
                            }
                            else
                            {
                                if (mappingChild.Id != null && mappingChild.Id != Guid.Empty)
                                {
                                    Menu child = await _uow.GetRepository<Menu>().GetSingleAsync(y => y.Id.Equals(mappingChild.Id.Value));
                                    child.ParentId = null;
                                    _uow.GetRepository<Menu>().Update(child);
                                }

                            }
                        }

                    }
                }
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "UPDATE_MENU_IS_SUCCESSFULLY" },
                    Object = _mapper.Map<MenuDTO>(menu)
                };

            }
            else
            {
                //insert
                var existing = await _uow.GetRepository<Menu>().GetSingleAsync(y => y.Code.Equals(args.Code));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "CREATE_MENU_IS_EXIST" } };
                }
                else
                {
                    Menu newMenu = new Menu();
                    newMenu.Code = args.Code;
                    newMenu.Name = args.Name;
                    newMenu.Location = args.Location;
                    newMenu.Url = args.Url;
                    newMenu.IconUrl = args.IconUrl;
                    newMenu.GroupId = args.GroupId;
                    newMenu.ParentId = args.ParentId;
                    newMenu.VnName = args.VnName;
                    var menu = _uow.GetRepository<Menu>().Add(newMenu);
                    if (menu != null)
                    {
                        if (args.MenuDepartmentMappings != null)
                        {
                            foreach (var mappingDeptCre in args.MenuDepartmentMappings)
                            {
                                MenuDepartmentMapping dept = new MenuDepartmentMapping();
                                dept.MenuId = menu.Id;
                                dept.DepartmentId = mappingDeptCre.DepartmentId;
                                var menuDept = _uow.GetRepository<MenuDepartmentMapping>().Add(dept);

                            }
                        }
                        if (args.MenuUserMappings != null)
                        {
                            foreach (var mappingUserCre in args.MenuUserMappings)
                            {
                                MenuUserMapping user = new MenuUserMapping();
                                user.MenuId = menu.Id;
                                user.UserId = mappingUserCre.UserId;
                                user.UserId = mappingUserCre.UserId;
                                var menuDept = _uow.GetRepository<MenuUserMapping>().Add(user);

                            }
                        }
                        if (args.MenuRoleMappings != null)
                        {
                            foreach (var mappingRoleCre in args.MenuRoleMappings)
                            {
                                MenuRoleMapping role = new MenuRoleMapping();
                                role.MenuId = menu.Id;
                                role.RoleId = mappingRoleCre.RoleId;
                                var menuDept = _uow.GetRepository<MenuRoleMapping>().Add(role);
                            }
                        }
                        if (args.InverseParent != null)
                        {
                            foreach (var mappingChild in args.InverseParent)
                            {
                                if (mappingChild.Id != null && mappingChild.Id != Guid.Empty)
                                {
                                    Menu child = await _uow.GetRepository<Menu>().GetSingleAsync(y => y.Id.Equals(mappingChild.Id.Value));
                                    child.ParentId = menu.Id;
                                    _uow.GetRepository<Menu>().Update(child);
                                }
                            }
                        }
                    }
                    await _uow.CommitAsync();

                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "CREATE_MENU_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MenuDTO>(menu)
                    };
                }
            }

            return resultDTO;
        }

        public async Task<ResultDTO> DeleteMenu(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<Menu>().GetSingleAsync(y => y.Id.Equals(Id));
            var listChild = await _uow.GetRepository<Menu>().AnyAsync(y => !y.Id.Equals(Id) && y.ParentId.Equals(Id));
            var listUserChild = await _uow.GetRepository<MenuUserMapping>().AnyAsync(y => y.MenuId.Equals(Id));
            var listDeptChild = await _uow.GetRepository<MenuDepartmentMapping>().AnyAsync(y => y.MenuId.Equals(Id));
            var listRoleChild = await _uow.GetRepository<MenuRoleMapping>().AnyAsync(y => y.MenuId.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "MENU_NOT_EXIST" } };
            }
            else if (listChild)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "MENU_HAS_CHILD_ITEM" } };
            }
            else if (listUserChild)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "MENU_HAS_CHILD_USER" } };
            }
            else if (listDeptChild)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "MENU_HAS_CHILD_DEPARTMENT" } };
            }
            else if (listRoleChild)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "MENU_HAS_CHILD_ROLE" } };
            }
            else
            {

                _uow.GetRepository<Menu>().Delete(existing);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "DELETE_MENU_IS_SUCCESSFULLY" },
                    Object = true
                };
            }
            return resultDTO;
        }
        #endregion
        #region GET DATA

        public async Task<ResultDTO> GetListMenuTree(QueryArgs args)
        {
            var lstMenu = await _uow.GetRepository<Menu>().FindByAsync<MenuTreeViewModel>(x => true);
            List<MenuTreeViewModel> menuTree = new List<MenuTreeViewModel>();
            List<MenuTreeViewModel> vmLstmenu = lstMenu.ToList();
            var highestMenu = lstMenu.Where(x => x.ParentId == x.Id).FirstOrDefault();
            if (highestMenu != null)
            {
                vmLstmenu.ForEach(x =>
                {
                    x.InverseParent = vmLstmenu.Where(y => y.ParentId == x.Id && y.Id != x.Id).OrderByDescending(k => k.Name).ToList();
                }
                );

                menuTree = vmLstmenu.Where(item => item.Id == highestMenu.Id).ToList();
                menuTree.AddRange(vmLstmenu.Where(item => !item.ParentId.HasValue).ToList());
                ResultDTO result = new ResultDTO
                {
                    Object = new ArrayResultDTO
                    {
                        Data = menuTree,
                        Count = 1,
                    },
                };
                return result;
            }
            else
            {

                vmLstmenu.ForEach(x =>
                x.InverseParent = vmLstmenu.Where(y => y.ParentId == x.Id).OrderByDescending(k => k.Name).ToList()

                );
                menuTree = vmLstmenu.Where(item => !item.ParentId.HasValue).ToList();
                if (!menuTree.Any())
                {
                    menuTree = vmLstmenu;
                }
                ResultDTO result = new ResultDTO
                {
                    Object = new ArrayResultDTO
                    {
                        Data = menuTree,
                        Count = 1,
                    },
                };
                return result;
            }
        }
        public async Task<ResultDTO> GetListMenu(QueryArgs args)
        {
            var resultDTO = new ResultDTO();
            var findMenu = await _uow.GetRepository<Menu>().FindByAsync<MenuDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            if (findMenu != null)
            {
                resultDTO.Object = findMenu;
            }
            return resultDTO;
        }
        #endregion
        #region USER
        public async Task<ResultDTO> GetListUserByMenuId(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var findUserInMenu = await _uow.GetRepository<MenuUserMapping>().FindByAsync<MenuUserMappingDTO>(x => x.MenuId == Id);
            if (findUserInMenu != null)
            {
                resultDTO.Object = findUserInMenu;
            }
            return resultDTO;
        }

        public async Task<ResultDTO> CreateOrUpdateUser(UserInMenuArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.MenuId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { "MENU_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.UserId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { "USER_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {
                //update
                var existing = await _uow.GetRepository<MenuUserMapping>().GetSingleAsync(y => y.MenuId.Equals(args.MenuId) && y.UserId.Equals(args.UserId) && !y.Id.Equals(args.Id));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "USER_IN_MENU_EXISTED" } };
                }
                else
                {
                    var currentMeta = await _uow.GetRepository<MenuUserMapping>().GetSingleAsync(y => y.Id.Equals(args.Id));
                    if (currentMeta == null)
                    {
                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "USER_IN_MENU_NOT_EXIST" } };
                    }
                    if (args.MenuId != Guid.Empty)
                    {
                        currentMeta.MenuId = args.MenuId;
                    }
                    if (args.UserId != Guid.Empty)
                    {
                        currentMeta.UserId = args.UserId;
                    }

                    var dept = _uow.GetRepository<MenuUserMapping>().Update(currentMeta);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "UPDATE_USER_IN_MENU_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MenuUserMappingDTO>(currentMeta)
                    };
                }
            }
            else
            {
                //insert
                var existing = await _uow.GetRepository<MenuUserMapping>().GetSingleAsync(y => y.MenuId.Equals(args.MenuId) && y.UserId.Equals(args.UserId));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "CREATE_USER_IN_MENU_IS_EXIST" } };
                }
                else
                {
                    var data = args.Adapt<MenuUserMapping>();
                    var menu = _uow.GetRepository<MenuUserMapping>().Add(data);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "CREATE_USER_IN_MENU_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MenuUserMappingDTO>(menu)
                    };
                }
            }
            return resultDTO;
        }
        public async Task<ResultDTO> DeleteUserInMenu(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<MenuUserMapping>().GetSingleAsync(y => y.Id.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "USER_IN_MENU_NOT_EXIST" } };
            }
            else
            {

                _uow.GetRepository<MenuUserMapping>().Delete(existing);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "DELETE_USER_IN_MENU_IS_SUCCESSFULLY" },
                    Object = true
                };
            }
            return resultDTO;
        }

        #endregion
        #region DEPARTMENT

        public async Task<ResultDTO> GetListDepartmentByMenuId(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var findDepartmentInMenu = await _uow.GetRepository<MenuDepartmentMapping>().FindByAsync<MenuDepartmentMappingDTO>(x => x.MenuId == Id);
            if (findDepartmentInMenu != null)
            {
                resultDTO.Object = findDepartmentInMenu;
            }
            return resultDTO;
        }

        public async Task<ResultDTO> CreateOrUpdateDepartment(DepartmentInMenuArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.MenuId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { "MENU_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.DepartmentId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { "DEPARTMENT_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {
                //update
                var existing = await _uow.GetRepository<MenuDepartmentMapping>().GetSingleAsync(y => y.MenuId.Equals(args.MenuId) && y.DepartmentId.Equals(args.DepartmentId) && !y.Id.Equals(args.Id));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "DEPARTMENT_IN_MENU_EXISTED" } };
                }
                else
                {
                    var currentMeta = await _uow.GetRepository<MenuDepartmentMapping>().GetSingleAsync(y => y.Id.Equals(args.Id));
                    if (currentMeta == null)
                    {
                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "DEPARTMENT_IN_MENU_NOT_EXIST" } };
                    }
                    if (args.MenuId != Guid.Empty)
                    {
                        currentMeta.MenuId = args.MenuId;
                    }
                    if (args.DepartmentId != Guid.Empty)
                    {
                        currentMeta.DepartmentId = args.DepartmentId;
                    }

                    var dept = _uow.GetRepository<MenuDepartmentMapping>().Update(currentMeta);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "UPDATE_DEPARTMENT_IN_MENU_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MenuDepartmentMappingDTO>(currentMeta)
                    };
                }
            }
            else
            {
                //insert
                var existing = await _uow.GetRepository<MenuDepartmentMapping>().GetSingleAsync(y => y.MenuId.Equals(args.MenuId) && y.DepartmentId.Equals(args.DepartmentId));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "CREATE_DEPARTMENT_IN_MENU_IS_EXIST" } };
                }
                else
                {
                    var data = args.Adapt<MenuDepartmentMapping>();
                    var menu = _uow.GetRepository<MenuDepartmentMapping>().Add(data);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "CREATE_DEPARTMENT_IN_MENU_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MenuDepartmentMappingDTO>(menu)
                    };
                }
            }
            return resultDTO;
        }
        public async Task<ResultDTO> DeleteDepartmentInMenu(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<MenuDepartmentMapping>().GetSingleAsync(y => y.Id.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "DEPARTMENT_IN_MENU_NOT_EXIST" } };
            }
            else
            {

                _uow.GetRepository<MenuDepartmentMapping>().Delete(existing);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "DELETE_USER_IN_MENU_IS_SUCCESSFULLY" },
                    Object = true
                };
            }
            return resultDTO;
        }

        #endregion
        #region ROLE

        public async Task<ResultDTO> GetListRoleByMenuId(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var findRoleInMenu = await _uow.GetRepository<MenuRoleMapping>().FindByAsync<MenuRoleMappingDTO>(x => x.MenuId == Id);
            if (findRoleInMenu != null)
            {
                resultDTO.Object = findRoleInMenu;
            }
            return resultDTO;
        }

        public async Task<ResultDTO> CreateOrUpdateRole(RoleInMenuArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.MenuId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { "MENU_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.RoleId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { "ROLE_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {
                //update
                var existing = await _uow.GetRepository<MenuRoleMapping>().GetSingleAsync(y => y.MenuId.Equals(args.MenuId) && y.RoleId.Equals(args.RoleId) && !y.Id.Equals(args.Id));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "ROLE_IN_MENU_EXISTED" } };
                }
                else
                {
                    var currentMeta = await _uow.GetRepository<MenuRoleMapping>().GetSingleAsync(y => y.Id.Equals(args.Id));
                    if (currentMeta == null)
                    {
                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "ROLE_IN_MENU_NOT_EXIST" } };
                    }
                    if (args.MenuId != Guid.Empty)
                    {
                        currentMeta.MenuId = args.MenuId;
                    }
                    if (args.RoleId != Guid.Empty)
                    {
                        currentMeta.RoleId = args.RoleId;
                    }

                    var dept = _uow.GetRepository<MenuRoleMapping>().Update(currentMeta);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "UPDATE_ROLE_IN_MENU_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MenuRoleMappingDTO>(currentMeta)
                    };
                }
            }
            else
            {
                //insert
                var existing = await _uow.GetRepository<MenuRoleMapping>().GetSingleAsync(y => y.MenuId.Equals(args.MenuId) && y.RoleId.Equals(args.RoleId));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "CREATE_ROLE_IN_MENU_IS_EXIST" } };
                }
                else
                {
                    var data = args.Adapt<MenuRoleMapping>();
                    var menu = _uow.GetRepository<MenuRoleMapping>().Add(data);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "CREATE_ROLE_IN_MENU_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MenuRoleMappingDTO>(menu)
                    };
                }
            }

            return resultDTO;
        }
        public async Task<ResultDTO> DeleteRoleInMenu(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<MenuRoleMapping>().GetSingleAsync(y => y.Id.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "ROLE_IN_MENU_NOT_EXIST" } };
            }
            else
            {

                _uow.GetRepository<MenuRoleMapping>().Delete(existing);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "DELETE_ROLE_IN_MENU_IS_SUCCESSFULLY" },
                    Object = true
                };
            }
            return resultDTO;
        }

        #endregion
    }
}
