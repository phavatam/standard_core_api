using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Interfaces;

using IziWork.Data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repositories.Business.IRepositories;
using IziWork.Common.DTO;
using IziWork.Common.Args;

namespace IziWork.Business.Handlers
{
    public class MetadataBusiness : IMetadataBusiness
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly AutoMapper.IMapper _mapper;
        public MetadataBusiness(IUnitOfWork uow, IConfiguration configuration, AutoMapper.IMapper mapper)
        {
            _uow = uow;
            _configuration = configuration;
            _secretKey = (_configuration != null && _configuration["AppSettings:SecretKey"] != null) ? _configuration["AppSettings:SecretKey"].ToString() : "8qaa,AQ%UrhXY|#PRsb%!4qc8yCbh8n'Bsi{>;I7,%R#EhV@wn%+ni.g#g^h]rF~BQ_>:-F)+dC%!ST6K2";
            _mapper = mapper;
        }

        #region INSERT - UPDATE - DELETE
        public async Task<ResultDTO> CreateOrUpdateType(MetadataTypeArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
            }
            if (string.IsNullOrEmpty(args.Code))
            {
                return new ResultDTO() { Messages = new List<string> { "CODE_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (string.IsNullOrEmpty(args.Name))
            {
                return new ResultDTO() { Messages = new List<string> { "NAME_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.MetadataItems != null && args.MetadataItems.Count > 0)
            {
                foreach (var mapping in args.MetadataItems)
                {

                    if (mapping.IsDeleted.HasValue && mapping.IsDeleted.Value)
                        continue;
                    if (string.IsNullOrEmpty(mapping.Code))
                    {
                        return new ResultDTO() { Messages = new List<string> { "CODE_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
                    }
                    if (string.IsNullOrEmpty(mapping.Name))
                    {
                        return new ResultDTO() { Messages = new List<string> { "NAME_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
                    }
                    if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
                    {
                        if (mapping.TypeId == Guid.Empty)
                        {
                            return new ResultDTO() { Messages = new List<string> { "TYPE_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
                        }
                        var existing = await _uow.GetRepository<MetadataItem>().GetSingleAsync(y => y.Code.Equals(mapping.Code) && !y.Id.Equals(mapping.Id) && y.TypeId.Equals(mapping.TypeId));
                        if (existing != null)
                        {
                            return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "METADATAITEM_EXISTED" } };
                        }
                    }
                }

            }
            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {
                //update
                var existing = await _uow.GetRepository<MetadataType>().GetSingleAsync(y => y.Code.Equals(args.Code) && !y.Id.Equals(args.Id));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "METADATATYPE_EXISTED" } };
                }
                else
                {
                    var currentMeta = await _uow.GetRepository<MetadataType>().GetSingleAsync(y => y.Id.Equals(args.Id));
                    if (currentMeta == null)
                    {
                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "METADATATYPE_NOT_EXIST" } };
                    }
                    if (args.MetadataItems != null && args.MetadataItems.Count > 0)
                    {
                        foreach (var mapping in args.MetadataItems)
                        {
                            if (mapping.IsDeleted != null && !mapping.IsDeleted.Value)
                            {
                                if (mapping.Id != null && mapping.Id.Value != Guid.Empty)
                                {
                                    var currentItem = await _uow.GetRepository<MetadataItem>().GetSingleAsync(y => y.Id.Equals(mapping.Id));
                                    if (currentItem == null)
                                    {
                                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "METADATAITEM_NOT_EXIST" } };
                                    }

                                    currentItem.Code = mapping.Code;
                                    currentItem.Name = mapping.Name;
                                    _uow.GetRepository<MetadataItem>().Update(currentItem);
                                }
                                else
                                {
                                    var item = new MetadataItem()
                                    {
                                        Code = mapping.Code,
                                        Name = mapping.Name,
                                        TypeId = mapping.TypeId,
                                    };
                                    _uow.GetRepository<MetadataItem>().Add(item);

                                }
                            }
                            else
                            {
                                if (mapping.Id != null && mapping.Id.Value != Guid.Empty)
                                {
                                    var currentItem = await _uow.GetRepository<MetadataItem>().GetSingleAsync(y => y.Id.Equals(mapping.Id));
                                    if (currentItem == null)
                                    {
                                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "METADATAITEM_NOT_EXIST" } };
                                    }
                                    _uow.GetRepository<MetadataItem>().Delete(currentItem);
                                }
                            }

                        }
                    }



                    if (!string.IsNullOrEmpty(args.Code))
                    {
                        currentMeta.Code = args.Code;
                    }
                    if (!string.IsNullOrEmpty(args.Name))
                    {
                        currentMeta.Name = args.Name;
                    }
                    var dept = _uow.GetRepository<MetadataType>().Update(currentMeta);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "UPDATE_METADATATYPE_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MetadataTypeDTO>(currentMeta)
                    };
                }
            }
            else
            {
                var existing = await _uow.GetRepository<MetadataType>().GetSingleAsync(y => y.Code.Equals(args.Code));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "CREATE_METADATATYPE_IS_EXIST" } };
                }
                else
                {
                    var data = new MetadataType();
                    data.Code = args.Code;
                    data.Name = args.Name;
                    _uow.GetRepository<MetadataType>().Add(data);
                    if (data != null)
                    {
                        if (args.MetadataItems != null && args.MetadataItems.Count > 0)
                        {
                            foreach (var mapping in args.MetadataItems)
                            {
                                if (mapping.IsDeleted != null && !mapping.IsDeleted.Value)
                                {
                                    var item = new MetadataItem()
                                    {
                                        Code = mapping.Code,
                                        Name = mapping.Name,
                                        TypeId = data.Id,
                                    };
                                    _uow.GetRepository<MetadataItem>().Add(item);
                                }
                            }
                        }
                    }
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "CREATEUPDATE_METADATATYPE_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MetadataTypeDTO>(data)
                    };
                }
            }

            return resultDTO;
        }
        public async Task<ResultDTO> DeleteMetadataType(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<MetadataType>().GetSingleAsync(y => y.Id.Equals(Id));
            var listChild = await _uow.GetRepository<MetadataItem>().AnyAsync(y => !y.Id.Equals(Id) && y.TypeId.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "METADATATYPE_NOT_EXIST" } };
            }
            else if (listChild)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "METADATATYPE_HAS_CHILD_ITEM" } };
            }
            else
            {

                _uow.GetRepository<MetadataType>().Delete(existing);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "DELETE_METADATATYPE_IS_SUCCESSFULLY" },
                    Object = true
                };
            }
            return resultDTO;
        }

        public async Task<ResultDTO> CreateOrUpdateItem(MetadataArgs args)
        {
            var resultDTO = new ResultDTO();
            if (args is null)
            {
                return new ResultDTO() { Messages = new List<string> { "CANNOT_FIND_ANY_PARAM" }, ErrorCodes = new List<int> { -1 } };
            }
            if (string.IsNullOrEmpty(args.Code))
            {
                return new ResultDTO() { Messages = new List<string> { "CODE_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (string.IsNullOrEmpty(args.Name))
            {
                return new ResultDTO() { Messages = new List<string> { "NAME_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.TypeId == Guid.Empty)
            {
                return new ResultDTO() { Messages = new List<string> { "TYPE_IS_REQUIRED" }, ErrorCodes = new List<int> { -1 } };
            }
            if (args.Id != null && args.Id.HasValue && args.Id.Value != Guid.Empty)
            {
                //update
                var existing = await _uow.GetRepository<MetadataItem>().GetSingleAsync(y => y.Code.Equals(args.Code) && !y.Id.Equals(args.Id) && y.TypeId.Equals(args.TypeId));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "METADATAITEM_EXISTED" } };
                }
                else
                {
                    var currentMeta = await _uow.GetRepository<MetadataItem>().GetSingleAsync(y => y.Id.Equals(args.Id));
                    if (currentMeta == null)
                    {
                        return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "METADATAITEM_NOT_EXIST" } };
                    }
                    if (!string.IsNullOrEmpty(args.Code))
                    {
                        currentMeta.Code = args.Code;
                    }
                    if (!string.IsNullOrEmpty(args.Name))
                    {
                        currentMeta.Name = args.Name;
                    }
                    var dept = _uow.GetRepository<MetadataItem>().Update(currentMeta);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "UPDATE_METADATITEM_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MetadataItemDTO>(currentMeta)
                    };
                }
            }
            else
            {
                //insert
                var existing = await _uow.GetRepository<MetadataItem>().GetSingleAsync(y => y.TypeId.Equals(args.TypeId) && y.Code.Equals(args.Code));
                if (existing != null)
                {
                    return new ResultDTO { ErrorCodes = { 1001 }, Messages = { "CREATE_METADATAITEM_IS_EXIST" } };
                }
                else
                {
                    var data = _mapper.Map<MetadataItem>(args);
                    var dept = _uow.GetRepository<MetadataItem>().Add(data);
                    await _uow.CommitAsync();
                    resultDTO = new ResultDTO()
                    {
                        Messages = new List<string> { "CREATE_METADATAITEM_IS_SUCCESSFULLY" },
                        Object = _mapper.Map<MetadataItemDTO>(dept)
                    };
                }
            }

            return resultDTO;
        }
        public async Task<ResultDTO> DeleteMetadataItem(Guid Id)
        {
            var resultDTO = new ResultDTO();
            var existing = await _uow.GetRepository<MetadataItem>().GetSingleAsync(y => y.Id.Equals(Id));
            if (existing == null)
            {
                return new ResultDTO { ErrorCodes = new List<int> { -1 }, Messages = { "METADATAITEM_NOT_EXIST" } };
            }

            else
            {
                _uow.GetRepository<MetadataItem>().Delete(existing);
                await _uow.CommitAsync();
                resultDTO = new ResultDTO()
                {
                    Messages = new List<string> { "DELETE_METADATAITEM_IS_SUCCESSFULLY" },
                    Object = true
                };
            }
            return resultDTO;
        }
        #endregion
        #region GET DATA
        public async Task<ResultDTO> GetListMetadataType(QueryArgs args)
        {
            var metaList = await _uow.GetRepository<MetadataType>().FindByAsync<MetadataTypeDTO>(args.Order, args.Page, args.Limit, args.Predicate, args.PredicateParameters);
            var totalList = metaList.Count();
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = metaList,
                    Count = totalList
                }
            };
        }

        public async Task<ResultDTO> GetMetadataItemByType(Guid TypeId)
        {
            var metaList = await _uow.GetRepository<MetadataItem>().FindByAsync<MetadataItemDTO>(x => x.TypeId.Equals(TypeId));
            var totalList = metaList.Count();
            return new ResultDTO()
            {
                Object = new ArrayResultDTO()
                {
                    Data = metaList,
                    Count = totalList
                }
            };
        }
        #endregion
    }
}
