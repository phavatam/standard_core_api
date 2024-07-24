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
    public class TrackingLogBusiness : ITrackingLogBusiness
    {
        private readonly IUnitOfWork _uow;
        public TrackingLogBusiness(IUnitOfWork uow)
        {
            _uow = uow;
        }


    }
}
