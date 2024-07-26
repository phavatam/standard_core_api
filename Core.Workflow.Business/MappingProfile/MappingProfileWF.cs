using Core.Workflow.Business.DTO;
using IziWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Workflow.Business.MappingProfile
{
    public class MappingProfileWF : AutoMapper.Profile
    {

        public MappingProfileWF()
        {
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
        }
    }
}
