using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class PermissionApproveDTO
    {
        public bool HasPermissionApprove { get; set; }
        public WorkflowStepDTO CurrentStep { get; set; }
    }
}
