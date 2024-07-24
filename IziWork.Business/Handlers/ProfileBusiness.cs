using AutoMapper;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;
using IziWork.Business.IRepositories;
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
    public class ProfileBusiness : IProfileBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProfileBusiness(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        #region INSERT - UPDATE - DELETE
        public async Task<ResultDTO> CreateOrUpdate(ProfileArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {
                var currentProfile = await _uow.GetRepository<Data.Entities.Profile>().GetSingleAsync(y => y.Id.Equals(args.Id));
                if (currentProfile == null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "PROFILE_NOT_EXISTED" } };
                }
                if (!string.IsNullOrEmpty(args.Name))
                {
                    currentProfile.Name = args.Name;
                }
                if (!string.IsNullOrEmpty(args.Code))
                {
                    currentProfile.Code = args.Code;
                }
                if (!string.IsNullOrEmpty(args.Description))
                {
                    currentProfile.Description = args.Description;
                }
                var profileUpdated = _uow.GetRepository<Data.Entities.Profile>().Update(currentProfile);
                if (args.AttachmentFileIds != null && args.AttachmentFileIds.Any())
                {
                    var findOldAttachment = await _uow.GetRepository<ProfileAttachmentFileMapping>().FindByAsync(x => x.ProfileId == currentProfile.Id);
                    if (findOldAttachment != null) _uow.GetRepository<ProfileAttachmentFileMapping>().Delete(findOldAttachment);

                    foreach(var attachmentFileId in args.AttachmentFileIds)
                    {
                        var findAttachmentFile = await _uow.GetRepository<AttachmentFile>().FindByIdAsync(attachmentFileId);
                        if (findAttachmentFile != null)
                        {
                            var addAttachmentMapping = new ProfileAttachmentFileMapping()
                            {
                                AttachmentFileId = findAttachmentFile.Id,
                                ProfileId = profileUpdated.Id
                            };
                            _uow.GetRepository<ProfileAttachmentFileMapping>().Add(addAttachmentMapping);
                        }
                    }
                }
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "UPDATE_PROFILE_IS_SUCCESSFULLY" },
                    Object = _mapper.Map<ProfileDTO>(profileUpdated)
                };
            }
            else
            {
                var data = _mapper.Map<Data.Entities.Profile>(args);
                var createdData = _uow.GetRepository<Data.Entities.Profile>().Add(data);
                if (args.AttachmentFileIds != null && args.AttachmentFileIds.Any())
                {
                    foreach (var attachmentFileId in args.AttachmentFileIds)
                    {
                        var findAttachmentFile = await _uow.GetRepository<AttachmentFile>().FindByIdAsync(attachmentFileId);
                        if (findAttachmentFile != null)
                        {
                            var addAttachmentMapping = new ProfileAttachmentFileMapping()
                            {
                                AttachmentFileId = findAttachmentFile.Id,
                                ProfileId = createdData.Id
                            };
                            _uow.GetRepository<ProfileAttachmentFileMapping>().Add(addAttachmentMapping);
                        }
                    }
                }
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "CREATE_PROFILE_IS_SUCCESSFULLY" },
                    Object = _mapper.Map<ProfileDTO>(createdData)
                };
            }
            return resultDTO;
        }
        public async Task<ResultDTO> DeleteById(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<Data.Entities.Profile>().GetSingleAsync(y => y.Id.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "PROFILE_NOT_EXIST" } };
            }
            else
            {
                _uow.GetRepository<Data.Entities.Profile>().Delete(existing);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "DELETE_PROFILE_IS_SUCCESSFULLY" },
                };
            }
            return resultDTO;
        }
        #endregion
        #region GET DATA
        public async Task<ResultDTO> GetList(QueryArgs args)
        {
            var jobgradeList = await _uow.GetRepository<Data.Entities.Profile>().FindByAsync<ProfileDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = await _uow.GetRepository<Data.Entities.Profile>().CountAsync(args.Predicate, args.PredicateParameters);
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
            var findProfile = await _uow.GetRepository<Data.Entities.Profile>().GetSingleAsync<ProfileDTO>(x => x.Id == Id);
            if (findProfile == null)
            {
                return new ResultDTO() { Messages = new List<string> { "PROFILE_IS_NOT_EXIST" }, ErrorCodes = new List<int> { -1 } };
            }
            return new ResultDTO { Object = findProfile };
        }
        #endregion
    }
}
