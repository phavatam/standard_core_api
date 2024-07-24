using IziWork.Business.Args;
using IziWork.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Interfaces
{
    public interface IAttachmentFileBusiness
    {
        Task<ResultDTO> Save(AttachmentFileDTO model);
        Task<AttachmentFileDTO> Get(Guid id);
        Task<ResultDTO> Delete(Guid id);
        Task<ResultDTO> DeleteMultiFile(List<Guid> ids);
    }
}
