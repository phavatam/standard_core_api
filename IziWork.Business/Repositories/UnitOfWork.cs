using AutoMapper;
using IziWork.Business.DTO;
using IziWork.Business.IRepositories;
using IziWork.Business.ViewModel;
using IziWork.Data;
using IziWork.Data.Entities;
using IziWork.Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private UpgradeApplicationContext _context;
        private Dictionary<Type, object> _repositories;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //public IUserRepository UserRepository { get; private set; }
        public UnitOfWork(UpgradeApplicationContext context, ILoggerFactory loggerFactory, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            //UserRepository = new UserRepository(context);
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        //public CurrentUserDTO CurrentUser { get; set; }
        /*private string[] GetChangedEntityNames()
        {
            // Updated version of this method: \EFSecondLevelCache\EFSecondLevelCache.Tests\EFSecondLevelCache.TestDataLayer\DataLayer\SampleContext.cs
            return _context.ChangeTracker.Entries()
                .Where(x => x.State == Microsoft.EntityFrameworkCore.EntityState.Added ||
                            x.State == Microsoft.EntityFrameworkCore.EntityState.Modified ||
                            x.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
                .Select(x => System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(x.Entity.GetType()).FullName)
                .Distinct()
                .ToArray();
        }*/

        public async Task<int> CommitAsync()
        {
            //var changedEntityNames = GetChangedEntityNames();
            //var audit = new Audit();
            var rowAffecteds = await _context.SaveChangesAsync();
            //audit.PostSaveChanges();

            /*if (audit.Configuration.AutoSavePreAction != null)
            {
                await _context.SaveChangesAsync();
            }*/
            //new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            return rowAffecteds;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity, new()
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            /*if (!_repositories.ContainsKey(type))
            {
                if (!(UserContext is null))
                {
                    if ((UserContext.CurrentUserRole & UserRole.SAdmin) == UserRole.SAdmin || (UserContext.CurrentUserRole & UserRole.HRAdmin) == UserRole.HRAdmin)
                    {
                        _repositories[type] = new Repository<TEntity>(Context, true, UserContext);
                    }

                    else if ((UserContext.CurrentUserRole & UserRole.CB) == UserRole.CB && typeof(ICBEntity).IsAssignableFrom(type) && UserContext.IsHQ)
                    {
                        _repositories[type] = new Repository<TEntity>(Context, true, UserContext);

                    }
                    else if ((UserContext.CurrentUserRole & UserRole.Accounting) == UserRole.Accounting && typeof(BusinessTripApplication).IsAssignableFrom(type))
                    {
                        _repositories[type] = new Repository<TEntity>(Context, true, UserContext);
                    }
                    else if ((UserContext.CurrentUserRole & UserRole.Admin) == UserRole.Admin && typeof(BusinessTripApplication).IsAssignableFrom(type))
                    {
                        _repositories[type] = new Repository<TEntity>(Context, true, UserContext);
                    }
                    else
                    {
                        _repositories[type] = new Repository<TEntity>(Context, false, UserContext);
                    }
                }
                else
                {
                    
                }
            }*/
            _repositories[type] = new GenericRepository<TEntity>(_context, false, _mapper ,_httpContextAccessor);
            return (GenericRepository<TEntity>)_repositories[type];
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>(bool forceAllItems) where TEntity : class, IEntity, new()
        {
            return new GenericRepository<TEntity>(_context, forceAllItems, _mapper, _httpContextAccessor);
        }

        public int Commit()
        {
            //var changedEntityNames = GetChangedEntityNames();
            var rowAffecteds = _context.SaveChanges();
            //new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            return rowAffecteds;
        }

        public async Task CompletedAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task Rollback()
        {
            await _context.DisposeAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
