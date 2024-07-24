using AutoMapper;
using IziWork.Business.Args;
using IziWork.Business.CustomExtensions;
using IziWork.Business.DTO;
using IziWork.Business.Enums;
using IziWork.Business.Interfaces;
using IziWork.Business.IRepositories;
using IziWork.Data.Abstracts;
using IziWork.Data.Entities;
using IziWork.Data.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static IziWork.Business.Handlers.WorkflowBusiness;
using IziWork.Business.Constans;

namespace IziWork.Business.Handlers
{
    public class TaskManagementBusiness : ITaskManagementBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IPermissionBusiness _permissionBusiness;
        private readonly AutoMapper.IMapper _mapper;
        public TaskManagementBusiness(IUnitOfWork uow, AutoMapper.IMapper mapper, IPermissionBusiness permissionBusiness)
        {
            _uow = uow;
            _mapper = mapper;
            _permissionBusiness = permissionBusiness;
        }
        public async Task<ResultDTO> GetListTasks(QueryArgs args)
        {
            var userList = await _uow.GetRepository<TaskManagement>().FindByAsync<TaskManagementDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<TaskManagement>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = userList,
                    Count = totalList
                }
            };
        }

        public async Task<ResultDTO> AssignTask(AssignTaskArgs args)
        {
            var resultDTO = new ResultDTO() { };
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.ParentTaskId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_PARENT_ID }, ErrorCodes = new List<int> { -1 } };
            }
            if (string.IsNullOrEmpty(args.Name))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.REQUIRED_NAME }, ErrorCodes = new List<int> { -1 } };
            }
            var findParentTask = _uow.GetRepository<TaskManagement>().FindById(args.ParentTaskId.Value);
            if (findParentTask == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_PARENT_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var newTask = new TaskManagement()
            {
                Name = args.Name,
                ParentTaskId = args.ParentTaskId,
                Type = (int) args.Type,
                FromDate = args.FromDate,
                ToDate = args.ToDate,
                DocumentId = findParentTask.DocumentId,
                ClassifyId = args.ClassifyId,
                CriticalLevel = args.CriticalLevel,
                Hour = args.Hour,
                Content = args.Content,
                Status = StatusConst.Task.NEW
            };
            var newTaskManagement = _uow.GetRepository<TaskManagement>().Add(newTask);
            _permissionBusiness.AddPerm(newTaskManagement.Id, null, null, newTaskManagement.CreatedById, PermEnum.View); // nguoi tao project xem task
            var mapData = _mapper.Map<TaskManagementDTO>(newTaskManagement);
            if (args.TaskDepartmentMappings.Any())
            {
                foreach (var taskDepartmentMapping in args.TaskDepartmentMappings)
                {
                    var findUserAssign = _uow.GetRepository<User>().GetSingle(x => x.Id == taskDepartmentMapping.UserId);
                    if (findUserAssign == null)
                    {
                        resultDTO = new ResultDTO { ErrorCodes = { -1 }, Messages = { MessageConst.NOT_FOUND_PROCESSOR } };
                        break;
                    }
                    var addTaskDepartmentMapping = new TaskDepartmentMapping()
                    {
                        TaskManagementId = newTaskManagement.Id,
                        UserId = findUserAssign.Id,
                        IsCoordinated = taskDepartmentMapping.IsCoordinated,
                        //IsViewed = taskDepartmentMapping.IsViewed,
                        IsProcessed = taskDepartmentMapping.IsProcessed
                    };
                    var addNewTask = _uow.GetRepository<TaskDepartmentMapping>().Add(addTaskDepartmentMapping);
                    if (addNewTask != null)
                        mapData.TaskDepartmentMappings.Add(_mapper.Map<TaskDepartmentMappingDTO>(addNewTask));

                    /*var perm = ((taskDepartmentMapping.IsProcessed != null && taskDepartmentMapping.IsProcessed.Value) || (taskDepartmentMapping.IsCoordinated != null && taskDepartmentMapping.IsCoordinated.Value))
                        ? PermEnum.Edit : PermEnum.View;*/
                    var perm = PermEnum.Edit;
                    _permissionBusiness.AddPerm(newTaskManagement.Id, null, null, findUserAssign.Id, perm); // quyen xu ly task
                }
            }

            if (args.AttachmentFileIds != null && args.AttachmentFileIds.Any())
            {
                foreach (var attachmentFileId in args.AttachmentFileIds)
                {
                    var findAttachment = _uow.GetRepository<AttachmentFile>().GetSingle(x => x.Id == attachmentFileId);
                    if (findAttachment != null)
                    {
                        var newDocumentAttachmentMapping = new TaskAttachmentMapping()
                        {
                            AttachmentFileId = findAttachment.Id,
                            TaskManagementId = newTaskManagement.Id,
                            Type = 1 // loai default
                        };
                        _uow.GetRepository<TaskAttachmentMapping>().Add(newDocumentAttachmentMapping);
                    }
                }
            }
            resultDTO.Object = mapData;

            if (resultDTO.ErrorCodes.Any())
            {
                return resultDTO;
                // _uow.Rollback();
            }
            this.AddLogs(newTaskManagement.Id, TaskManagementHistoriesType.ASSIGN_TASK, null, null, newTaskManagement.Status, args.Content);
            await _uow.CommitAsync();
            return resultDTO;
        }

        public async Task<ResultDTO> UpdateTask(AssignTaskArgs args)
        {
            var resultDTO = new ResultDTO() { };
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Id == null || (args.Id != null && args.Id.HasValue && args.Id == Guid.Empty))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }
            if (string.IsNullOrEmpty(args.Name))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.REQUIRED_NAME }, ErrorCodes = new List<int> { -1 } };
            }
            var findCurrentTask = _uow.GetRepository<TaskManagement>().FindById(args.Id.Value);
            if (findCurrentTask == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.ParentTaskId != null && args.ParentTaskId.HasValue)
            {
                var findParent = _uow.GetRepository<TaskManagement>().FindById(args.ParentTaskId.Value);
                if (findParent == null)
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_PARENT_ID }, ErrorCodes = new List<int> { -1 } };
                }

                var fromDateParent = findParent.FromDate;
                var toDateParent = findParent.ToDate;
                if (args.FromDate != null && fromDateParent != null && (args.FromDate.Date < fromDateParent.Value.Date || args.FromDate.Date > toDateParent.Value.Date))
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.FROM_DATE_IS_INVALID }, ErrorCodes = new List<int> { -1 } };
                }

                if (args.ToDate != null && toDateParent != null && (args.ToDate.Date > toDateParent.Value.Date || args.ToDate.Date < fromDateParent.Value.Date))
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.TO_DATE_IS_INVALID }, ErrorCodes = new List<int> { -1 } };
                }

                findCurrentTask.ParentTaskId = args.ParentTaskId;
            } else
                findCurrentTask.ParentTaskId = null;

            bool isChangePercentCompleted = findCurrentTask.PercentCompleted != null && args.PercentCompleted != null
                && findCurrentTask.PercentCompleted.Value != args.PercentCompleted.Value ? true : false;

            findCurrentTask.Name = args.Name;
            findCurrentTask.Type = (int) args.Type;
            findCurrentTask.FromDate = args.FromDate;
            findCurrentTask.ToDate = args.ToDate;
            findCurrentTask.ClassifyId = args.ClassifyId;
            findCurrentTask.CriticalLevel = args.CriticalLevel;
            findCurrentTask.Content = args.Content;
            findCurrentTask.Hour = args.Hour;
            if (findCurrentTask.PercentCompleted != null)
            {
                var listStatusValid = new List<string>() { StatusConst.Task.NEW, StatusConst.Task.VIEWED };
                if ((listStatusValid.Contains(findCurrentTask.Status) && args.PercentCompleted != null && args.PercentCompleted.Value > 0) || 
                    (findCurrentTask.PercentCompleted != null && findCurrentTask.PercentCompleted.Value > 0 && listStatusValid.Contains(findCurrentTask.Status)))
                {
                    findCurrentTask.Status = StatusConst.Task.IN_PROCESS;
                    if (findCurrentTask.ParentTaskId != null && findCurrentTask.ParentTaskId.HasValue)
                    {
                        var parentTask = await _uow.GetRepository<TaskManagement>(true).GetSingleAsync(x => x.Id == findCurrentTask.ParentTaskId);
                        if (parentTask != null)
                        {
                            parentTask.Status = StatusConst.Task.IN_PROCESS;
                            _uow.GetRepository<TaskManagement>().Update(parentTask);
                        }
                    }
                }
            }

            findCurrentTask.PercentCompleted = args.IsCompleted ? 100 : args.PercentCompleted;
            if (args.IsCompleted)
                findCurrentTask.Status = StatusConst.Task.COMPLETED;
            var updatedItem = _uow.GetRepository<TaskManagement>().Update(findCurrentTask);

            var mapData = _mapper.Map<TaskManagementDTO>(updatedItem);
            if (args.TaskDepartmentMappings != null && args.TaskDepartmentMappings.Any())
            {
                var oldDepartmentMapping = _uow.GetRepository<TaskDepartmentMapping>().FindBy(x => x.TaskManagementId == findCurrentTask.Id);
                if (oldDepartmentMapping != null)
                {
                    foreach(var item in oldDepartmentMapping)
                    {
                        var permission = _uow.GetRepository<Permission>().FindBy(
                            x => x.ItemId == findCurrentTask.Id && x.Perm == (int) PermEnum.Edit && item.UserId != null && x.UserId != null 
                            && item.UserId == x.UserId);
                        _uow.GetRepository<Permission>().Delete(permission);
                        _uow.GetRepository<TaskDepartmentMapping>().Delete(item);
                    }
                }
                foreach (var taskDepartmentMapping in args.TaskDepartmentMappings)
                {
                    var findUserAssign = _uow.GetRepository<User>().GetSingle(x => x.Id == taskDepartmentMapping.UserId);
                    if (findUserAssign == null)
                    {
                        resultDTO = new ResultDTO { ErrorCodes = { -1 }, Messages = { MessageConst.NOT_FOUND_PROCESSOR } };
                        break;
                    }
                    var addTaskDepartmentMapping = new TaskDepartmentMapping()
                    {
                        TaskManagementId = findCurrentTask.Id,
                        UserId = findUserAssign.Id,
                        IsCoordinated = taskDepartmentMapping.IsCoordinated,
                        //IsViewed = taskDepartmentMapping.IsViewed,
                        IsProcessed = taskDepartmentMapping.IsProcessed
                    };
                    var addNewTask = _uow.GetRepository<TaskDepartmentMapping>().Add(addTaskDepartmentMapping);
                    if (addNewTask != null)
                        mapData.TaskDepartmentMappings.Add(_mapper.Map<TaskDepartmentMappingDTO>(addNewTask));

                    /*var perm = ((taskDepartmentMapping.IsProcessed != null && taskDepartmentMapping.IsProcessed.Value) || (taskDepartmentMapping.IsCoordinated != null && taskDepartmentMapping.IsCoordinated.Value))
                        ? PermEnum.Edit : PermEnum.View;*/
                    var perm = PermEnum.Edit;
                    _permissionBusiness.AddPerm(findCurrentTask.Id, null, null, findUserAssign.Id, perm); // quyen xu ly task
                }
            }

            if (args.AttachmentFileIds != null && args.AttachmentFileIds.Any())
            {
                var oldAttachmentFiles = _uow.GetRepository<TaskAttachmentMapping>().FindBy(x => x.TaskManagementId == findCurrentTask.Id && x.Type == 1);
                if (oldAttachmentFiles != null)
                {
                    foreach (var item in oldAttachmentFiles) _uow.GetRepository<TaskAttachmentMapping>().Delete(item);
                }
                foreach (var attachmentFileId in args.AttachmentFileIds)
                {
                    var findAttachment = _uow.GetRepository<AttachmentFile>().GetSingle(x => x.Id == attachmentFileId);
                    if (findAttachment != null)
                    {
                        var newDocumentAttachmentMapping = new TaskAttachmentMapping()
                        {
                            AttachmentFileId = findAttachment.Id,
                            TaskManagementId = findCurrentTask.Id,
                            Type = 1 // loai default
                        };
                        _uow.GetRepository<TaskAttachmentMapping>().Add(newDocumentAttachmentMapping);
                    }
                }
            }
            resultDTO.Object = mapData;

            if (resultDTO.ErrorCodes.Any())
            {
                return resultDTO;
                // _uow.Rollback();
            }
            if (isChangePercentCompleted)
                this.AddLogs(findCurrentTask.Id, TaskManagementHistoriesType.UPDATE, updatedItem.PercentCompleted, null, findCurrentTask.Status, args.Content);

            await _uow.CommitAsync();
            return resultDTO;
        }

        public async Task<ResultDTO> CreateTask(AssignTaskArgs args)
        {
            var resultDTO = new ResultDTO() { };
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (string.IsNullOrEmpty(args.Name))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.REQUIRED_NAME }, ErrorCodes = new List<int> { -1 } };
            }

            var newTask = new TaskManagement() { };
            newTask.Name = args.Name;
            newTask.Type = (int)args.Type;
            newTask.FromDate = args.FromDate;
            newTask.ToDate = args.ToDate;
            newTask.ClassifyId = args.ClassifyId;
            newTask.CriticalLevel = args.CriticalLevel;
            newTask.Content = args.Content;
            newTask.Hour = args.Hour;
            newTask.PercentCompleted = args.IsCompleted ? 100 : args.PercentCompleted;
            var createdItem = _uow.GetRepository<TaskManagement>().Add(newTask);
            var mapData = _mapper.Map<TaskManagementDTO>(createdItem);
            if (args.TaskDepartmentMappings != null && args.TaskDepartmentMappings.Any())
            {
                foreach (var taskDepartmentMapping in args.TaskDepartmentMappings)
                {
                    var findUserAssign = _uow.GetRepository<User>().GetSingle(x => x.Id == taskDepartmentMapping.UserId);
                    if (findUserAssign == null)
                    {
                        resultDTO = new ResultDTO { ErrorCodes = { -1 }, Messages = { MessageConst.NOT_FOUND_PROCESSOR } };
                        break;
                    }
                    var addTaskDepartmentMapping = new TaskDepartmentMapping()
                    {
                        TaskManagementId = createdItem.Id,
                        UserId = findUserAssign.Id,
                        IsCoordinated = taskDepartmentMapping.IsCoordinated,
                        IsProcessed = taskDepartmentMapping.IsProcessed
                    };
                    var addNewTask = _uow.GetRepository<TaskDepartmentMapping>().Add(addTaskDepartmentMapping);
                    if (addNewTask != null)
                        mapData.TaskDepartmentMappings.Add(_mapper.Map<TaskDepartmentMappingDTO>(addNewTask));

                    var perm = PermEnum.Edit;
                    _permissionBusiness.AddPerm(createdItem.Id, null, null, findUserAssign.Id, perm); // quyen xu ly task
                }
            }

            if (args.AttachmentFileIds != null && args.AttachmentFileIds.Any())
            {
                foreach (var attachmentFileId in args.AttachmentFileIds)
                {
                    var findAttachment = _uow.GetRepository<AttachmentFile>().GetSingle(x => x.Id == attachmentFileId);
                    if (findAttachment != null)
                    {
                        var newDocumentAttachmentMapping = new TaskAttachmentMapping()
                        {
                            AttachmentFileId = findAttachment.Id,
                            TaskManagementId = createdItem.Id,
                            Type = 1 // loai default
                        };
                        _uow.GetRepository<TaskAttachmentMapping>().Add(newDocumentAttachmentMapping);
                    }
                }
            }
            resultDTO.Object = mapData;

            if (resultDTO.ErrorCodes.Any())
            {
                return resultDTO;
            }
            this.AddLogs(createdItem.Id, TaskManagementHistoriesType.CREATE, createdItem.PercentCompleted, null, createdItem.Status, args.Content);

            await _uow.CommitAsync();
            return resultDTO;
        }


        public async Task<ResultDTO> ExtendTasks(ExtendTaskArgs args)
        {
            var resultDTO = new ResultDTO() { };
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.TaskManagementId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var findTask = _uow.GetRepository<TaskManagement>().FindById(args.TaskManagementId);
            if (findTask == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var findExistTaskExtend = _uow.GetRepository<TaskExtend>().GetSingle(x => x.TaskManagementId == findTask.Id && !x.IsCompleted);
            if (findExistTaskExtend != null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.TASK_EXTEND_IS_EXISTS }, ErrorCodes = new List<int> { -1 } };
            }

            var newExtendTask = new TaskExtend();
            newExtendTask.ToDate = args.ToDate;
            newExtendTask.Note = args.Note;
            newExtendTask.NumberOfDaysIncurred = args.NumberOfDaysIncurred;
            newExtendTask.TaskManagementId = args.TaskManagementId;
            newExtendTask.AssignToUserId = findTask.CreatedById;

            var addSuccessNewItem = _uow.GetRepository<TaskExtend>().Add(newExtendTask);
            if (args.AttachmentFileIds != null && args.AttachmentFileIds.Any())
            {
                foreach (var attachmentFileId in args.AttachmentFileIds)
                {
                    var findAttachment = _uow.GetRepository<AttachmentFile>().GetSingle(x => x.Id == attachmentFileId);
                    if (findAttachment != null)
                    {
                        var newDocumentAttachmentMapping = new TaskExtendAttachmentMapping()
                        {
                            AttachmentFileId = findAttachment.Id,
                            TaskExtendId = addSuccessNewItem.Id
                        };
                        _uow.GetRepository<TaskExtendAttachmentMapping>().Add(newDocumentAttachmentMapping);
                    }
                }
            }

            resultDTO.Object = _mapper.Map<TaskExtendDTO>(newExtendTask);
            this.AddLogs(findTask.Id, TaskManagementHistoriesType.EXTEND_TO_DATE, null, args.ToDate, findTask.Status, args.Note);
            await _uow.CommitAsync();
            return resultDTO;
        }

        public async Task<ResultDTO> ApproveExtendTask(ApproveExtendTaskArgs args)
        {
            var resultDTO = new ResultDTO() { };
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.TaskManagementId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var findTask = _uow.GetRepository<TaskManagement>().FindById(args.TaskManagementId);
            if (findTask == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }
            if (findTask.ParentTaskId == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_PARENT_ID }, ErrorCodes = new List<int> { -1 } };
            }
            var findParentTask = _uow.GetRepository<TaskManagement>().FindById(findTask.ParentTaskId.Value);
            if (findParentTask == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_PARENT_ID }, ErrorCodes = new List<int> { -1 } };
            }
            var findTaskExtend = _uow.GetRepository<TaskExtend>().GetSingle(x => x.TaskManagementId == findTask.Id && !x.IsApproved);
            if (findTaskExtend == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_ITEM }, ErrorCodes = new List<int> { -1 } };
            }

            findTaskExtend.ApproverExtendToDate = args.ApproverExtendToDate;
            findTaskExtend.ApproverNote = args.ApproverNote;
            findTaskExtend.IsApproved = args.IsApproved;
            findTaskExtend.IsCompleted = true;
            var updatedExtendDate = _uow.GetRepository<TaskExtend>().Update(findTaskExtend);
            if (updatedExtendDate.ModifiedById != null && findParentTask.CreatedById != null && updatedExtendDate.ModifiedById != findParentTask.CreatedById)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_PERMISSION_APPROVE }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.IsApproved)
            {
                findTask.ToDate = args.ApproverExtendToDate != null && args.ApproverExtendToDate.HasValue ? args.ApproverExtendToDate : findTaskExtend.ToDate;
            }
            var updatedItem = _uow.GetRepository<TaskManagement>().Update(findTask);
            var mapData = _mapper.Map<TaskManagementDTO>(updatedItem);
            mapData.TaskExtends = _mapper.Map<List<TaskExtendDTO>>(new List<TaskExtendDTO>() { _mapper.Map<TaskExtendDTO>(updatedExtendDate) });
            resultDTO.Object = mapData;
            this.AddLogs(findTask.Id, TaskManagementHistoriesType.APPROVE_EXTEND_TO_DATE, null, args.ApproverExtendToDate, findTask.Status, args.ApproverNote);
            await _uow.CommitAsync();
            return resultDTO;
        }
        public async Task<TaskManagementDTO> GetById(Guid Id)
        {
            var resultTask = await _uow.GetRepository<TaskManagement>().GetSingleAsync<TaskManagementDTO>(x => x.Id == Id);
            return resultTask;
        }
        public async Task<List<TaskManagementDTO>> GetTaskByParentTaskId(Guid parentTaskId)
        {
            var listTask = new List<TaskManagementDTO>();
            var resultTask = await _uow.GetRepository<TaskManagement>().FindByAsync<TaskManagementDTO>(y => y.ParentTaskId != null && y.ParentTaskId.HasValue && parentTaskId == y.ParentTaskId.Value);
            if (resultTask != null) listTask = resultTask.ToList();
            return listTask;
        }

        public async Task<ResultDTO> UpdateStatusTask(UpdateStatusTaskArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.TaskManagementId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var findTask = _uow.GetRepository<TaskManagement>().FindById(args.TaskManagementId);
            if (findTask == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }

            /*if (!findTask.Status.Equals(StatusConst.Task.NEW))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.STATUS_ITEM_INVALID_FOR_THIS_ACTION }, ErrorCodes = new List<int> { -1 } };
            }*/

            var statusInvalid = new List<string>()
            {
                StatusConst.Task.VIEWED,
                StatusConst.Task.REPORTING,
                StatusConst.Task.COMPLETED,
                StatusConst.Task.IN_PROCESS
            };

            if (!statusInvalid.Contains(args.Status))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.PARAM_INVALID_FOR_THIS_ACTION }, ErrorCodes = new List<int> { -1 } };
            }

            findTask.Status = args.Status;
            if (findTask.Status.Equals(StatusConst.Task.COMPLETED))
                findTask.PercentCompleted = 100;

            var updatedItem = _uow.GetRepository<TaskManagement>().Update(findTask);
            var mapData = _mapper.Map<TaskManagementDTO>(updatedItem);
            resultDTO.Object = mapData;
            await _uow.CommitAsync();

            return resultDTO;
        }

        #region Log
        public async Task<ResultDTO> GetTaskManagementHistories(QueryArgs args)
        {
            var userList = await _uow.GetRepository<TaskManagementHistory>().FindByAsync<TaskManagementHistoryDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<TaskManagementHistory>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = userList,
                    Count = totalList
                }
            };
        }

        
        public void AddLogs(Guid TaskManagementId, string type, int? percentCompleted, DateTimeOffset? extendToDate, string status, string note)
        {
            var addLog = new TaskManagementHistory()
            {
                TaskManagementId = TaskManagementId,
                Type = type
            };
            if (percentCompleted != null && percentCompleted.HasValue) addLog.PercentCompleted = percentCompleted;
            if (!string.IsNullOrEmpty(status)) addLog.Status = status;
            if (!string.IsNullOrEmpty(note)) addLog.Note = note;
            if (extendToDate != null) addLog.ExtendToDate = extendToDate;
            _uow.GetRepository<TaskManagementHistory>().Add(addLog);
        }
        #endregion
    }
}
