using AutoMapper;
using Core.Repositories.Business.IRepositories;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWork.Common.Constans;
using IziWork.Common.DTO;
using IziWork.Data.Entities;
using Mapster;
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

namespace IziWork.Business.Handlers
{
    public class AttachmentFileBusiness : IAttachmentFileBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AttachmentFileBusiness> _logger;
        public AttachmentFileBusiness(IUnitOfWork uow, IMapper mapper, ILogger<AttachmentFileBusiness> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ResultDTO> Save(AttachmentFileDTO model)
        {
            var res = new ResultDTO();
            try
            {
                if (model == null)
                {
                    res.Messages.Add("Attachment to save is null");
                    res.ErrorCodes.Add(0);
                    return res;
                }

                if (model.Id == Guid.Empty) // Create
                {
                    var attachment = _mapper.Map<AttachmentFile>(model);

                    _uow.GetRepository<AttachmentFile>().Add(attachment);
                    await _uow.CommitAsync();
                    res.Object = _mapper.Map<AttachmentFileDTO>(attachment);
                }
                else
                {
                    var att = await _uow.GetRepository<AttachmentFile>().GetSingleAsync(x => x.Id == model.Id);
                    if (att != null)
                    {
                        _mapper.Map(model, att);

                        await _uow.CommitAsync();

                        res.Object = _mapper.Map<AttachmentFileDTO>(att);
                    }
                    else
                    {
                        res.Messages.Add("Can not find Attachment");
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                res.Messages.Add(ex.Message);
            }
            return res;
        }

        public async Task<AttachmentFileDTO> Get(Guid id)
        {
            AttachmentFileDTO res = null;
            try
            {
                if (id != Guid.Empty)
                {
                    var att = await _uow.GetRepository<AttachmentFile>().GetSingleAsync(x => x.Id == id);
                    if (att != null)
                    {
                        res = _mapper.Map<AttachmentFileDTO>(att);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<ResultDTO> Delete(Guid id)
        {
            var res = new ResultDTO();
            try
            {
                if (id == Guid.Empty)
                {
                    res.Messages.Add("Attachment Id is not valid");
                    return res;
                }

                var attachment = await _uow.GetRepository<AttachmentFile>().GetSingleAsync(x => x.Id == id);
                if (attachment != null)
                {
                    _uow.GetRepository<AttachmentFile>().Delete(attachment);
                    await _uow.CommitAsync();
                    res.Messages.Add(MessageConst.DELETE_SUCCESSFULLY);
                    return res;
                }
                else
                {
                    res.Messages.Add("Can not find Attachment");
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                res.Messages.Add(ex.Message);
            }
            return res;
        }

        public async Task<ResultDTO> DeleteMultiFile(List<Guid> AttachmentFileIds)
        {
            var res = new ResultDTO();
            try
            {
                var attachments = await _uow.GetRepository<AttachmentFile>().FindByAsync(x => AttachmentFileIds.Contains(x.Id));
                if (attachments.Any())
                {
                    _uow.GetRepository<AttachmentFile>().Delete(attachments);
                    await _uow.CommitAsync();
                    res.Messages.Add(MessageConst.DELETE_SUCCESSFULLY);
                }
                else
                {
                    res.Messages.Add("Can not find Attachment");
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                res.Messages.Add(ex.Message);
            }
            return res;
        }
    }
}
