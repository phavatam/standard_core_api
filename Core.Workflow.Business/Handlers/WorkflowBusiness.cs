﻿using AutoMapper;

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
using static Core.Workflow.Business.Handlers.WorkflowBusiness;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Eventing.Reader;
using Core.Workflow.Business.Interface;
using Core.Workflow.Business.DTO;
using Core.Workflow.Business.Args;
using Core.Repositories.Business.IRepositories;
using Core.Repositories.Business.Interface;
using Core.Repositories.Business.DTO;
using IziWork.Common.Constans;
using Core.Repositories.Business.CustomExtensions;
using IziWork.Common.Enums;
using IziWork.Common.Args;
using IziWork.Common.DTO;

namespace Core.Workflow.Business.Handlers
{
    public class WorkflowBusiness : IWorkflowBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IPermissionBusiness _permissionBusiness;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid? _refDeparmentId = null;
        private readonly Guid WorkflowDocumentForPersonalApprovalId = new Guid("e5a8f784-4998-b6e7-6f93-f66f6c2b2d7d");
        public WorkflowBusiness(IUnitOfWork uow, IMapper mapper, IPermissionBusiness permissionBusiness, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _permissionBusiness = permissionBusiness;
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUserDTO GetCurrentUser()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.Items["UUID"] as Guid? ?? Guid.Empty;
            if (currentUserId != Guid.Empty)
            {
                var user = _uow.GetRepository<User>().GetSingle(x => x.Id == currentUserId && x.IsDeleted != null && !x.IsDeleted.Value && x.IsActivated);
                if (user != null)
                {
                    var userInDepartmentList = new List<CurrentUserDepartmentMappingDTO>();
                    var userInDepartment = _uow.GetRepository<UserDepartmentMapping>().FindBy(x => x.UserId == currentUserId && x.IsDeleted != null && !x.IsDeleted.Value);
                    foreach (var userInDept in userInDepartment)
                    {
                        var mapData = _mapper.Map<CurrentUserDepartmentMappingDTO>(userInDept);
                        var userInDepartmentRoleList = new List<CurrentUserDepartmentRoleMappingDTO>();
                        var currentRole = _uow.GetRepository<UserDepartmentRoleMapping>().FindBy(x => x.UserDepartmentMappingId == userInDept.Id && x.IsDeleted != null && !x.IsDeleted.Value);
                        if (currentRole != null)
                            userInDepartmentRoleList.AddRange(_mapper.Map<List<CurrentUserDepartmentRoleMappingDTO>>(currentRole.ToList()));

                        mapData.UserDepartmentRoleMappings = userInDepartmentRoleList;
                        if (userInDepartmentRoleList != null)
                        {
                            mapData.RoleIds = userInDepartmentRoleList.Where(x => x.RoleId != null).Select(x => x.RoleId.Value).ToList();
                        }
                        userInDepartmentList.Add(mapData);
                    }
                    return new CurrentUserDTO
                    {
                        Id = user.Id,
                        LoginName = user?.LoginName,
                        FullName = user?.FullName,
                        UserDepartmentMappingDTO = userInDepartmentList,
                    };
                }
            }
            return null;
        }

        public async Task<ResultDTO> CreateWorkflowTemplate(WorkflowTemplateArgs args)
        {
            WorkflowTemplate newWFTemplate = new WorkflowTemplate() { };
            newWFTemplate.WorkflowName = args.WorkflowName;
            newWFTemplate.ItemType = args.ItemType;
            newWFTemplate.IsActivated = args.IsActivated;
            newWFTemplate.Order = args.Order;
            newWFTemplate.DefaultCompletedStatus = args.DefaultCompletedStatus;
            var createdWorkflowTemplate = _uow.GetRepository<WorkflowTemplate>().Add(newWFTemplate);
            if (args.WorkflowSteps != null && args.WorkflowSteps.Any())
            {
                foreach (var step in args.WorkflowSteps)
                {
                    var addStep = new WorkflowStep() { };
                    addStep.WorkflowTemplateId = createdWorkflowTemplate.Id;
                    addStep.StepName = step.StepName;
                    addStep.StepNumber = step.StepNumber;
                    addStep.SuccessVote = step.SuccessVote;
                    addStep.FailureVote = step.FailureVote;
                    addStep.DueDateNumber = step.DueDateNumber;
                    addStep.AssignToDepartmentId = step.AssignToDepartmentId;
                    addStep.AssignToUserId = step.AssignToUserId;
                    addStep.StatusId = step.StatusId;
                    addStep.IsSign = step.IsSign;
                    _uow.GetRepository<WorkflowStep>().Add(addStep);
                    if (step.WorkflowRoles != null && step.WorkflowRoles.Any())
                    {
                        foreach (var role in step.WorkflowRoles)
                        {
                            var addRole = new WorkflowRole() { };
                            addRole.WorkflowStepId = addStep.Id;
                            addRole.RoleId = role.RoleId;
                            _uow.GetRepository<WorkflowRole>().Add(addRole);
                        }
                    }
                }
            }
            await _uow.CommitAsync();
            return new ResultDTO() { Object = _mapper.Map<WorkflowTemplateDTO>(createdWorkflowTemplate) };
        }

