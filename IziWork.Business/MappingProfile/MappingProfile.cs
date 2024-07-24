using AutoMapper;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.ViewModel;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.MappingProfile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            #region User
            CreateMap<User, UserDTO>()
            .ForMember(x => x.UserDepartmentMappings, opt => opt.MapFrom(src => src.UserDepartmentMappings
                .Where(x => !x.IsDeleted.Value && x.UserDepartmentRoleMappings.All(i => !i.IsDeleted.Value))))
            .ReverseMap();
            CreateMap<UserDTO, User>();
            CreateMap<UserCRUDRequest, User>();
            CreateMap<User, UserCRUDRequest>();
            #endregion
            #region DEPARTMENT
            CreateMap<Department, DepartmentDTO>()
                .ForMember(x => x.ParentCode, opt => opt.MapFrom(src => src.Parent.Code))
                .ForMember(x => x.ParentName, opt => opt.MapFrom(src => src.Parent.Name))
                .ForMember(x => x.UserDepartmentMappings, opt => opt.MapFrom(src => src.UserDepartmentMappings.Where(x => !x.IsDeleted.Value)))
                .ReverseMap();
            CreateMap<DepartmentDTO, Department>();
            CreateMap<DepartmentTreeViewModel, Department>();
            CreateMap<Department, DepartmentTreeViewModel>()
                .ForMember(x => x.ParentCode, opt => opt.MapFrom(src => src.Parent.Code))
                .ForMember(x => x.ParentName, opt => opt.MapFrom(src => src.Parent.Name))
                .ForMember(x => x.UserDepartmentMappings, opt => opt.MapFrom(src => src.UserDepartmentMappings.Where(x => !x.IsDeleted.Value)))
                .ReverseMap();
            CreateMap<DepartmentTreeViewModel, DepartmentDTO>();
            CreateMap<Department, DepartmentArgs>();
            CreateMap<DepartmentArgs, Department>();
            #endregion
            #region METADATA
            CreateMap<MetadataTypeArgs, MetadataType>();
            CreateMap<MetadataType, MetadataTypeArgs>();
            CreateMap<MetadataType, MetadataTypeDTO>()
                    .ForMember(x => x.MetadataItems, opt => opt.MapFrom(src => src.MetadataItems.Where(x => !x.IsDeleted.Value)))
                    .ReverseMap();
            CreateMap<MetadataTypeDTO, MetadataType>();
            CreateMap<MetadataItem, MetadataArgs>();
            CreateMap<MetadataArgs, MetadataItem>();
            CreateMap<MetadataItem, MetadataItemDTO>()
                .ForMember(x => x.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(x => x.TypeCode, opt => opt.MapFrom(src => src.Type.Code))
                .ReverseMap();
            CreateMap<MetadataItemDTO, MetadataItem>();
            #endregion
            #region Role
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();
            CreateMap<Role, RoleArgs>();
            CreateMap<RoleArgs, Role>();
            #endregion
            #region User In Department
            CreateMap<UserDepartmentMapping, UserDepartmentMappingDTO>()
                    .ForMember(x => x.UserLoginName, opt => opt.MapFrom(src => src.User.LoginName))
                    .ForMember(x => x.FullName, opt => opt.MapFrom(src => src.User.FullName))
                    .ReverseMap();
            CreateMap<UserDepartmentMappingDTO, UserDepartmentMapping>();
            CreateMap<UserDepartmentRoleMappingDTO, UserDepartmentRoleMapping>();
            CreateMap<UserDepartmentRoleMapping, UserDepartmentRoleMappingDTO>()
                    .ForMember(x => x.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : ""))
                    .ForMember(x => x.IsActivated, opt => opt.MapFrom(src => src.Role != null ? src.Role.IsActivated : false))
                    .ReverseMap();
            #endregion
            #region MENU
            CreateMap<MenuArgs, Menu>();
            CreateMap<Menu, MenuArgs>();
            CreateMap<Menu, MenuLinkDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Code))
                .ReverseMap();
            CreateMap<MenuDTO, MenuLinkDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Code))
                .ReverseMap();
            CreateMap<MenuDTO, Menu>();
            CreateMap<Menu, MenuDTO>()
                .ForMember(x => x.MenuUserMappings, opt => opt.MapFrom(src => src.MenuUserMappings.Where(x => !x.IsDeleted.Value)))
                .ForMember(x => x.MenuRoleMappings, opt => opt.MapFrom(src => src.MenuRoleMappings.Where(x => !x.IsDeleted.Value)))
                .ForMember(x => x.MenuDepartmentMappings, opt => opt.MapFrom(src => src.MenuDepartmentMappings.Where(x => !x.IsDeleted.Value)))
                .ReverseMap();
            CreateMap<Menu, MenuTreeViewModel>();
            CreateMap<MenuTreeViewModel, Menu>();
            CreateMap<MenuTreeViewModel, MenuDTO>();
            CreateMap<MenuDTO, MenuTreeViewModel>();

            CreateMap<UserInMenuArgs, MenuUserMapping>();
            CreateMap<MenuUserMapping, UserInMenuArgs>();
            CreateMap<MenuUserMapping, MenuUserMappingDTO>()
                .ForMember(x => x.UserLoginName, opt => opt.MapFrom(src => src.User.LoginName))
                .ForMember(x => x.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(x => x.MenuName, opt => opt.MapFrom(src => src.Menu.Name))
                .ForMember(x => x.MenuCode, opt => opt.MapFrom(src => src.Menu.Code))
                .ReverseMap();
            CreateMap<MenuUserMappingDTO, MenuUserMapping>();

            CreateMap<DepartmentInMenuArgs, MenuDepartmentMapping>();
            CreateMap<MenuDepartmentMapping, DepartmentInMenuArgs>();
            CreateMap<MenuDepartmentMapping, MenuDepartmentMappingDTO>()
                .ForMember(x => x.DepartmentCode, opt => opt.MapFrom(src => src.Department.Code))
                .ForMember(x => x.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(x => x.MenuName, opt => opt.MapFrom(src => src.Menu.Name))
                .ForMember(x => x.MenuCode, opt => opt.MapFrom(src => src.Menu.Code))
                .ReverseMap();
            CreateMap<MenuDepartmentMappingDTO, MenuDepartmentMapping>();

            CreateMap<RoleInMenuArgs, MenuRoleMapping[]>();
            CreateMap<MenuRoleMapping, RoleInMenuArgs>();
            CreateMap<MenuRoleMapping, MenuRoleMappingDTO>()
                .ForMember(x => x.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(x => x.MenuName, opt => opt.MapFrom(src => src.Menu.Name))
                .ForMember(x => x.MenuCode, opt => opt.MapFrom(src => src.Menu.Code))
                .ReverseMap();
            CreateMap<MenuRoleMappingDTO, MenuRoleMapping>();
            #endregion
            #region Workflow Template
            CreateMap<WorkflowTemplate, WorkflowTemplateDTO>()
                    .ForMember(x => x.WorkflowSteps, opt => opt.MapFrom(src => src.WorkflowSteps))
                    .ReverseMap();
            CreateMap<WorkflowTemplateDTO, WorkflowTemplate>()
                    .ForMember(x => x.WorkflowSteps, opt => opt.MapFrom(src => src.WorkflowSteps))
                    .ReverseMap();
            CreateMap<WorkflowStep, WorkflowStepDTO>();
            CreateMap<WorkflowStepDTO, WorkflowStep>();

            CreateMap<WorkflowRole, WorkflowRoleDTO>();
            CreateMap<WorkflowRoleDTO, WorkflowRole>();

            CreateMap<WorkflowInstance, WorkflowInstanceDTO>()
                    .ReverseMap();
            CreateMap<WorkflowInstanceDTO, WorkflowInstance>()
                    .ReverseMap(); 

            CreateMap<WorkflowProcessing, WorkflowProcessingDTO>();
            CreateMap<WorkflowProcessingDTO, WorkflowProcessing>();

            #endregion

            #region Profile
            CreateMap<Data.Entities.Profile, ProfileDTO>()
                .ForMember(x => x.ProfileAttachmentFileMappings, opt => opt.MapFrom(src => src.ProfileAttachmentFileMappings))
                .ReverseMap();
            CreateMap<ProfileDTO, Data.Entities.Profile>().ReverseMap();

            CreateMap<Data.Entities.Profile, ProfileArgs>().ReverseMap();
            CreateMap<ProfileArgs, Data.Entities.Profile>().ReverseMap();

            CreateMap<ProfileAttachmentFileMapping, ProfileAttachmentFileMappingDTO>()
                .ForMember(x => x.FileName, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.FileName : null))
                .ForMember(x => x.FileUniqueName, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.FileUniqueName : null))
                .ForMember(x => x.FileDisplayName, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.FileDisplayName : null))
                .ForMember(x => x.Extension, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.Extension : null))
                .ForMember(x => x.Type, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.Type : null))
                .ForMember(x => x.Size, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.Size : null))
                .ReverseMap();
            CreateMap<ProfileAttachmentFileMappingDTO, ProfileAttachmentFileMapping>();
            #endregion

            #region Category 
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<CategoryDetail, CategoryDetailDTO>()
                    .ForMember(x => x.Type, opt => opt.MapFrom(src => src.Category.Type))
                    .ReverseMap();
            CreateMap<CategoryDetailDTO, CategoryDetail>();

            CreateMap<CategoryDetail, CategoryDetailArgs>();
            CreateMap<CategoryDetailArgs, CategoryDetail>();
            #endregion

            #region Document
            CreateMap<Document, DocumentDTO>();
            CreateMap<DocumentDTO, Document>();

            CreateMap<Document, DocumentArgs>();
            CreateMap<DocumentArgs, Document>();

            CreateMap<ReferenceDocument, ReferenceDocumentDTO>();
            CreateMap<ReferenceDocumentDTO, ReferenceDocument>();

            CreateMap<DocumentForwarding, DocumentForwardingDTO>()
                    .ForMember(x => x.ProcessorName, opt => opt.MapFrom(src => src.Processor != null ? src.Processor.FullName : null))
                    .ForMember(x => x.DepartmentCode, opt => opt.MapFrom(src => src.Department != null ? src.Department.Code : null))
                    .ForMember(x => x.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                    .ReverseMap();
            CreateMap<DocumentForwardingDTO, DocumentForwarding>();

            CreateMap<TaskManagement, TaskManagementDTO>()
                .ForMember(x => x.ProcessorName, opt => opt.MapFrom(src => src.Processor != null ? src.Processor.FullName : null))
                .ReverseMap();
            CreateMap<TaskManagementDTO, TaskManagement>();

            CreateMap<TaskManagementHistory, TaskManagementHistoryDTO>().ReverseMap();
            CreateMap<TaskManagementHistoryDTO, TaskManagementHistory>().ReverseMap();

            CreateMap<TaskDepartmentMapping, TaskDepartmentMappingDTO>()
                /*.ForMember(x => x.DepartmentCode, opt => opt.MapFrom(src => src.Department != null ? src.Department.Code : ""))
                .ForMember(x => x.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : ""))*/
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.LoginName : ""))
                .ForMember(x => x.FullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : ""))
                .ReverseMap();
            CreateMap<TaskDepartmentMappingDTO, TaskDepartmentMapping>();

            CreateMap<DocumentAttachmentMapping, DocumentAttachmentMappingDTO>()
                .ForMember(x => x.FileName, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.FileName : null))
                .ForMember(x => x.FileUniqueName, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.FileUniqueName : null))
                .ForMember(x => x.FileDisplayName, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.FileDisplayName : null))
                .ForMember(x => x.Extension, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.Extension : null))
                .ForMember(x => x.Type, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.Type : null))
                .ForMember(x => x.Size, opt => opt.MapFrom(src => src.AttachmentFile != null ? src.AttachmentFile.Size : null))
                .ReverseMap(); ;
            CreateMap<DocumentAttachmentMappingDTO, DocumentAttachmentMapping>();

            CreateMap<DocumentDiscussionMapping, DocumentDiscussionMappingDTO>();
            CreateMap<DocumentDiscussionMappingDTO, DocumentDiscussionMapping>();

            CreateMap<Discussion, DiscussionDTO>();
            CreateMap<DiscussionDTO, Discussion>();
            #endregion

            #region Attachment file
            CreateMap<AttachmentFile, AttachmentFileDTO>();
            CreateMap<AttachmentFileDTO, AttachmentFile>();
            #endregion

            CreateMap<TaskExtend, TaskExtendDTO>();
            CreateMap<TaskExtendDTO, TaskExtend>();

            #region Status
            CreateMap<Status, StatusArgs>();
            CreateMap<StatusArgs, Status>();

            CreateMap<Status, StatusDTO>();
            CreateMap<StatusDTO, Status>();
            #endregion

            #region Company
            CreateMap<CompanyInfo, CompanyInfoDTO>().ReverseMap();
            CreateMap<CompanyInfo, CompanyInfoDTO>().ReverseMap();

            CreateMap<CompanyInfo, CompanyArgs>().ReverseMap();
            CreateMap<CompanyArgs, CompanyInfo>().ReverseMap();
            #endregion

            #region General Journal
            CreateMap<GeneralJournal, GeneralJournalArgs>().ReverseMap();
            CreateMap<GeneralJournalArgs, GeneralJournal>().ReverseMap();

            CreateMap<GeneralJournalDTO, GeneralJournal>().ReverseMap();
            CreateMap<GeneralJournal, GeneralJournalDTO>()
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName))
                .ForMember(x => x.GeneralJournalDetails, opt => opt.MapFrom(src => src.GeneralJournalDetails.Where(x => x.IsDeleted != null && x.IsDeleted.HasValue && !x.IsDeleted.Value)))
                .ReverseMap();

            CreateMap<GeneralJournalDetail, GeneralJournalDetailDTO>()
                .ReverseMap();
            CreateMap<GeneralJournalDetailDTO, GeneralJournalDetail>().ReverseMap();

            CreateMap<GeneralJournalDetail, GeneralJournalDetailArgs>().ReverseMap();
            CreateMap<GeneralJournalDetailArgs, GeneralJournalDetail>().ReverseMap();

            CreateMap<FinancialAccount, FinancialAccountDTO>().ReverseMap();
            CreateMap<FinancialAccountDTO, FinancialAccount>().ReverseMap();

            CreateMap<FinancialAccount, FinancialAccountArgs>().ReverseMap();
            CreateMap<FinancialAccountArgs, FinancialAccount>().ReverseMap();
            #endregion
        }
    }
}
