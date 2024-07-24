using IziWork.Business.Args;
using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface ITaskManagementBusiness
    {
        Task<ResultDTO> GetListTasks(QueryArgs args);
        Task<ResultDTO> AssignTask(AssignTaskArgs args);
        Task<ResultDTO> UpdateTask(AssignTaskArgs args);
        Task<ResultDTO> CreateTask(AssignTaskArgs args);
        Task<ResultDTO> ExtendTasks(ExtendTaskArgs args);
        Task<ResultDTO> ApproveExtendTask(ApproveExtendTaskArgs args);
        Task<ResultDTO> GetTaskManagementHistories(QueryArgs args);
        Task<ResultDTO> UpdateStatusTask(UpdateStatusTaskArgs args);
        Task<TaskManagementDTO> GetById(Guid Id);
        Task<List<TaskManagementDTO>> GetTaskByParentTaskId(Guid parentTaskId);
    }
}