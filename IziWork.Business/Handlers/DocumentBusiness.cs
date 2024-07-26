using AutoMapper;
using IziWork.Business.Args;

using IziWork.Business.DTO;

using IziWork.Business.Interfaces;

using IziWork.Data.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
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
using static System.Runtime.InteropServices.JavaScript.JSType;
using Core.Repositories.Business.IRepositories;
using Core.Repositories.Business.Enum;
using Core.Repositories.Business.Interface;
using IziWork.Common.Constans;
using IziWork.Common.Enums;
using IziWork.Common.DTO;
using IziWork.Common.Args;

namespace IziWork.Business.Handlers
{
    public class DocumentBusiness : IDocumentBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IPermissionBusiness _permissionBusiness;
        public DocumentBusiness(IUnitOfWork uow, IMapper mapper, IPermissionBusiness permissionBusiness)
        {
            _uow = uow;
            _mapper = mapper;
            _permissionBusiness = permissionBusiness;
        }

        public async Task<ResultDTO> GetCurrentReferenceNumberDocument(string type)
        {
            var resultDTO = new ResultDTO() { };
            var newRef = _uow.GetRepository<ReferenceDocument>().GenerateNewReferenceNumber(type);
            if (string.IsNullOrEmpty(newRef))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_TYPE_ITEM }, ErrorCodes = new List<int> { -1 } };
            }
            resultDTO.Object = newRef;
            return resultDTO;
        }

        #region INSERT - UPDATE - DELETE
        public async Task<ResultDTO> CreateDocument(DocumentArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }

            if ((args.RegistryId == null || args.RegistryId == Guid.Empty) && (!args.IsSaveDraft))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_REGISTRY }, ErrorCodes = new List<int> { -1 } };
            }
            var findRegistry = _uow.GetRepository<CategoryDetail>().GetSingle(x => x.Id == args.RegistryId && ((int)DefineEnums.CATEGORY_TYPE.Registry) == x.Category.Type);
            if (findRegistry == null && (!args.IsSaveDraft))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_REGISTRY }, ErrorCodes = new List<int> { -1 } };
            }

            if ((args.DocumentTypeId == null || args.DocumentTypeId == Guid.Empty) && (!args.IsSaveDraft))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_DOCUMENT }, ErrorCodes = new List<int> { -1 } };
            }
            var findDocumentType = _uow.GetRepository<CategoryDetail>().GetSingle(x => x.Id == args.DocumentTypeId && ((int)DefineEnums.CATEGORY_TYPE.DocumentType) == x.Category.Type);
            if (findDocumentType == null && (!args.IsSaveDraft))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_DOCUMENT }, ErrorCodes = new List<int> { -1 } };
            }

            Department sendingDepartment = null;
            if (args.SendingDepartmentId != null && args.SendingDepartmentId.HasValue)
            {
                sendingDepartment = _uow.GetRepository<Department>().GetSingle(x => x.Id == args.SendingDepartmentId);
                if (sendingDepartment == null)
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_SENDING_DEPARTMENT }, ErrorCodes = new List<int> { -1 } };
                }
            }

            if ((args.SecurityLevelId == null || args.SecurityLevelId == Guid.Empty) && (!args.IsSaveDraft))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_SECURITY_LEVEL }, ErrorCodes = new List<int> { -1 } };
            }
            var findSecurityLevel = _uow.GetRepository<CategoryDetail>().GetSingle(x => x.Id == args.SecurityLevelId && ((int)DefineEnums.CATEGORY_TYPE.SecurityLevel) == x.Category.Type);
            if (findSecurityLevel == null && (!args.IsSaveDraft))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_SECURITY_LEVEL }, ErrorCodes = new List<int> { -1 } };
            }

            if ((args.UrgencyLevelId == null || args.UrgencyLevelId == Guid.Empty) && (!args.IsSaveDraft))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_URGENCY_LEVEL }, ErrorCodes = new List<int> { -1 } };
            }
            var findUrgencyLevel = _uow.GetRepository<CategoryDetail>().GetSingle(x => x.Id == args.UrgencyLevelId && ((int)DefineEnums.CATEGORY_TYPE.UrgencyLevel) == x.Category.Type);
            if (findUrgencyLevel == null && (!args.IsSaveDraft))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_URGENCY_LEVEL }, ErrorCodes = new List<int> { -1 } };
            }

            if (string.IsNullOrEmpty(args.ReferenceNumber))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.REFERENCE_NUMBER_IS_REQUIRED }, ErrorCodes = new List<int> { -1 } };
            }

            var currentRef = await GetCurrentReferenceNumberDocument("Document");
            if (args.ReferenceNumber != currentRef.Object.ToString())
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.REFERENCE_NUMBER_HAS_CHANGED }, ErrorCodes = new List<int> { -1 } };
            }

            var data = _mapper.Map<Document>(args);
            if (sendingDepartment != null)
            {
                data.SendingDepartmentCode = sendingDepartment.Code;
                data.SendingDepartmentName = sendingDepartment.Name;
            }
            var dataCreated = _uow.GetRepository<Document>().Add(data);

            if (args.ReceivingDepartments != null && args.ReceivingDepartments.Any())
            {
                foreach (var deptId in args.ReceivingDepartments)
                {
                    var findDept = _uow.GetRepository<Department>().FindById(deptId);
                    if (findDept != null)
                    {
                        var newDocumentAttachmentMapping = new ReceivingDepartmentDocument()
                        {
                            DocumentId = dataCreated.Id,
                            DepartmentId = findDept.Id
                        };
                        _uow.GetRepository<ReceivingDepartmentDocument>().Add(newDocumentAttachmentMapping);
                        _permissionBusiness.AddPerm(data.Id, findDept.Id, CONST.ROLE.RECORDSCLERK_ID, null, PermEnum.View);
                    }
                }
            }

            if (args.AttachmentFileIds != null && args.AttachmentFileIds.Any())
            {
                foreach (var attachmentFileId in args.AttachmentFileIds) {
                    var findAttachment = _uow.GetRepository<AttachmentFile>().GetSingle(x => x.Id == attachmentFileId);
                    if (findAttachment != null)
                    {
                        var newDocumentAttachmentMapping = new DocumentAttachmentMapping()
                        {
                            AttachmentFileId = findAttachment.Id,
                            DocumentId = dataCreated.Id,
                            Type = 1 // loai default
                        };
                        _uow.GetRepository<DocumentAttachmentMapping>().Add(newDocumentAttachmentMapping);
                    }
                } 
            }
            _permissionBusiness.AddPerm(data.Id, null, null, data.CreatedById, PermEnum.Edit);

            await _uow.CommitAsync();
            resultDTO = new ResultDTO()
            {
                Messages = new List<string> { MessageConst.CREATED_SUCCESSFULLY },
                Object = _mapper.Map<DocumentDTO>(dataCreated)
            };

            return resultDTO;
        }

        public async Task<ResultDTO> UpdateDocument(DocumentArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Id == null || args.Id.HasValue && args.Id.Value == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var currentDocument = await _uow.GetRepository<Document>().GetSingleAsync(y => y.Id.Equals(args.Id));
            if (currentDocument == null)
            {
                return new ResultDTO { ErrorCodes = { 1001 }, Messages = { MessageConst.NOT_FOUND_ITEM } };
            }

            if (args.RegistryId != null && args.RegistryId != Guid.Empty)
            {
                var findRegistry = _uow.GetRepository<CategoryDetail>().GetSingle(x => x.Id == args.RegistryId.Value && ((int) DefineEnums.CATEGORY_TYPE.Registry) == x.Category.Type);
                if (findRegistry == null)
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_REGISTRY }, ErrorCodes = new List<int> { -1 } };
                }
                currentDocument.RegistryId = findRegistry.Id;
            }

            if (args.DocumentTypeId != null && args.DocumentTypeId != Guid.Empty)
            {
                var findDocumentType = _uow.GetRepository<CategoryDetail>().GetSingle(x => x.Id == args.DocumentTypeId && ((int)DefineEnums.CATEGORY_TYPE.DocumentType) == x.Category.Type);
                if (findDocumentType == null)
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_DOCUMENT }, ErrorCodes = new List<int> { -1 } };
                }
                currentDocument.DocumentTypeId = findDocumentType.Id;
            }

            Department sendingDepartment = null;
            if (args.SendingDepartmentId != null && args.SendingDepartmentId.HasValue)
            {
                sendingDepartment = _uow.GetRepository<Department>().GetSingle(x => x.Id == args.SendingDepartmentId);
                if (sendingDepartment == null)
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_SENDING_DEPARTMENT }, ErrorCodes = new List<int> { -1 } };
                }
                currentDocument.SendingDepartmentId = sendingDepartment.Id;
                currentDocument.SendingDepartmentCode = sendingDepartment.Code;
                currentDocument.SendingDepartmentName = sendingDepartment.Name;
            }

            if (args.SecurityLevelId != null && args.SecurityLevelId != Guid.Empty)
            {
                var findSecurityLevel = _uow.GetRepository<CategoryDetail>().GetSingle(x => x.Id == args.SecurityLevelId && ((int)DefineEnums.CATEGORY_TYPE.SecurityLevel) == x.Category.Type);
                if (findSecurityLevel == null)
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_SECURITY_LEVEL }, ErrorCodes = new List<int> { -1 } };
                }
                currentDocument.SecurityLevelId = findSecurityLevel.Id;
            }

            if (args.UrgencyLevelId != null && args.UrgencyLevelId == Guid.Empty)
            {
                var findUrgencyLevel = _uow.GetRepository<CategoryDetail>().GetSingle(x => x.Id == args.UrgencyLevelId && ((int)DefineEnums.CATEGORY_TYPE.UrgencyLevel) == x.Category.Type);
                if (findUrgencyLevel == null)
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_URGENCY_LEVEL }, ErrorCodes = new List<int> { -1 } };
                }
                currentDocument.UrgencyLevelId = findUrgencyLevel.Id;
            }

            if (!string.IsNullOrEmpty(args.Name))
            {
                currentDocument.Name = args.Name;
            }

            if (args.IssueDate != null && args.IssueDate.HasValue)
            {
                currentDocument.IssueDate = args.IssueDate.Value;
            }

            if (args.ExpectedDate != null && args.ExpectedDate.HasValue)
            {
                currentDocument.ExpectedDate = args.ExpectedDate.Value;
            }

            if (!string.IsNullOrEmpty(args.ArrivalNumber))
            {
                currentDocument.ArrivalNumber = args.ArrivalNumber;
            }

            if (args.ArrivalDate != null && args.ArrivalDate.HasValue)
            {
                currentDocument.ArrivalDate = args.ArrivalDate.Value;
            }

            if (args.IsDirect != null && args.IsDirect.HasValue)
            {
                currentDocument.IsDirect = args.IsDirect.Value;
            }

            if (args.IsEmail != null && args.IsEmail.HasValue)
            {
                currentDocument.IsEmail = args.IsEmail.Value;
            }

            if (args.IsFax != null && args.IsFax.HasValue)
            {
                currentDocument.IsFax = args.IsFax.Value;
            }

            if (args.IsPostOffice != null && args.IsPostOffice.HasValue)
            {
                currentDocument.IsPostOffice = args.IsPostOffice.Value;
            }

            if (!string.IsNullOrEmpty(args.SentBy))
            {
                currentDocument.SentBy = args.SentBy;
            }

            if (args.DocumentDate != null && args.DocumentDate.HasValue)
            {
                currentDocument.DocumentDate = args.DocumentDate.Value;
            }

            if (args.BanBo != null && args.BanBo.HasValue)
            {
                currentDocument.BanBo = args.BanBo.Value;
            }

            if (args.PageCount != null && args.PageCount.HasValue)
            {
                currentDocument.PageCount = args.PageCount.Value;
            }

            if (!string.IsNullOrEmpty(args.Description))
            {
                currentDocument.Description = args.Description;
            }
            currentDocument = _uow.GetRepository<Document>().Update(currentDocument);

            if (args.AttachmentFileIds != null && args.AttachmentFileIds.Any())
            {
                ClearAttachmentFiles(currentDocument.Id);
                foreach (var attachmentFileId in args.AttachmentFileIds)
                {
                    var findAttachment = _uow.GetRepository<AttachmentFile>().GetSingle(x => x.Id == attachmentFileId);
                    if (findAttachment != null)
                    {
                        var newDocumentAttachmentMapping = new DocumentAttachmentMapping()
                        {
                            AttachmentFileId = findAttachment.Id,
                            DocumentId = currentDocument.Id,
                            Type = 1 // loai default
                        };
                        _uow.GetRepository<DocumentAttachmentMapping>().Add(newDocumentAttachmentMapping);
                    }
                }
            } else ClearAttachmentFiles(currentDocument.Id);

            if (args.ReceivingDepartments != null && args.ReceivingDepartments.Any())
            {
                ClearRecevingDepartmentDocument(currentDocument.Id);
                foreach (var deptId in args.ReceivingDepartments)
                {
                    var findDept = _uow.GetRepository<Department>().FindById(deptId);
                    if (findDept != null)
                    {
                        var newDocumentAttachmentMapping = new ReceivingDepartmentDocument()
                        {
                            DocumentId = currentDocument.Id,
                            DepartmentId = findDept.Id
                        };
                        _uow.GetRepository<ReceivingDepartmentDocument>().Add(newDocumentAttachmentMapping);
                        _permissionBusiness.AddPerm(currentDocument.Id, findDept.Id, CONST.ROLE.RECORDSCLERK_ID, null, PermEnum.View);
                    }
                }
            } else ClearRecevingDepartmentDocument(currentDocument.Id);

            await _uow.CommitAsync();
            resultDTO = new ResultDTO()
            {
                Messages = new List<string> { MessageConst.UPDATE_SUCCESSFULLY },
                Object = _mapper.Map<DocumentDTO>(currentDocument)
            };

            return resultDTO;
        }

        public void ClearAttachmentFiles(Guid documentId)
        {
            var findDocumentAttachmentMapping = _uow.GetRepository<DocumentAttachmentMapping>().FindBy(x => x.DocumentId == documentId);
            if (findDocumentAttachmentMapping != null)
                _uow.GetRepository<DocumentAttachmentMapping>().Delete(findDocumentAttachmentMapping);
        }

        public void ClearRecevingDepartmentDocument(Guid documentId)
        {
            var findDocumentAttachmentMapping = _uow.GetRepository<ReceivingDepartmentDocument>().FindBy(x => x.DocumentId == documentId);
            if (findDocumentAttachmentMapping != null)
                _uow.GetRepository<ReceivingDepartmentDocument>().Delete(findDocumentAttachmentMapping);
        }

        public async Task<ResultDTO> DeleteById(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<Document>().GetSingleAsync(y => y.Id.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { MessageConst.NOT_FOUND_ITEM } };
            }
            else
            {
                _uow.GetRepository<Document>().Delete(existing);
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
        public async Task<ResultDTO> GetList(QueryArgs args)
        {
            var jobgradeList = await _uow.GetRepository<Document>().FindByAsync<DocumentDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<Document>().CountAsync(args.Predicate, args.PredicateParameters);
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = jobgradeList,
                    Count = totalList
                }
            };
        }

        public async Task<ResultDTO> GetById(Guid Id)
        {
            var findItem = await _uow.GetRepository<Document>().GetSingleAsync<DocumentDTO>(x => x.Id == Id);
            if (findItem == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_ITEM }, ErrorCodes = new List<int> { -1 } };
            }
            return new ResultDTO { Object = findItem };
        }
        #endregion

        public async Task<ResultDTO> LinkDocuments(LinkDocumentArgs args)
        {
            var resultDTO = new ResultDTO() { };
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.DocumentId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var currentDocument = await _uow.GetRepository<Document>().GetSingleAsync(y => y.Id.Equals(args.DocumentId));
            if (currentDocument == null)
            {
                return new ResultDTO { ErrorCodes = { 1001 }, Messages = { MessageConst.NOT_FOUND_ITEM } };
            }

            if (!args.ReferenceDocumentIds.Any())
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.NOT_FOUND_REFERENCEDOCUMENTIDS }, ErrorCodes = new List<int> { -1 } };
            }

            var countRefernceDocument = _uow.GetRepository<Document>().Count(x => args.ReferenceDocumentIds.Contains(x.Id));
            if (countRefernceDocument != args.ReferenceDocumentIds.Count)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.DOCUMENT.REFERENCEDOCUMENTIDS_INVALID }, ErrorCodes = new List<int> { -1 } };
            }
            var findOldRef = await _uow.GetRepository<ReferenceDocument>().FindByAsync(x => x.DocumentId == args.DocumentId);
            #region Remove old item
            if (findOldRef != null && findOldRef.Any()) foreach (var oldtem in findOldRef) _uow.GetRepository<ReferenceDocument>().Delete(oldtem);
            #endregion

            var refDocumentDTO = new List<ReferenceDocumentDTO>();
            foreach (var refDocumentId in args.ReferenceDocumentIds)
            {
                var findRef = _uow.GetRepository<Document>().GetSingle(x => x.Id == refDocumentId);
                var addReferenceDocument = new ReferenceDocument()
                {
                    DocumentId = currentDocument.Id,
                    ReferenceDocId = findRef.Id
                };
                var newRef =  _uow.GetRepository<ReferenceDocument>().Add(addReferenceDocument);
                if (newRef != null) refDocumentDTO.Add(newRef.Adapt<ReferenceDocumentDTO>()); 
            }
            await _uow.CommitAsync();
            resultDTO.Object = refDocumentDTO;
            return resultDTO;
        }

        public async Task<ResultDTO> GetListReferenceDocumentByDocumentId(Guid DocumentId)
        {
            var findItem = await _uow.GetRepository<ReferenceDocument>().FindByAsync<ReferenceDocumentDTO>(x => x.DocumentId == DocumentId);
            if (findItem == null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_ITEM }, ErrorCodes = new List<int> { -1 } };
            }
            return new ResultDTO { Object = findItem };
        }

        public async Task<ResultDTO> DocumentForwarding(DocumentForwardingArgs args)
        {
            var resultDTO = new ResultDTO() { };
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.DocumentId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var currentDocument = await _uow.GetRepository<Document>().GetSingleAsync(y => y.Id.Equals(args.DocumentId));
            if (currentDocument == null)
            {
                return new ResultDTO { ErrorCodes = { 1001 }, Messages = { MessageConst.NOT_FOUND_ITEM } };
            }

            if ((args.ProcessorId == null || !args.ProcessorId.HasValue) && (args.DepartmentId == null || !args.DepartmentId.HasValue))
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            User processor = null;
            if ((args.ProcessorId != null && args.ProcessorId.HasValue))
            {
                processor = _uow.GetRepository<User>().GetSingle(x => x.Id == args.ProcessorId);
                if (processor == null)
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PROCESSOR }, ErrorCodes = new List<int> { -1 } };
                }
                _permissionBusiness.RemovePermissonEdit(currentDocument.Id);
                _permissionBusiness.AddPerm(currentDocument.Id, null, null, processor.Id, PermEnum.Edit);
            }

            Department departmentProcess = null;
            if ((args.DepartmentId != null && args.DepartmentId.HasValue))
            {
                departmentProcess = _uow.GetRepository<Department>().GetSingle(x => x.Id == args.DepartmentId);
                if (departmentProcess == null)
                {
                    return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_DEPARTMENT }, ErrorCodes = new List<int> { -1 } };
                }
                _permissionBusiness.RemovePermissonEdit(currentDocument.Id);
                _permissionBusiness.AddPerm(currentDocument.Id, departmentProcess.Id, CONST.ROLE.MEMBER_ID, null, PermEnum.Edit);
            }

            var findLastDocumentForwarding = await _uow.GetRepository<DocumentForwarding>().GetSingleAsync(x => x.DocumentId == args.DocumentId && x.Action == (int) ActionEnum.UPDATE);
            if (findLastDocumentForwarding != null) {
                findLastDocumentForwarding.Action = (int)ActionEnum.VIEW;
                _uow.GetRepository<DocumentForwarding>().Update(findLastDocumentForwarding);
            }

            var addDocumentForwarding = new DocumentForwarding()
            {
                DocumentId = currentDocument.Id,
                ProcessorId = processor?.Id,
                ProcessorName = processor?.FullName,
                DepartmentId = departmentProcess?.Id,
                DepartmentCode = departmentProcess?.Code,
                DepartmentName = departmentProcess?.Name,
                Action = (int) ActionEnum.UPDATE
            };
            var newDocumentForwarding = _uow.GetRepository<DocumentForwarding>().Add(addDocumentForwarding);
            await _uow.CommitAsync();
            resultDTO.Object = _mapper.Map<DocumentForwardingDTO>(newDocumentForwarding);
            return resultDTO;
        }

        public async Task<ResultDTO> DocumentAssignTask(DocumentAssignTaskArgs args)
        {
            var resultDTO = new ResultDTO() { };
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.DocumentId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.ProcessorId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var findProcessor = _uow.GetRepository<User>().GetSingle(x => x.Id == args.ProcessorId);
            if (findProcessor == null)
            {
                return new ResultDTO { ErrorCodes = { -1 }, Messages = { MessageConst.NOT_FOUND_PROCESSOR } };
            }

            var currentDocument = await _uow.GetRepository<Document>().GetSingleAsync(y => y.Id.Equals(args.DocumentId));
            if (currentDocument == null)
            {
                return new ResultDTO { ErrorCodes = { -1 }, Messages = { MessageConst.DOCUMENT.NOT_FOUND_DOCUMENT } };
            }
            _permissionBusiness.AddPerm(currentDocument.Id, null, null, findProcessor.Id, PermEnum.View);

            var newTask = new TaskManagement()
            {
                DocumentId = currentDocument.Id,
                ProcessorId = findProcessor.Id,
                AssignType = (int) args.AssignType,
                Content = args.Content,
                FromDate = args.FromDate,
                ToDate = args.ToDate,
                IsSendMail = args.IsSendMail,
                IsReportRequest = args.IsReportRequest
            };

            if (args.IsCompleted) newTask.Status = StatusConst.COMPLETED;

            var newTaskManagement = _uow.GetRepository<TaskManagement>().Add(newTask);
            _permissionBusiness.AddPerm(newTaskManagement.Id, null, null, findProcessor.Id, PermEnum.Edit); // quyen xu ly task
            _permissionBusiness.AddPerm(newTaskManagement.Id, null, null, currentDocument.CreatedById, PermEnum.View); // nguoi tao project xem task
            var mapData = _mapper.Map<TaskManagementDTO>(newTaskManagement);
            if (args.TaskDepartmentMappings.Any())
            {
                foreach (var taskDepartmentMapping in args.TaskDepartmentMappings)
                {
                    var curentDepartment = _uow.GetRepository<Department>().GetSingle(x => x.Id == taskDepartmentMapping.DepartmentId);
                    if (curentDepartment == null)
                    {
                        resultDTO = new ResultDTO { ErrorCodes = { -1 }, Messages = { MessageConst.NOT_FOUND_DEPARTMENT } };
                        break;
                    }
                    var addTaskDepartmentMapping = new TaskDepartmentMapping()
                    {
                        TaskManagementId = newTaskManagement.Id,
                        DepartmentId = curentDepartment.Id,
                        IsCoordinated = taskDepartmentMapping.IsCoordinated,
                        IsViewed = taskDepartmentMapping.IsViewed,
                        IsProcessed = taskDepartmentMapping.IsProcessed
                    };
                    var addNewTask = _uow.GetRepository<TaskDepartmentMapping>().Add(addTaskDepartmentMapping);
                    if (addNewTask != null)
                        mapData.TaskDepartmentMappings.Add(_mapper.Map<TaskDepartmentMappingDTO>(addNewTask));

                    var perm = ((taskDepartmentMapping.IsProcessed != null && taskDepartmentMapping.IsProcessed.Value) || (taskDepartmentMapping.IsCoordinated != null && taskDepartmentMapping.IsCoordinated.Value))
                        ? PermEnum.Edit : PermEnum.View;
                    _permissionBusiness.AddPerm(newTaskManagement.Id, curentDepartment.Id, CONST.ROLE.MEMBER_ID, null, perm); // quyen xu ly task
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

            if (resultDTO.ErrorCodes.Any())
            {
                return resultDTO;
                // _uow.Rollback();
            }
            await _uow.CommitAsync();
            resultDTO.Object = mapData;
            return resultDTO;
        }

        public async Task<ResultDTO> DocumentDiscussion(DocumentDiscussionArgs args)
        {
            var resultDTO = new ResultDTO() { };
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.NOT_FOUND_PARAM }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.DocumentId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { MessageConst.CANNOT_FIND_DOCUMENT_ID }, ErrorCodes = new List<int> { -1 } };
            }

            var currentDocument = await _uow.GetRepository<Document>().GetSingleAsync(y => y.Id.Equals(args.DocumentId));
            if (currentDocument == null)
            {
                return new ResultDTO { ErrorCodes = { -1 }, Messages = { MessageConst.DOCUMENT.NOT_FOUND_DOCUMENT } };
            }
            
            if (resultDTO.ErrorCodes.Any())
            {
                return resultDTO;
            }

            if (args.ParentDiscussionId != null && args.ParentDiscussionId.HasValue)
            {
                var findParentDiscussion = _uow.GetRepository<Discussion>().FindById(args.ParentDiscussionId.Value);
                if (findParentDiscussion == null)
                {
                    return new ResultDTO { ErrorCodes = { -1 }, Messages = { MessageConst.DOCUMENT.NOT_FOUND_PARENT_DISCUSSION } };
                }
                var addNewDiscussion = new Discussion()
                {
                    Message = args.Message,
                    ParentDiscussionId = findParentDiscussion.Id
                };
                _uow.GetRepository<Discussion>().Add(addNewDiscussion);
                await _uow.CommitAsync();
                resultDTO.Object = _mapper.Map<DiscussionDTO>(addNewDiscussion);

            }
            else
            {
                var addNewDiscussion = new Discussion()
                {
                    Message = args.Message
                };
                var createdDiscussion = _uow.GetRepository<Discussion>().Add(addNewDiscussion);

                var addNewDiscussionMapping = new DocumentDiscussionMapping()
                {
                    DocumentId = currentDocument.Id,
                    DiscussionId = createdDiscussion.Id
                };
                var newDis = _uow.GetRepository<DocumentDiscussionMapping>().Add(addNewDiscussionMapping);
                await _uow.CommitAsync();
                resultDTO.Object = _mapper.Map<DiscussionDTO>(createdDiscussion);
            }
            return resultDTO;
        }
    }
}
