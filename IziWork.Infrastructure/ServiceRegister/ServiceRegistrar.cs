using AutoMapper;
using CacheManager.Core;
using EFSecondLevelCache.Core;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWork.Business.IRepositories;
using IziWork.Business.MappingProfile;
using IziWork.Business.Repositories;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Infrastructure.ServiceRegister
{
    public static class ServiceRegistrar
    {
        public static void AddCORS(IServiceCollection services, IConfiguration configuration)
        {
            
        }

        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddHttpContextAccessor();
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
            /*services.AddEFSecondLevelCache();
            services.AddMemoryCache();*/
            //services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<IDepartmentBusiness, DepartmentBusiness>();
            services.AddScoped<IMenuBusiness, MenuBusiness>();
            services.AddScoped<IRoleBusiness, RoleBusiness>();
            services.AddScoped<IWorkflowBusiness, WorkflowBusiness>();
            services.AddScoped<IMetadataBusiness, MetadataBusiness>();
            services.AddScoped<ICategoryDetailBusiness, CategoryDetailBusiness>();
            services.AddScoped<IDocumentBusiness, DocumentBusiness>();
            services.AddScoped<IProfileBusiness, ProfileBusiness>();
            services.AddScoped<IAttachmentFileBusiness, AttachmentFileBusiness>();
            services.AddScoped<IPermissionBusiness, PermissionBusiness>();
            services.AddScoped<ITaskManagementBusiness, TaskManagementBusiness>();
            services.AddScoped<IStatusBusiness, StatusBusiness>();
            services.AddScoped<IGeneralJournalBusiness, GeneralJournalBusiness>();
            services.AddScoped<ICompanyBusiness, CompanyBusiness>();
            services.AddScoped<IFinancialAccountBusiness, FinancialAccountBusiness>();
            services.AddScoped<IAccountingBalanceSheetBusiness, AccountingBalanceSheetBusiness>();
        }
    }
}