        public async Task<ResultDTO> UpdateWorkflowTemplate(WorkflowTemplateArgs args)
        {
            if (args == null)
            {
                return new ResultDTO() { Messages = new List<string> { "Payload is null" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Id == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { "WORKFLOW_TEMPLATE_ID_IS_NULL" }, ErrorCodes = new List<int> { -1 } };
            }

            var findWorkflowTemplate = await _uow.GetRepository<WorkflowTemplate>().GetSingleAsync(x => x.Id == args.Id);
            if (findWorkflowTemplate == null)
            {
                return new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_WORKFLOW_TEMPLATE" }, ErrorCodes = new List<int> { -1 } };
            }

            if (!string.IsNullOrEmpty(args.WorkflowName))
            {
                findWorkflowTemplate.WorkflowName = args.WorkflowName;
            }
            if (!string.IsNullOrEmpty(args.ItemType))
            {
                findWorkflowTemplate.ItemType = args.ItemType;
            }
            if (!string.IsNullOrEmpty(args.DefaultCompletedStatus))
            {
                findWorkflowTemplate.DefaultCompletedStatus = args.DefaultCompletedStatus;
            }
            if (!string.IsNullOrEmpty(args.WorkflowName))
            {
                findWorkflowTemplate.WorkflowName = args.WorkflowName;
            }
            findWorkflowTemplate.Order = args.Order;
            _uow.GetRepository<WorkflowTemplate>().Update(findWorkflowTemplate);

            var oldStep = await _uow.GetRepository<WorkflowStep>().FindByAsync(x => x.WorkflowTemplateId == findWorkflowTemplate.Id);
            if (oldStep != null)
            {
                _uow.GetRepository<WorkflowStep>().Delete(oldStep);
            }
            if (args.WorkflowSteps != null && args.WorkflowSteps.Any())
            {
                foreach (var step in args.WorkflowSteps)
                {
                    var addStep = new WorkflowStep() { };
                    addStep.WorkflowTemplateId = findWorkflowTemplate.Id;
                    addStep.StepName = step.StepName;
                    addStep.StepNumber = step.StepNumber;
                    addStep.SuccessVote = step.SuccessVote;
                    addStep.FailureVote = step.FailureVote;
                    addStep.DueDateNumber = step.DueDateNumber;
                    addStep.AssignToDepartmentId = step.AssignToDepartmentId;
                    addStep.AssignToUserId = step.AssignToUserId;
                    addStep.StatusId = step.StatusId;
                    addStep.IsSign = step.IsSign;
                    _uow.GetRepository<WorkflowStep>().Add(addStep);
                    if (step.WorkflowRoles != null && step.WorkflowRoles.Any())
                    {
                        foreach (var role in step.WorkflowRoles)
                        {
                            var addRole = new WorkflowRole() { };
                            addRole.WorkflowStepId = addStep.Id;
                            addRole.RoleId = role.RoleId;
                            _uow.GetRepository<WorkflowRole>().Add(addRole);
                        }
                    }
                }
            }
            await _uow.CommitAsync();
            return new ResultDTO() { Object = _mapper.Map<WorkflowTemplateDTO>(findWorkflowTemplate) };
        }

        public async Task<ResultDTO> GetWorkflowTemplates(QueryArgs args)
        {
            var wfTemplates = await _uow.GetRepository<WorkflowTemplate>().FindByAsync<WorkflowTemplateDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var count = await _uow.GetRepository<WorkflowTemplate>().CountAsync(args.Predicate, args.PredicateParameters);
            ResultDTO result = new ResultDTO
            {
                Object = new ArrayResultDTO { Data = wfTemplates, Count = count }
            };
            return result;
        }

        public async Task<ResultDTO> GetWorkflowTemplateById(Guid id)
        {
            var wfTemplate = await _uow.GetRepository<WorkflowTemplate>().GetSingleAsync<WorkflowTemplateDTO>(x => x.Id == id);
            return new ResultDTO() { Object = _mapper.Map<WorkflowTemplateDTO>(wfTemplate) };
        }
        public async Task<ResultDTO> GetPermissionApproveByItemId(Guid itemId)
        {
            var resultDTO = new ResultDTO() { };
            var item = await GetWorkflowItem(itemId);
            if (item == null)
            {
                return new ResultDTO()
                {
                    ErrorCodes = new List<int> { -1 },
                    Messages = new List<string>() { MessageConst.NOT_FOUND_ITEM }
                };
            }

            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.CANNOT_FIND_CURRENT_USER }, ErrorCodes = new List<int>() { -1 } };
                goto Finish;
            }

            var workflowInstance = await _uow.GetRepository<WorkflowInstance>().GetSingleAsync(x => x.ItemId == itemId);
            if (workflowInstance == null)
            {
                return new ResultDTO()
                {
                    ErrorCodes = new List<int> { -1 },
                    Messages = new List<string>() { MessageConst.NOT_FOUND_ITEM }
                };
            }

            if (workflowInstance.IsCompleted)
            {
                return new ResultDTO()
                {
                    ErrorCodes = new List<int> { -1 },
                    Messages = new List<string>() { MessageConst.WORKFLOW.WORKFLOW_IS_COMPLETED }
                };
            }

            var lastHistory = await _uow.GetRepository<WorkflowProcessing>().GetSingleAsync(x => x.InstanceId == workflowInstance.Id, "Created desc");
            if (lastHistory == null || lastHistory.IsStepCompleted)
            {
                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.CANNOT_FIND_ANY_RUNNING_WORKFLOW }, ErrorCodes = new List<int>() { -1 } };
                goto Finish;
            }

            var mappedWorkflowInstance = _mapper.Map<WorkflowInstanceDTO>(workflowInstance);
            var stepNumber = lastHistory.StepNumber;
            var findCurrentStep = mappedWorkflowInstance.WorkflowData.WorkflowSteps.Where(x => x.StepNumber.Equals(stepNumber)).FirstOrDefault();
            if (findCurrentStep == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.CANNOT_FIND_STEP }, ErrorCodes = new List<int>() { -1 } };
                goto Finish;
            }
            var hasPermisson = lastHistory.AssignedToUserId != null && lastHistory.AssignedToUserId.HasValue ? lastHistory.AssignedToUserId == currentUser.Id : false;
            if (!hasPermisson)
            {
                hasPermisson = lastHistory.AssignedToDepartmentId != null && lastHistory.AssignedToDepartmentId.HasValue && lastHistory.AssignedToRoleId != null && lastHistory.AssignedToRoleId.HasValue
                    ? currentUser.UserDepartmentMappingDTO.Any(i => i.DepartmentId == lastHistory.AssignedToDepartmentId && i.RoleIds.Contains(lastHistory.AssignedToRoleId.Value)) : false;
            }

            var permissionApproveModel = new PermissionApproveDTO()
            {
                HasPermissionApprove = hasPermisson,
                CurrentStep = hasPermisson ? findCurrentStep : null
            };

            resultDTO.Object = permissionApproveModel;
        Finish:
            return resultDTO;
        }

        private async Task<WorkflowItemDTO> GetWorkflowItem(Guid ItemId)
        {
            var ass = Assembly.GetAssembly(typeof(WorkflowInstance));
            var workflowModelTypes = ass.GetTypes().Where(x => typeof(IAutoNumber).IsAssignableFrom(x)).Where(p => !p.Name.Equals("IAutoNumber"));
            foreach (var workflowModelType in workflowModelTypes)
            {
                var repository = DynamicInvoker.InvokeGeneric(_uow, "GetRepository", workflowModelType);
                var item = await DynamicInvoker.InvokeAsync(repository, "FindByIdAsync", ItemId);
                if (item != null)
                {
                    var returnDTO = item.Adapt<WorkflowItemDTO>();
                    returnDTO.Type = workflowModelType.Name;
                    return returnDTO;
                }
            }
            return null;
        }

        private async Task UpdateStatusItem(Guid ItemId, string status)
        {
            var ass = Assembly.GetAssembly(typeof(WorkflowInstance));
            var workflowModelTypes = ass.GetTypes().Where(x => typeof(IAutoNumber).IsAssignableFrom(x)).Where(p => !p.Name.Equals("IAutoNumber"));
            foreach (var workflowModelType in workflowModelTypes)
            {
                var repository = DynamicInvoker.InvokeGeneric(_uow, "GetRepository", workflowModelType);
                var item = (IAutoNumber)await DynamicInvoker.InvokeAsync(repository, "FindByIdAsync", ItemId);
                if (!(item is null))
                {
                    item.Status = status;
                    break;
                }
            }
        }

        public async Task<ResultDTO> SubmitDocument(SubmitDocumentArgs args)
        {
            var resultDTO = new ResultDTO() { };
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.CANNOT_FIND_CURRENT_USER }, ErrorCodes = new List<int>() { -1 } };
                goto Finish;
            }
            var findHeadcountDept = currentUser.UserDepartmentMappingDTO.Where(x => x.IsHeadCount).ToList();
            if (findHeadcountDept == null || !findHeadcountDept.Any())
            {
                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.CANNOT_FIND_CURRENT_USER }, ErrorCodes = new List<int>() { -1 } };
                goto Finish;
            }

            if (args == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            if (args.WorkflowTemplateId == null && args.ApproverId == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            if (args.WorkflowTemplateId != null && args.ApproverId != null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CAN_ONLY_CHOOSE_ONE_WORKFLOW_TYPE }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            var item = await GetWorkflowItem(args.Id);
            if (item == null)
            {
                return new ResultDTO()
                {
                    ErrorCodes = new List<int> { -1 },
                    Messages = new List<string>() { "Item not found!" }
                };
            }

            if (!new List<string>() { StatusConst.NEW, StatusConst.REQUEST_TO_CHANGE }.Contains(item.Status))
            {
                return new ResultDTO()
                {
                    ErrorCodes = new List<int> { -1 },
                    Messages = new List<string>() { "Status Item is invalid to submit!" }
                };
            }

            if (args.WorkflowTemplateId != null && args.WorkflowTemplateId.HasValue)
            {
                // Quy trinh workflow default
                var findWorklfowTemplate = await _uow.GetRepository<WorkflowTemplate>().GetSingleAsync<WorkflowTemplateDTO>(x => x.Id == args.WorkflowTemplateId);
                if (findWorklfowTemplate == null)
                {
                    return new ResultDTO()
                    {
                        ErrorCodes = new List<int> { -1 },
                        Messages = new List<string>() { MessageConst.WORKFLOW.CANNOT_FIND_WORKFLOW_TEMPLATE }
                    };
                }

                var startWorkflowArgs = new StartWorkflowArgs() { ItemId = item.Id, WorkflowTemplateId = findWorklfowTemplate.Id, Comment = args.Comment };
                resultDTO = await StartWorkflow(startWorkflowArgs);
            }
            else if (args.ApproverId.HasValue)
            {
                // Workflow assign approver
                var findApprover = _uow.GetRepository<User>().GetSingle(x => x.Id == args.ApproverId.Value);
                if (findApprover == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CANNOT_FIND_APPROVER_ID }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                
                var workflowDefaulAssignApprover = await _uow.GetRepository<WorkflowTemplate>().GetSingleAsync<WorkflowTemplateDTO>(x => x.Id == WorkflowDocumentForPersonalApprovalId);
                if (workflowDefaulAssignApprover == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CANNOT_FIND_WORKFLOW }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                var newWFInstance = new WorkflowInstance()
                {
                    TemplateId = workflowDefaulAssignApprover.Id,
                    WorkflowName = workflowDefaulAssignApprover.WorkflowName,
                    WorkflowDataStr = JsonConvert.SerializeObject(workflowDefaulAssignApprover),
                    ItemId = args.Id,
                    ItemReferenceNumber = item.ReferenceNumber,
                    IsCompleted = false,
                    IsTerminated = false,
                    DefaultCompletedStatus = workflowDefaulAssignApprover.DefaultCompletedStatus
                };

                var addedWorkflowInstance = _uow.GetRepository<WorkflowInstance>().Add(newWFInstance);
                var mapperDataWorkflowInstance = _mapper.Map<WorkflowInstanceDTO>(addedWorkflowInstance);
                var firstStep = mapperDataWorkflowInstance.WorkflowData.WorkflowSteps.Where(x => x.StepNumber == 1).FirstOrDefault();
                if (firstStep == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CANNOT_FIND_STEP }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }

                var newFirstStepWorkflowProcessing = new WorkflowProcessing()
                {
                    InstanceId = mapperDataWorkflowInstance.Id,
                    ItemId = mapperDataWorkflowInstance.ItemId,
                    RequestedUserId = currentUser.Id,
                    RequestedUserName = currentUser.FullName,
                    RequestedDepartmentId = findHeadcountDept.FirstOrDefault().DepartmentId,
                    RequestedDepartmentName = findHeadcountDept.FirstOrDefault().DepartmentName,
                    ReferenceNumber = mapperDataWorkflowInstance.ItemReferenceNumber,
                    Comment = args.Comment,
                    VoteType = (int)VoteType.Approve,
                    StepNumber = firstStep.StepNumber,
                    IsStepCompleted = true,
                    DueDate = DateTimeOffset.Now,
                    ItemType = workflowDefaulAssignApprover.ItemType,
                    Status = StatusConst.WORKFLOW.WAITING_FOR_SUBMIT
                };
                var addedNewFirstStepWorkflowProcessing = _uow.GetRepository<WorkflowProcessing>().Add(newFirstStepWorkflowProcessing);

                var secondStep = mapperDataWorkflowInstance.WorkflowData.WorkflowSteps.Where(x => x.StepNumber == 2).FirstOrDefault();
                if (secondStep == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CANNOT_FIND_STEP }, ErrorCodes = new List<int> { -1 } };
                    goto Finish;
                }
                var newSecondStepWorkflowProcessing = new WorkflowProcessing()
                {
                    InstanceId = mapperDataWorkflowInstance.Id,
                    ItemId = mapperDataWorkflowInstance.ItemId,
                    RequestedUserId = currentUser.Id,
                    RequestedUserName = currentUser.FullName,
                    RequestedDepartmentId = newFirstStepWorkflowProcessing.RequestedDepartmentId,
                    RequestedDepartmentName = newFirstStepWorkflowProcessing.RequestedDepartmentName,
                    ReferenceNumber = mapperDataWorkflowInstance.ItemReferenceNumber,
                    AssignedToUserId = findApprover.Id,
                    VoteType = (int) VoteType.None,
                    StepNumber = secondStep.StepNumber,
                    DueDate = DateTimeOffset.Now.AddDays(secondStep.DueDateNumber),
                    ItemType = workflowDefaulAssignApprover.ItemType,
                    Status = StatusConst.SetStatus("")
                };
                var addedNewSecondStepWorkflowProcessing = _uow.GetRepository<WorkflowProcessing>().Add(newSecondStepWorkflowProcessing);

                _permissionBusiness.AddPerm(mapperDataWorkflowInstance.ItemId, null, null, findApprover.Id, PermEnum.View);
                _permissionBusiness.AddPerm(addedNewSecondStepWorkflowProcessing.Id, null, null, findApprover.Id, PermEnum.View);

                await this.UpdateStatusItem(mapperDataWorkflowInstance.ItemId, StatusConst.SetStatus("")); // cap nhat item cua phieu

                item.Status = StatusConst.SetStatus("");
                resultDTO.Messages = new List<string>() { MessageConst.WORKFLOW.START_WORLFOW_IS_SUCCESSFULLY };
                resultDTO.Object = mapperDataWorkflowInstance;
                await _uow.CommitAsync();
            }
        Finish:
            return resultDTO;
        }

        public async Task<ResultDTO> StartWorkflow(StartWorkflowArgs args)
        {
            var resultDTO = new ResultDTO() { };
            var type = string.Empty;
            if (args.ItemId == Guid.Empty)
            {
                return new ResultDTO()
                {
                    ErrorCodes = new List<int> { -1 },
                    Messages = new List<string>() { "Item not found!" }
                };
            }

            var item = await GetWorkflowItem(args.ItemId);
            if (item == null)
            {
                return new ResultDTO()
                {
                    ErrorCodes = new List<int> { -1 },
                    Messages = new List<string>() { "Item not found!" }
                };
            }

            if (!item.Status.Equals(StatusConst.NEW) && !item.Status.Equals(StatusConst.REQUEST_TO_CHANGE))
            {
                return new ResultDTO()
                {
                    ErrorCodes = new List<int> { -1 },
                    Messages = new List<string>() { "WORKFLOW_IS_ALREADY_START!" }
                };
            }

            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.CANNOT_FIND_CURRENT_USER }, ErrorCodes = new List<int>() { -1 } };
                goto Finish;
            }

            var findHeadCountCurrentUser = currentUser.UserDepartmentMappingDTO.FirstOrDefault(x => x.IsHeadCount);
            if (findHeadCountCurrentUser == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.CANNOT_FIND_CURRENT_USER }, ErrorCodes = new List<int>() { -1 } };
                goto Finish;
            }

            var wfTemplate = await _uow.GetRepository<WorkflowTemplate>().GetSingleAsync<WorkflowTemplateDTO>(x => x.Id == args.WorkflowTemplateId);
            if (wfTemplate == null)
            {
                return new ResultDTO()
                {
                    ErrorCodes = new List<int> { -1 },
                    Messages = new List<string>() { "Workflow Template not found !" }
                };
            }
            wfTemplate.WorkflowSteps = wfTemplate.WorkflowSteps.OrderBy(x => x.StepNumber).ToList();
            var wfInstance = new WorkflowInstance()
            {
                TemplateId = wfTemplate.Id,
                WorkflowDataStr = JsonConvert.SerializeObject(wfTemplate),
                ItemId = args.ItemId,
                WorkflowName = wfTemplate.WorkflowName,
                DefaultCompletedStatus = wfTemplate.DefaultCompletedStatus,
                ItemReferenceNumber = item.ReferenceNumber
            };
            var addWFInstanceSuccess = _uow.GetRepository<WorkflowInstance>().Add(wfInstance);
            var mappedWFInstance = _mapper.Map<WorkflowInstanceDTO>(addWFInstanceSuccess);

            var firstStep = mappedWFInstance.WorkflowData.WorkflowSteps.Where(x => x.StepNumber == 1).FirstOrDefault();
            if (firstStep == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CANNOT_FIND_STEP }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            var newFirstStepWorkflowProcessing = new WorkflowProcessing()
            {
                InstanceId = mappedWFInstance.Id,
                ItemId = mappedWFInstance.ItemId,
                RequestedUserId = currentUser.Id,
                RequestedUserName = currentUser.FullName,
                ReferenceNumber = mappedWFInstance.ItemReferenceNumber,
                RequestedDepartmentId = findHeadCountCurrentUser.DepartmentId,
                RequestedDepartmentName = findHeadCountCurrentUser.DepartmentName,
                Comment = args.Comment,
                VoteType = (int) VoteType.Approve,
                StepNumber = firstStep.StepNumber,
                IsStepCompleted = true,
                DueDate = DateTimeOffset.Now,
                ItemType = wfTemplate.ItemType,
                Status = StatusConst.WORKFLOW.WAITING_FOR_SUBMIT
            };
            var addedNewFirstStepWorkflowProcessing = _uow.GetRepository<WorkflowProcessing>().Add(newFirstStepWorkflowProcessing);

            var secondStep = mappedWFInstance.WorkflowData.WorkflowSteps.Where(x => x.StepNumber == 2).FirstOrDefault();
            if (secondStep == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CANNOT_FIND_NEXT_STEP }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            var roleList = secondStep.WorkflowRoles != null ? secondStep.WorkflowRoles.Select(x => x.RoleId).ToList() : [];
            var findDepartmentPermApproveId = await GetDepartmentPermApprove(findHeadCountCurrentUser.DepartmentId, secondStep.AssignToDepartmentId, roleList);
            if (findDepartmentPermApproveId == Guid.Empty && secondStep.AssignToUserId == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CANNOT_FIND_APPROVER_ID_IN_NEXT_STEP }, ErrorCodes = new List<int> { -1 } };
                goto Finish;
            }

            var findStatus = _uow.GetRepository<Status>().FindById(secondStep.StatusId);
            if (findStatus == null)
            {
                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.STATUS_NOT_FOUND }, ErrorCodes = new List<int>() { -1 } };
                goto Finish;
            }

            var newSecondStepWorkflowProcessing = new WorkflowProcessing()
            {
                InstanceId = mappedWFInstance.Id,
                ItemId = mappedWFInstance.ItemId,
                RequestedUserId = currentUser.Id,
                RequestedUserName = currentUser.FullName,
                ReferenceNumber = mappedWFInstance.ItemReferenceNumber,
                AssignedToUserId = secondStep.AssignToUserId,
                AssignedToDepartmentId = findDepartmentPermApproveId != Guid.Empty ? findDepartmentPermApproveId : null,
                RequestedDepartmentId = newFirstStepWorkflowProcessing.RequestedDepartmentId,
                RequestedDepartmentName = newFirstStepWorkflowProcessing.RequestedDepartmentName,
                AssignedToRoleId = roleList.Any() ? roleList.FirstOrDefault() : null,
                VoteType = (int) VoteType.None,
                StepNumber = secondStep.StepNumber,
                DueDate = DateTimeOffset.Now.AddDays(secondStep.DueDateNumber),
                ItemType = wfTemplate.ItemType,
                Status = StatusConst.SetStatus(findStatus.Name)
            };
            var addedNewSecondStepWorkflowProcessing = _uow.GetRepository<WorkflowProcessing>().Add(newSecondStepWorkflowProcessing);

            _permissionBusiness.AddPerm(mappedWFInstance.ItemId, newSecondStepWorkflowProcessing.RequestedDepartmentId, newSecondStepWorkflowProcessing.AssignedToRoleId, newSecondStepWorkflowProcessing.AssignedToUserId, PermEnum.View);
            _permissionBusiness.AddPerm(addedNewSecondStepWorkflowProcessing.Id, newSecondStepWorkflowProcessing.RequestedDepartmentId, newSecondStepWorkflowProcessing.AssignedToRoleId, newSecondStepWorkflowProcessing.AssignedToUserId, PermEnum.View);

            await UpdateStatusItem(mappedWFInstance.ItemId, StatusConst.SetStatus(findStatus.Name));
            await _uow.CommitAsync();
            resultDTO.Messages = new List<string>() { MessageConst.WORKFLOW.START_WORLFOW_IS_SUCCESSFULLY };
            mappedWFInstance.RoundNum = await _uow.GetRepository<WorkflowInstance>().CountAsync(x => x.ItemId == args.ItemId);
            resultDTO.Object = mappedWFInstance;
            Finish:
            return resultDTO;
        }

        private async Task<Guid> GetDepartmentPermApprove(Guid? requestedDepartmentId, Guid? assignToDepartmentId, List<Guid> roleIds)
        {
            var foundDepartmentId = Guid.Empty;
            if (assignToDepartmentId != null && assignToDepartmentId.HasValue)
            {
                foundDepartmentId = assignToDepartmentId.Value;
                goto Finish;
            }

            var findRequestedDepartment = await _uow.GetRepository<Department>().GetSingleAsync(x => x.Id == requestedDepartmentId);
            if (findRequestedDepartment != null)
            {
                var hasParticipants = await _uow.GetRepository<UserDepartmentMapping>().AnyAsync(x => x.DepartmentId == findRequestedDepartment.Id && x.UserDepartmentRoleMappings.Any(i => roleIds.Contains(i.RoleId.Value)));
                if (hasParticipants)
                    return findRequestedDepartment.Id;
            }
            bool skip = false;
            while (!skip)
            {
                if (findRequestedDepartment.ParentId.HasValue)
                {
                    findRequestedDepartment = await _uow.GetRepository<Department>().GetSingleAsync(x => x.Id == findRequestedDepartment.ParentId);
                    if (findRequestedDepartment != null)
                    {
                        var hasParticipants = await _uow.GetRepository<UserDepartmentMapping>().AnyAsync(x => x.DepartmentId == findRequestedDepartment.Id && x.UserDepartmentRoleMappings.Any(i => roleIds.Contains(i.RoleId.Value)));
                        if (hasParticipants)
                            return findRequestedDepartment.Id;
                        else
                            _refDeparmentId = findRequestedDepartment.Id;
                    }
                }
                if (findRequestedDepartment == null || !findRequestedDepartment.ParentId.HasValue)
                {
                    return Guid.Empty;
                }
            }

        Finish:
            return foundDepartmentId;
        }

        private async Task AssignTaskAndPermission(WorkflowInstance wfInstance, WorkflowItemDTO wfItem, WorkflowStepDTO nextStep, Guid? userId, Guid? departmentId, List<WorkflowRoleDTO> roleIds, Department rqDepartment)
        {
            var newProcessingTask = new WorkflowProcessing()
            {
                DueDate = DateTime.Now.AddDays(nextStep.DueDateNumber),
                ReferenceNumber = wfItem.ReferenceNumber,
                ItemId = wfItem.ItemId,
                InstanceId = wfInstance.Id,
                Status = wfItem.Status,
                ItemType = wfItem.Type,
                RequestedDepartmentId = rqDepartment?.Id,
                //RequestedDepartmentCode = rqDepartment?.Code,
                RequestedDepartmentName = rqDepartment?.Name,
                StepNumber = nextStep.StepNumber,
                AssignedToUserId = userId,
                AssignedToDepartmentId = departmentId,
                //IsTurnedOffSendNotification = nextStep.IsTurnedOffSendNotification
            };
            if (roleIds.Any())
                newProcessingTask.AssignedToRoleId = roleIds.FirstOrDefault().RoleId;
            _uow.GetRepository<WorkflowProcessing>().Add(newProcessingTask);
            _permissionBusiness.AddPerm(newProcessingTask.Id, departmentId, newProcessingTask.AssignedToRoleId, userId, PermEnum.View);
            _permissionBusiness.AddPerm(wfItem.ItemId, departmentId, newProcessingTask.AssignedToRoleId, userId, PermEnum.View);
        }

        public async Task<ResultDTO> Vote(VoteArgs args)
        {
            var resultDTO = new ResultDTO();
            try
            {
                var item = await GetWorkflowItem(args.ItemId);
                if (item == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.NOT_FOUND_ITEM }, ErrorCodes = new List<int>() { -1 } };
                    goto Finish;
                }

                var wfInstance = await _uow.GetRepository<WorkflowInstance>().GetSingleAsync(x => x.ItemId == args.ItemId, "Created desc");
                if (wfInstance.IsCompleted)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.WORKFLOW_IS_COMPLETED }, ErrorCodes = new List<int>() { -1 } };
                    goto Finish;
                }
                var mapDataWFInstance = _mapper.Map<WorkflowInstanceDTO>(wfInstance);

                var currentUser = GetCurrentUser();
                if (currentUser == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.CANNOT_FIND_CURRENT_USER }, ErrorCodes = new List<int>() { -1 } };
                    goto Finish;
                }

                var userDepartmentMappingDTO = currentUser.UserDepartmentMappingDTO;
                var lastHistoryList = await _uow.GetRepository<WorkflowProcessing>().FindByAsync(x => x.InstanceId == wfInstance.Id, "Created desc");
                lastHistoryList = lastHistoryList.Where(x =>
                (x.AssignedToUserId != null && x.AssignedToUserId.HasValue ? x.AssignedToUserId == currentUser.Id : false) ||
                (x.AssignedToDepartmentId != null && x.AssignedToDepartmentId.HasValue && x.AssignedToRoleId != null ? userDepartmentMappingDTO.Any(s => s.DepartmentId == x.AssignedToDepartmentId && s.RoleIds.Contains(x.AssignedToRoleId.Value)) : false)
                    );
                if (lastHistoryList == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.CANNOT_FIND_ANY_RUNNING_WORKFLOW }, ErrorCodes = new List<int>() { -1 } };
                    goto Finish;
                }
                WorkflowProcessing lastHistory = lastHistoryList.ToList().FirstOrDefault();
                if (lastHistory == null || lastHistory.IsStepCompleted)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.CANNOT_FIND_ANY_RUNNING_WORKFLOW }, ErrorCodes = new List<int>() { -1 } };
                    goto Finish;
                }

                lastHistory.VoteType = (int)args.Vote;
                lastHistory.Modified = DateTime.Now;
                lastHistory.Comment = args.Comment;
                lastHistory.ApproverId = currentUser.Id;
                lastHistory.Approver = currentUser.LoginName;
                lastHistory.ApproverFullName = currentUser.FullName;
                lastHistory.IsStepCompleted = true;
                string resultMessage = null;
                var currentStep = mapDataWFInstance.WorkflowData.WorkflowSteps.FirstOrDefault(x => x.StepNumber == lastHistory.StepNumber);
                if (currentStep == null)
                {
                    resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.CANNOT_FIND_CURRENT_STEP }, ErrorCodes = new List<int>() { -1 } };
                    goto Finish;
                }
                switch (args.Vote)
                {
                    case VoteType.Approve:
                        WorkflowStepDTO nextStep = mapDataWFInstance.WorkflowData.WorkflowSteps.FirstOrDefault(x => x.StepNumber == (currentStep.StepNumber + 1));
                        if (nextStep == null)
                        {
                            // Flow completed
                            wfInstance.IsCompleted = true;
                            await UpdateStatusItem(item.Id, StatusConst.COMPLETED);
                        } else
                        {
                            if ((nextStep.WorkflowRoles == null) && ((nextStep.AssignToDepartmentId == null) && (nextStep.AssignToUserId == null)))
                            {
                                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.CANNOT_FIND_APPROVER_ID_IN_NEXT_STEP }, ErrorCodes = new List<int>() { -1 } };
                                goto Finish;
                            }

                            var findStatus = _uow.GetRepository<Status>().FindById(nextStep.StatusId);
                            if (findStatus == null)
                            {
                                resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.STATUS_NOT_FOUND }, ErrorCodes = new List<int>() { -1 } };
                                goto Finish;
                            }
                            
                            var roleList = nextStep.WorkflowRoles != null ? nextStep.WorkflowRoles.Select(x => x.RoleId).ToList() : [];
                            var findDepartmentPermApproveId = await GetDepartmentPermApprove(lastHistory.RequestedDepartmentId, nextStep.AssignToDepartmentId, roleList);
                            if (findDepartmentPermApproveId == Guid.Empty && nextStep.AssignToUserId == null)
                            {
                                resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CANNOT_FIND_APPROVER_ID_IN_NEXT_STEP }, ErrorCodes = new List<int> { -1 } };
                                goto Finish;
                            }
                            var status = StatusConst.SetStatus(findStatus.Name);
                            var createNextStep = new WorkflowProcessing
                            {
                                InstanceId = mapDataWFInstance.Id,
                                ItemId = mapDataWFInstance.ItemId,
                                RequestedUserId = lastHistory.RequestedUserId,
                                RequestedUserName = lastHistory.RequestedUserName,
                                RequestedDepartmentId = lastHistory.RequestedDepartmentId,
                                RequestedDepartmentName = lastHistory.RequestedDepartmentName,
                                ReferenceNumber = mapDataWFInstance.ItemReferenceNumber,
                                AssignedToDepartmentId = findDepartmentPermApproveId != Guid.Empty ? findDepartmentPermApproveId : null,
                                AssignedToUserId = nextStep.AssignToUserId,
                                AssignedToRoleId = roleList.Any() ? roleList.FirstOrDefault() : null,
                                VoteType = (int)VoteType.None,
                                StepNumber = nextStep.StepNumber,
                                DueDate = DateTimeOffset.Now.AddDays(nextStep.DueDateNumber),
                                ItemType = lastHistory.ItemType,
                                Status = status
                            };
                            await UpdateStatusItem(item.Id, status);

                            bool autoNextStep = false;
                            if (nextStep.AssignToUserId != null && nextStep.AssignToUserId.Value != Guid.Empty)
                            {
                                createNextStep.AssignedToUserId = nextStep.AssignToUserId;
                                if (currentUser.Id == nextStep.AssignToUserId)
                                {
                                    createNextStep.IsStepCompleted = true;
                                    createNextStep.VoteType = (int) VoteType.Approve;
                                    autoNextStep = true;
                                }
                            }
                            else
                            {
                                if (nextStep.WorkflowRoles == null || !nextStep.WorkflowRoles.Any())
                                {
                                    resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.CANNOT_FIND_ROLE_IN_NEXT_STEP }, ErrorCodes = new List<int>() { -1 } };
                                    goto Finish;
                                }

                                findDepartmentPermApproveId = await GetDepartmentPermApprove(lastHistory.RequestedDepartmentId, nextStep.AssignToDepartmentId, (nextStep.WorkflowRoles != null ? nextStep.WorkflowRoles.Select(x => x.RoleId).ToList() : []));
                                bool currentUserInNextStepDepartment = userDepartmentMappingDTO.Any(x => x.DepartmentId == findDepartmentPermApproveId);
                                if (currentUserInNextStepDepartment)
                                {
                                    createNextStep.IsStepCompleted = true;
                                    createNextStep.VoteType = (int)VoteType.Approve;
                                    autoNextStep = true;
                                }
                            }
                            _uow.GetRepository<WorkflowProcessing>().Add(createNextStep);
                            _permissionBusiness.AddPerm(createNextStep.Id, createNextStep.AssignedToDepartmentId, createNextStep.AssignedToRoleId, createNextStep.AssignedToUserId, PermEnum.View);
                            _permissionBusiness.AddPerm(mapDataWFInstance.ItemId, createNextStep.AssignedToDepartmentId, createNextStep.AssignedToRoleId, createNextStep.AssignedToUserId, PermEnum.View);

                            #region Auto next step
                            if (autoNextStep)
                            {
                                nextStep = mapDataWFInstance.WorkflowData.WorkflowSteps.FirstOrDefault(x => x.StepNumber == (nextStep.StepNumber + 1));
                                if (nextStep == null)
                                {
                                    // Flow completed
                                    wfInstance.IsCompleted = true;
                                    await UpdateStatusItem(item.Id, StatusConst.COMPLETED);
                                }
                                else
                                {
                                    findStatus = _uow.GetRepository<Status>().FindById(nextStep.StatusId);
                                    if (findStatus == null)
                                    {
                                        resultDTO = new ResultDTO() { Messages = new List<string>() { MessageConst.WORKFLOW.STATUS_NOT_FOUND }, ErrorCodes = new List<int>() { -1 } };
                                        goto Finish;
                                    }
                                    roleList = nextStep.WorkflowRoles != null ? nextStep.WorkflowRoles.Select(x => x.RoleId).ToList() : [];
                                    findDepartmentPermApproveId = await GetDepartmentPermApprove(lastHistory.RequestedDepartmentId, nextStep.AssignToDepartmentId, roleList);
                                    if (findDepartmentPermApproveId == Guid.Empty && nextStep.AssignToUserId == null)
                                    {
                                        resultDTO = new ResultDTO() { Messages = new List<string> { MessageConst.WORKFLOW.CANNOT_FIND_APPROVER_ID_IN_NEXT_STEP }, ErrorCodes = new List<int> { -1 } };
                                        goto Finish;
                                    }
                                    status = StatusConst.SetStatus(findStatus.Name);
                                    createNextStep = new WorkflowProcessing
                                    {
                                        InstanceId = mapDataWFInstance.Id,
                                        ItemId = mapDataWFInstance.ItemId,
                                        RequestedUserId = lastHistory.RequestedUserId,
                                        RequestedUserName = lastHistory.RequestedUserName,
                                        ReferenceNumber = mapDataWFInstance.ItemReferenceNumber,
                                        RequestedDepartmentId = lastHistory.RequestedDepartmentId,
                                        RequestedDepartmentName = lastHistory.RequestedDepartmentName,
                                        AssignedToDepartmentId = findDepartmentPermApproveId != Guid.Empty ? findDepartmentPermApproveId : null,
                                        AssignedToUserId = nextStep.AssignToUserId,
                                        AssignedToRoleId = roleList.Any() ? roleList.FirstOrDefault() : null,
                                        VoteType = (int) VoteType.None,
                                        IsStepCompleted = false,
                                        StepNumber = nextStep.StepNumber,
                                        DueDate = DateTimeOffset.Now.AddDays(nextStep.DueDateNumber),
                                        ItemType = lastHistory.ItemType,
                                        Status = status
                                    };
                                    _uow.GetRepository<WorkflowProcessing>().Add(createNextStep);
                                    await UpdateStatusItem(item.Id, status);
                                    _permissionBusiness.AddPerm(createNextStep.Id, createNextStep.AssignedToDepartmentId, createNextStep.AssignedToRoleId, createNextStep.AssignedToUserId, PermEnum.View);
                                    _permissionBusiness.AddPerm(mapDataWFInstance.ItemId, createNextStep.AssignedToDepartmentId, createNextStep.AssignedToRoleId, createNextStep.AssignedToUserId, PermEnum.View);
                                }
                            }
                            #endregion
                        }

                        resultMessage = MessageConst.WORKFLOW.APPROVE_WORLFOW_IS_SUCCESSFULLY;
                        break;
                    case VoteType.Reject:
                        wfInstance.IsCompleted = true;
                        resultMessage = MessageConst.WORKFLOW.REJECT_WORLFOW_IS_SUCCESSFULLY;
                        await UpdateStatusItem(item.Id, StatusConst.REJECTED);
                        break;

                    case VoteType.Cancel:
                        wfInstance.IsCompleted = true;
                        resultMessage = MessageConst.WORKFLOW.CANCEL_WORLFOW_IS_SUCCESSFULLY;
                        await UpdateStatusItem(item.Id, StatusConst.CANCELLED);
                        break;
                    case VoteType.RequestToChange:
                        wfInstance.IsTerminated = true;
                        wfInstance.IsCompleted = true;
                        await UpdateStatusItem(item.Id, StatusConst.REQUEST_TO_CHANGE);
                        resultMessage = MessageConst.WORKFLOW.REQUESTTOCHANGE_WORLFOW_IS_SUCCESSFULLY;
                        break;
                    default:
                        break;
                }
                resultDTO.Messages = new List<string>() { resultMessage };
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                resultDTO = new ResultDTO() { Messages = new List<string>() { ex.Message }, ErrorCodes = new List<int>() { -1 } };
                goto Finish;
            }
        Finish:
            return resultDTO;
        }

        /*private async Task<List<WorkflowTemplate>> GetWorkflowTemplates(WorkflowItem item)
        {
            List<WorkflowTemplate> matchedWfTemplates = new List<WorkflowTemplate>();
            var wfTemplates = await _uow.GetRepository<WorkflowTemplate>().FindByAsync(x => x.ItemType == item.Type && x.IsActivated, "order asc");
            var itemPros = await ExtractItemProperties(item.Entity);
            foreach (var wfTemp in wfTemplates)
            {
                try
                {
                    await CheckOverwriteWorkflowData(item, itemPros, wfTemp);
                    if (wfTemp.WorkflowData != null)
                    {
                        var isValid = IsValidCondition(itemPros, wfTemp.WorkflowData.StartWorkflowConditions);
                        if (isValid)
                        {
                            matchedWfTemplates.Add(wfTemp);
                        }
                    }
                }
                catch
                {
                }
            }

            return matchedWfTemplates;
        }*/

        /*private async Task<Department> GetRequestDepartment(Guid userId, Dictionary<string, string> itemPros = null, WorkflowData wfData = null, WorkflowStep nextStep = null)
        {
            Department rqDepartment = null;
            if (wfData != null && wfData.OverwriteRequestedDepartment)
            {
                var field = wfData.RequestedDepartmentField;
                if (nextStep != null && nextStep.OverwriteRequestedDepartment)
                {
                    field = nextStep.RequestedDepartmentField;
                }
                if (itemPros.ContainsKey(field?.Trim().ToLowerInvariant()))
                {
                    if (Guid.TryParse(itemPros[field?.Trim().ToLowerInvariant()], out Guid departmentGuid))
                    {
                        rqDepartment = await _uow.GetRepository<Department>().GetSingleAsync(x => x.Id == departmentGuid, "", x => x.JobGrade);
                    }
                    else
                    {
                        rqDepartment = await _uow.GetRepository<Department>().GetSingleAsync(x => x.Code == field, "", x => x.JobGrade);
                    }
                }
            }
            if (rqDepartment == null)
            {
                rqDepartment = await _uow.GetRepository<Department>().GetSingleAsync(x => x.UserDepartmentMappings.Any(t => t.UserId == userId && t.IsHeadCount), "", x => x.JobGrade);
            }
            return rqDepartment;
        }*/

        /*private bool IsValidCondition(Dictionary<string, string> itemProperties, IList<WorkflowCondition> conditions)
        {
            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    var field = condition.FieldName.ToLowerInvariant();
                    if (itemProperties.ContainsKey(field))
                    {
                        var extractedValue = itemProperties[field]?.Trim().ToLowerInvariant();
                        var isValid = IsValid(condition, extractedValue);
                        if (!isValid)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }*/

        /*private bool IsValid(WorkflowCondition condition, string extractedValue)
        {
            foreach (var conV in condition.FieldValues)
            {
                if (conV?.ToLowerInvariant().Trim() == extractedValue)
                {
                    return true;
                }
            }
            return false;
        }*/

        /*private async Task<Dictionary<string, string>> ExtractItemProperties(WorkflowEntity item)
        {
            var result = new Dictionary<string, string>();
            //Add department values
            var rqDepartment = await GetRequestDepartment(item.CreatedById.Value);
            if (rqDepartment != null)
            {
                result["isassistant"] = Convert.ToString(await _uow.GetRepository<UserDepartmentMapping>().AnyAsync(x => x.UserId == item.CreatedById && x.DepartmentId == rqDepartment.Id));
                result["isstore"] = Convert.ToString(rqDepartment.IsStore);
                result["ishr"] = Convert.ToString(rqDepartment.IsHR);
                result["iscb"] = Convert.ToString(rqDepartment.IsCB);
                result["isPerf"] = Convert.ToString(rqDepartment.IsPerfomance);
                // result["requestedjobgrade"] = Convert.ToString(rqDepartment.JobGrade.Caption);
                result["requestedjobgrade"] = Convert.ToString(rqDepartment.JobGrade.Grade);
                result["requesteddepartment"] = Convert.ToString(rqDepartment.Name);
                result["requesteddepartmentcode"] = Convert.ToString(rqDepartment.Code);
                if (rqDepartment.BusinessModel != null && !string.IsNullOrEmpty(rqDepartment.BusinessModel.Code))
                {
                    result["businessmodelcode"] = Convert.ToString(rqDepartment.BusinessModel.Code);
                }
            }

            var fields = item.GetType().GetProperties();
            foreach (var field in fields)
            {
                result[field.Name.ToLower()] = Convert.ToString(field.GetValue(item));
            }
            return result;
        }*/

        /*private async Task<Department> GetUpperDept(WorkflowHistory lastHistory, WorkflowStep nextStep, Department rqDepartment)
        {
            Department indexDept = rqDepartment;
            if (lastHistory != null && !nextStep.TraversingFromRoot)
            {
                if (lastHistory.AssignedToDepartmentId.HasValue || lastHistory.AssignedToUserId.HasValue)
                {
                    indexDept = await _uow.GetRepository<Department>().GetSingleAsync(x => x.Id == lastHistory.AssignedToDepartmentId || x.UserDepartmentMappings.Any(u => u.UserId == lastHistory.AssignedToUserId && u.IsHeadCount));
                }
                if (indexDept == null)
                {
                    indexDept = rqDepartment;
                }
            }
            if (indexDept != null)
            {
                bool skip = false;
                var jobGrades = await _uow.GetRepository<JobGrade>().GetAllAsync();
                var nextStepJobGrade = jobGrades.FirstOrDefault(x => x.Grade == int.Parse(nextStep.JobGrade));
                var nextStepMaxJobGrade = jobGrades.FirstOrDefault(x => x.Grade == int.Parse(nextStep.MaxJobGrade));
                var currentJobGrade = jobGrades.FirstOrDefault(x => x.Id == indexDept.JobGradeId);
                if (nextStep.ReverseJobGrade)
                {
                    var depts = new Dictionary<int, Guid>();
                    depts.Add(currentJobGrade.Grade, indexDept.Id);
                    while (!skip)
                    {
                        if (indexDept.ParentId.HasValue)
                        {
                            indexDept = await _uow.GetRepository<Department>().GetSingleAsync(x => x.Id == indexDept.ParentId);
                            if (indexDept != null)
                            {
                                var indexJobGrade = jobGrades.FirstOrDefault(x => x.Id == indexDept.JobGradeId);
                                if (indexJobGrade.Grade > nextStepMaxJobGrade.Grade)
                                {
                                    skip = true;
                                }
                                else
                                {
                                    if (!depts.ContainsKey(indexJobGrade.Grade))
                                    {
                                        depts.Add(indexJobGrade.Grade, indexDept.Id);
                                    }
                                }
                            }
                            else
                            {
                                skip = true;
                            }
                        }
                        else
                        {
                            skip = true;
                        }
                    }
                    var jobGrade = nextStepMaxJobGrade.Grade;
                    for (var i = jobGrade; i >= nextStepJobGrade.Grade; i--)
                    {
                        if (depts.ContainsKey(i))
                        {
                            //Check next department type
                            var deptId = depts[i];
                            var hasParticipants = await _uow.GetRepository<UserDepartmentMapping>().AnyAsync(x => x.DepartmentId == deptId && (nextStep.DepartmentType == x.Role || (x.Department.IsPerfomance && (Group.Member == x.Role || Group.Checker == x.Role))));
                            if (hasParticipants)
                            {

                                indexDept = await _uow.GetRepository<Department>().GetSingleAsync(x => x.Id == deptId);
                                return indexDept;
                            }
                            else
                            {
                                _refDeparmentId = indexDept.Id;
                            }
                        }
                    }
                    return null;
                }
                else
                {
                    if (nextStep.IncludeCurrentNode && currentJobGrade.Grade >= nextStepJobGrade.Grade && currentJobGrade.Grade <= nextStepMaxJobGrade.Grade)
                    {
                        var hasParticipants = await _uow.GetRepository<UserDepartmentMapping>().AnyAsync(x => x.DepartmentId == indexDept.Id && (nextStep.DepartmentType == x.Role || (x.Department.IsPerfomance && (Group.Member == x.Role || Group.Checker == x.Role))));
                        if (hasParticipants)
                        {
                            return indexDept;
                        }
                        else
                        {
                            _refDeparmentId = indexDept.Id;
                        }
                    }
                    while (!skip)
                    {
                        if (indexDept.ParentId.HasValue)
                        {
                            indexDept = await _uow.GetRepository<Department>().GetSingleAsync(x => x.Id == indexDept.ParentId);
                            if (indexDept != null)
                            {
                                var indexJobGrade = jobGrades.FirstOrDefault(x => x.Id == indexDept.JobGradeId);
                                if (indexJobGrade.Grade >= nextStepJobGrade.Grade)
                                {
                                    if (indexJobGrade.Grade > nextStepMaxJobGrade.Grade)
                                    {
                                        return null;
                                    }
                                    //If next step is large than department type, get the next step
                                    if (indexJobGrade.Grade > nextStepJobGrade.Grade)
                                    {
                                        nextStep.DepartmentType = nextStep.NextDepartmentType;
                                    }
                                    //Check next department type
                                    var hasParticipants = await _uow.GetRepository<UserDepartmentMapping>().AnyAsync(x => x.DepartmentId == indexDept.Id && (nextStep.DepartmentType == x.Role || (x.Department.IsPerfomance && (Group.Member == x.Role || Group.Checker == x.Role))));
                                    if (hasParticipants)
                                    {
                                        return indexDept;
                                    }
                                    else
                                    {
                                        _refDeparmentId = indexDept.Id;
                                    }
                                }
                            }
                            else
                            {
                                if (indexDept.JobGrade.Grade == 9)
                                {
                                    return indexDept;
                                }
                                return null;
                            }
                        }
                        else
                        {
                            if (indexDept.JobGrade.Grade == 9)
                            {
                                return indexDept;
                            }
                        }
                        if (indexDept == null || !indexDept.ParentId.HasValue)
                        {
                            return null;
                        }
                    }
                }
            }
            return indexDept;
        }*/



        /*private async Task AssignTaskAndPermission(WorkflowInstance wfInstance, *//*WorkflowItem wfItem,*//* WorkflowStep nextStep, *//*WorkflowEntity item,*//* Guid? userId, Guid? departmentId, Department rqDepartment)
        {
            *//*var wfHistories = await _uow.GetRepository<WorkflowHistory>().FindByAsync(x => x.Instance.ItemId == wfItem.Entity.Id);
            if (userId.HasValue || departmentId.HasValue)
            {
                Func<Group, bool> DoesHaveParticipants = (Group groupType) =>
                {
                    bool checkStatus = _uow.GetRepository<UserDepartmentMapping>().Any(x => x.DepartmentId == departmentId.Value && (nextStep.ParticipantType == ParticipantType.HRDepartment && ((x.User.Role & UserRole.HR) == UserRole.HR || (x.User.Role & UserRole.HRAdmin) == UserRole.HRAdmin)) && groupType == x.Role);
                    return checkStatus;
                };

                //Ignore task list and new permission for appover
                //Create new task list
                var hasParticipants = true;
                var customDepartmentType = nextStep.DepartmentType;

                if (departmentId.HasValue)
                {
                    hasParticipants = DoesHaveParticipants(nextStep.DepartmentType);
                    if (hasParticipants)
                    {
                        customDepartmentType = nextStep.DepartmentType;
                    }
                    else
                    {
                        customDepartmentType = DoesHaveParticipants(nextStep.NextDepartmentType) ? nextStep.NextDepartmentType : nextStep.DepartmentType;
                    }

                    var deprt = await _uow.GetRepository<Department>().GetSingleAsync(x => x.Id == departmentId);
                    if (!hasParticipants && deprt.IsPerfomance)
                    {
                        if (nextStep.DepartmentType == Group.Checker)
                        {
                            nextStep.DepartmentType = Group.Member;
                        }
                        else
                            nextStep.DepartmentType = Group.Checker;

                    }
                }

                var newTask = new WorkflowTask()
                {
                    DueDate = DateTime.Now.AddDays(nextStep.DueDateNumber),
                    Title = item.ReferenceNumber,
                    ItemId = item.Id,
                    WorkflowInstanceId = wfInstance.Id,
                    Status = item.Status,
                    ItemType = wfItem.Type,
                    RequestorId = item.Id,
                    RequestorFullName = item.CreatedByFullName,
                    RequestorUserName = item.CreatedBy,
                    RequestedDepartmentId = rqDepartment?.Id,
                    RequestedDepartmentCode = rqDepartment?.Code,
                    RequestedDepartmentName = rqDepartment?.Name,
                    ReferenceNumber = item.ReferenceNumber,
                    AssignedToId = userId,
                    AssignedToDepartmentId = departmentId,
                    AssignedToDepartmentGroup = customDepartmentType,
                };
                nextStep.DepartmentType = customDepartmentType;
                _uow.GetRepository<WorkflowTask>().Add(newTask);

            }*/
        /*else if (!nextStep.IgnoreIfNoParticipant)
        {

        }
        _uow.GetRepository<Permission>().Add(permissions);*//*
    }

    *//* public async Task<ResultDTO> Vote(VoteArgs args)
    {
        try
        {
            var item = await GetWorkflowItem(args.ItemId);
            if (item == null)
            {
                return new ResultDTO()
                {
                    Messages = new List<string>() {
                    "Item not found!"

                }
                };
            }
            var wfInstance = await _uow.GetRepository<WorkflowInstance>().GetSingleAsync(x => x.ItemId == args.ItemId, "Created desc");
            if (wfInstance.IsCompleted)
            {
                return new ResultDTO()
                {
                    Messages = new List<string>() {
                    "Workflow is completed!"

                }
                };
            }
            var lastHistory = await _uow.GetRepository<WorkflowHistory>().GetSingleAsync(x => x.InstanceId == wfInstance.Id, "Created desc");
            if (lastHistory == null || lastHistory.IsStepCompleted)
            {
                return new ResultDTO()
                {
                    Messages = new List<string>() {
                        "Cannot find any running workflow !"
                    }
                };
            }

            var ignorePerm = args.Vote == VoteType.Cancel && item.Entity.CreatedById == _uow.UserContext.CurrentUserId;
            var currentTask = await _uow.GetRepository<WorkflowTask>(ignorePerm).GetSingleAsync(x => x.ItemId == item.Entity.Id && x.WorkflowInstanceId == wfInstance.Id && !x.IsCompleted, "Created desc");

            //If user cancel incase of pending, the tasks was not generated
            if (currentTask == null && args.Vote != VoteType.Cancel)
            {
                return new ResultDTO()
                {
                    Messages = new List<string>() {
                        "Cannot find any running workflow !"
                    }
                };
            }
            if (currentTask != null)
            {
                currentTask.IsCompleted = true;
                currentTask.Vote = args.Vote;
            }
            var currentStep = wfInstance.WorkflowData.Steps.FirstOrDefault(x => x.StepNumber == lastHistory.StepNumber);
            lastHistory.VoteType = args.Vote;
            lastHistory.Modified = DateTime.Now;
            lastHistory.Comment = args.Comment;
            lastHistory.ApproverId = _uow.UserContext.CurrentUserId;
            lastHistory.Approver = _uow.UserContext.CurrentUserName;
            lastHistory.ApproverFullName = _uow.UserContext.CurrentUserFullName;
            lastHistory.IsStepCompleted = true;
            WorkflowStep nextStep = null;
            if (args.Vote == VoteType.Approve)
            {
                lastHistory.Outcome = currentStep.OnSuccess;
                nextStep = wfInstance.WorkflowData.Steps.GetNextStep(currentStep, item.Entity, item.Entity.GetType());
            }
            else if (args.Vote == VoteType.Reject)
            {
                lastHistory.Outcome = currentStep.OnFailure;
            }
            else if (args.Vote == VoteType.Cancel)
            {
                if (string.IsNullOrEmpty(wfInstance.WorkflowData.OnCancelled))
                {
                    lastHistory.Outcome = "Cancelled";
                }
                else
                {
                    lastHistory.Outcome = wfInstance.WorkflowData.OnCancelled;
                }
            }
            await UpdatePermission(item.Entity.Id, Right.View);
            await _uow.CommitAsync();
            if (args.Vote == VoteType.RequestToChange)
            {
                List<string> statusAllowStartRevoke = new List<string>() { "completed", "revoke - request to change" };
                item.Entity.Status = !string.IsNullOrEmpty(wfInstance.WorkflowData.onRequestToChange) ? wfInstance.WorkflowData.onRequestToChange : "Requested To Change";
                wfInstance.IsTerminated = true;
                wfInstance.IsCompleted = true;
                //Send email notification
                await SendEmailNotificationForCreator(EmailTemplateName.ForCreatorRequestToChange, item);
                if (currentStep.ReturnToStepNumber == 0)
                {
                    if (item.Entity.Status.Equals("revoke - request to change", StringComparison.OrdinalIgnoreCase))
                    {
                        await AssignPermissionToUser(item.Entity.Id, item.Entity.CreatedById.Value, Right.View);
                    }
                    else
                    {
                        await AssignPermissionToUser(item.Entity.Id, item.Entity.CreatedById.Value, Right.Full);
                    }
                    await _uow.CommitAsync();
                }
                else
                {
                    var newInstance = new WorkflowInstance()
                    {
                        ItemId = wfInstance.ItemId,
                        ItemReferenceNumber = wfInstance.ItemReferenceNumber,
                        TemplateId = wfInstance.TemplateId,
                        WorkflowData = wfInstance.WorkflowData,
                        WorkflowName = wfInstance.WorkflowName,
                        DefaultCompletedStatus = wfInstance.DefaultCompletedStatus
                    };
                    _uow.GetRepository<WorkflowInstance>().Add(newInstance);
                    var histories = wfInstance.Histories.Where(x => x.StepNumber < currentStep.ReturnToStepNumber).ToList();
                    foreach (var history in histories)
                    {
                        var wfHistory = new WorkflowHistory()
                        {
                            Approver = history.Approver,
                            ApproverFullName = history.ApproverFullName,
                            ApproverId = history.ApproverId,
                            AssignedToDepartmentId = history.AssignedToDepartmentId,
                            AssignedToDepartmentType = history.AssignedToDepartmentType,
                            AssignedToUserId = history.AssignedToUserId,
                            Comment = history.Comment,
                            DueDate = history.DueDate,
                            InstanceId = newInstance.Id,
                            IsStepCompleted = history.IsStepCompleted,
                            Outcome = history.Outcome,
                            StepNumber = history.StepNumber,
                            VoteType = history.VoteType
                        };
                        _uow.GetRepository<WorkflowHistory>().Add(wfHistory);
                    }
                    await _uow.CommitAsync();
                    nextStep = newInstance.WorkflowData.Steps.FirstOrDefault(x => x.StepNumber == currentStep.ReturnToStepNumber);
                    lastHistory = await _uow.GetRepository<WorkflowHistory>().GetSingleAsync(x => x.InstanceId == wfInstance.Id && x.StepNumber < currentStep.ReturnToStepNumber, "Created desc");
                    await RequestToChangeAction(item, currentStep.StepName);
                    await ProcessNextStep(newInstance, item, lastHistory, nextStep);
                }
            }
            else
            {
                await ProcessNextStep(wfInstance, item, lastHistory, nextStep);

                #region Save Department of Next Step For Send Daily Mail
                bool isResignationItem = typeof(ResignationApplication).IsAssignableFrom(item.Entity.GetType());
                if (isResignationItem && currentStep.StepNumber == 1 && currentStep.OnSuccess.Equals("Submitted", StringComparison.OrdinalIgnoreCase)) // submit
                {
                    WorkflowStep nxStep = wfInstance.WorkflowData.Steps.GetNextStep(nextStep, item.Entity, item.Entity.GetType());
                    await ProcessUpdateNextDepartment(wfInstance, item, lastHistory, nxStep);
                }
                #endregion

                #region Push Acting to SAP at step Acting Employee Confirmation
                bool isActingItem = typeof(Acting).IsAssignableFrom(item.Entity.GetType());
                if (isActingItem && currentStep.OnSuccess.Equals("Accepted Target", StringComparison.OrdinalIgnoreCase))
                {
                    await _exBO.SubmitData(item.Entity, true);
                }

                #endregion
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return new ResultDTO()
            { Messages = new List<string>() { ex.Message } };
        }
        return new ResultDTO()
        { Object = true };
    }*/
    }
}
