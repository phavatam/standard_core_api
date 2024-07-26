using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Repositories.Business.CustomExtensions;
using Core.Repositories.Business.DTO;
using Core.Repositories.Business.Enum;
using Core.Repositories.Business.IRepositories;
using IziWork.Data;
using IziWork.Data.Abstracts;
using IziWork.Data.Entities;
using IziWork.Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Repositories.Business.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity, new()
    {
        private static Dictionary<string, ReferenceNumber> _refs;
        protected UpgradeApplicationContext _context;
        private readonly bool _forceAllItems;
        protected DbSet<T> _dbSet;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GenericRepository(UpgradeApplicationContext context, bool forceAllItems, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _context = context;
            _dbSet = context.Set<T>();
            _forceAllItems = forceAllItems;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            GetCurrentUser();
            LoadRefs();
        }
        public CurrentUserDTO CurrentUser;
        private void LoadRefs()
        {
            if (GlobalData.Instance.Refs == null || GlobalData.Instance.Refs.Count == 0)
            {
                lock (GlobalData.Instance.GlobalLock)
                {
                    if (GlobalData.Instance.Refs == null || GlobalData.Instance.Refs.Count == 0)
                    {
                        GlobalData.Instance.Refs = _context.Set<ReferenceNumber>().AsNoTracking().ToList().ToDictionary(x => x.ModuleType, x => x);
                    }
                }
            }
            _refs = GlobalData.Instance.Refs;
        }

        public void GetCurrentUser()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.Items["UUID"] as Guid? ?? Guid.Empty;
            if (currentUserId != Guid.Empty)
            {
                var users = _context.Users.Where(x => x.Id == currentUserId && x.IsDeleted != null && !x.IsDeleted.Value && x.IsActivated);
                if (users != null)
                {
                    var user = users.FirstOrDefault();
                    var userInDepartmentList = new List<CurrentUserDepartmentMappingDTO>();
                    var userInDepartment = _context.UserDepartmentMappings.Where(x => x.UserId == currentUserId && x.IsDeleted != null && !x.IsDeleted.Value).ToList();
                    foreach (var userInDept in userInDepartment)
                    {
                        var mapData = _mapper.Map<CurrentUserDepartmentMappingDTO>(userInDept);
                        var userInDepartmentRoleList = new List<CurrentUserDepartmentRoleMappingDTO>();
                        var currentRole = _context.UserDepartmentRoleMappings.Where(x => x.UserDepartmentMappingId == userInDept.Id && x.IsDeleted != null && !x.IsDeleted.Value).ToList();
                        if (currentRole != null)
                            userInDepartmentRoleList.AddRange(_mapper.Map<List<CurrentUserDepartmentRoleMappingDTO>>(currentRole));

                        mapData.UserDepartmentRoleMappings = userInDepartmentRoleList;
                        userInDepartmentList.Add(mapData);
                    }
                    CurrentUser = new CurrentUserDTO
                    {
                        Id = user.Id,
                        LoginName = user.LoginName,
                        FullName = user.FullName,
                        UserDepartmentMappingDTO = userInDepartmentList,
                    };
                }
            }
        }

        public T Add(T entity)
        {
            entity.Id = Guid.NewGuid();
            entity.Created = DateTime.Now;
            entity.Modified = DateTime.Now;
            if (CurrentUser != null)
            {
                if (entity is IAuditableEntity softEntity)
                {
                    softEntity.CreatedById = CurrentUser.Id;
                    softEntity.CreatedBy = CurrentUser.LoginName;
                    softEntity.CreatedByFullName = CurrentUser.FullName;
                }

                #region
                try
                {
                    string className = entity.GetType().Name;
                    var trackingLog = new TrackingLog
                    {
                        Id = Guid.NewGuid(),
                        ItemId = entity.Id,
                        ItemType = className,
                        Action = (int)ActionEnum.ADD,
                        Created = DateTime.Now,
                        CreatedById = CurrentUser.Id,
                        CreatedBy = CurrentUser.LoginName,
                        CreatedByFullName = CurrentUser.FullName
                    };
                    _context.Set<TrackingLog>().Add(trackingLog);
                }
                catch (Exception e) { }
                #endregion  
            }

            if (entity is IAutoNumber autoNamingEntity && !typeof(Document).IsAssignableFrom(typeof(T)))
            {
                autoNamingEntity.ReferenceNumber = GenerateReferenceNumber(typeof(T).Name);
            }

            if (entity is ISoftDeleteEntity softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = false;
            }

            if (entity is ISoftStatusEntity softStatusEntity)
            {
                softStatusEntity.Status = "New";
            }
            /*if (entity is IPermission)
            {
                AddPerm(entity);
            }*/
            /* if (entity is WorkflowEntity wfEntity)
             {
                 wfEntity.Status = "Draft"; //add draft for work flow entity by default
             }*/

            _context.Set<T>().Add(entity);
            return entity;
        }
        public T? Update(T entity)
        {
            var dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
            dbEntityEntry.Property(x => x.Created).IsModified = false;
            if (!(CurrentUser is null))
            {
                if (entity is IAuditableEntity auditEntity)
                {
                    auditEntity.Modified = DateTime.Now;
                    auditEntity.ModifiedById = CurrentUser.Id;
                    auditEntity.ModifiedBy = CurrentUser.LoginName;
                    auditEntity.ModifiedByFullName = CurrentUser.FullName;
                    var auditableEntityEntry = _context.Entry(auditEntity);
                    auditableEntityEntry.Property(x => x.CreatedBy).IsModified = false;
                    auditableEntityEntry.Property(x => x.CreatedById).IsModified = false;
                    auditableEntityEntry.Property(x => x.CreatedByFullName).IsModified = false;
                }

                #region Save Log
                try
                {
                    string className = entity.GetType().Name;
                    var trackingLog = new TrackingLog
                    {
                        Id = Guid.NewGuid(),
                        ItemId = entity.Id,
                        ItemType = className,
                        Action = (int)ActionEnum.UPDATE,
                        Created = DateTime.Now,
                        CreatedById = CurrentUser.Id,
                        CreatedBy = CurrentUser.LoginName,
                        CreatedByFullName = CurrentUser.FullName,
                        Modified = DateTime.Now,
                        ModifiedById = CurrentUser.Id,
                        ModifiedBy = CurrentUser.LoginName,
                        ModifiedByFullName = CurrentUser.FullName
                    };
                    _context.Set<TrackingLog>().Add(trackingLog);
                }
                catch (Exception e) { }
                #endregion
            }

            if (entity is ISoftDeleteEntity deleteEntity)
            {
                var auditableEntityEntry = _context.Entry(deleteEntity);
                auditableEntityEntry.Property(x => x.IsDeleted).IsModified = false;
            }
            return entity;
        }
        public void Delete(T entity)
        {
            if (entity is IEntity soft)
            {
                soft.Modified = DateTime.Now;
            }

            if (CurrentUser != null)
            {
                if (entity is IAuditableEntity softEntity)
                {

                    softEntity.ModifiedById = CurrentUser.Id;
                    softEntity.ModifiedBy = CurrentUser.LoginName;
                    softEntity.ModifiedByFullName = CurrentUser.FullName;
                    var dbEntityEntry = _context.Entry(entity);
                    dbEntityEntry.State = EntityState.Modified;
                }
                if (entity is ISoftDeleteEntity softDeleteEntity)
                {
                    softDeleteEntity.IsDeleted = true;
                }
                else
                {
                    var dbEntityEntry = _context.Entry(entity);
                    dbEntityEntry.State = EntityState.Deleted;
                }
            }
            else
            {
                var dbEntityEntry = _context.Entry(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }

            #region Save Log
            if (CurrentUser != null)
            {
                try
                {
                    string className = entity.GetType().Name;
                    var trackingLog = new TrackingLog
                    {
                        Id = Guid.NewGuid(),
                        ItemId = entity.Id,
                        ItemType = className,
                        Action = (int)ActionEnum.DELETE,
                        Created = DateTime.Now,
                        CreatedById = CurrentUser.Id,
                        CreatedBy = CurrentUser.LoginName,
                        CreatedByFullName = CurrentUser.FullName,
                        Modified = DateTime.Now,
                        ModifiedById = CurrentUser.Id,
                        ModifiedBy = CurrentUser.LoginName,
                        ModifiedByFullName = CurrentUser.FullName
                    };
                    _context.Set<TrackingLog>().Add(trackingLog);
                }
                catch (Exception e) { }
            }
            #endregion

        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
        public virtual Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public virtual async Task<T?> GetById(Guid Id)
        {
            return await _dbSet.FindAsync(Id);
        }
        public virtual Task<bool> UpSert(T entity)
        {
            throw new NotImplementedException();
        }
        private IQueryable<T> Query
        {
            get
            {
                var query = _context.Set<T>().AsQueryable();
                if (!_forceAllItems)
                {
                    if (typeof(Document).IsAssignableFrom(typeof(T)) || typeof(TaskManagement).IsAssignableFrom(typeof(T)))
                    {
                        query = query
                            .GroupJoin(_context.Set<Permission>().AsQueryable(), t => t.Id, p => p.ItemId, (t, p) => new { t, p })
                            .Where(x => x.p.Any(u => u.UserId == CurrentUser.Id) || x.p.Any(d => d.Department.UserDepartmentMappings.Any(u => u.UserId == CurrentUser.Id && u.UserDepartmentRoleMappings.Any(j => j.RoleId == d.RoleId))))
                            .Select(x => x.t);
                    }
                }
                if (typeof(ISoftDeleteEntity).IsAssignableFrom(typeof(T)))
                {
                    query = query.Where("IsDeleted=@0", new object[] { false });
                }
                return query;
            }
        }
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            query = query.OrderBy(order);
            if (predicate == null)
            {
                return await query.FirstOrDefaultAsync();
            }
            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public IEnumerable<T> GetAll(string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.OrderBy(order).AsEnumerable();
        }

        public IEnumerable<T> GetAll(string order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).AsEnumerable();
        }

        /*public async Task<IEnumerable<T>> GetAllAsync()
        {
            IQueryable<T> query = Query;
            return await query.ToListAsync();
        }*/

        /*public async Task<IEnumerable<T>> GetAllAsync(string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.OrderBy(order).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<TKey>(Expression<Func<T, TKey>> order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ToListAsync();
        }*/
        public T GetSingle(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            query = query.OrderBy(order);
            if (predicate == null)
            {
                return query.FirstOrDefault();
            }
            return query.Where(predicate).FirstOrDefault();
        }

        public async Task<T> GetSingleAsync(string predicate, object[] parameters, string order = "")
        {
            return await Query.Where(predicate, parameters).OrderBy(order).FirstOrDefaultAsync();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.Where(predicate).OrderBy(order).AsEnumerable();
        }
        public IEnumerable<T> FindBy(string order, int pageIndex, int limit, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.Where(predicate).OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).AsEnumerable();
        }
        public IEnumerable<T> FindBy(string predicate, object[] parameters, string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.Where<T>(predicate, parameters).OrderBy(order).AsEnumerable();
        }
        public IEnumerable<T> FindBy(string order, int pageIndex, int limit, string predicate, object[] parameters, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.Where(predicate, parameters).OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).AsEnumerable();
        }
        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.Where(predicate).OrderBy(order).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindByAsync(string order, int pageIndex, int limit, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.Where(predicate).OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindByAsync(string predicate, object[] parameters, string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.Where(predicate, parameters).OrderBy(order).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindByAsync(string order, int pageIndex, int limit, string predicate, object[] parameters, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.Where(predicate, parameters).OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ToListAsync();
        }
        public async Task<IEnumerable<H>> FindByAsync<H>(string order, int pageIndex, int limit, string predicate, object[] parameters, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.Where(predicate, parameters).OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ProjectTo<H>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public IEnumerable<H> GetAll<H>(string order = "")
        {
            return Query.OrderBy(order).ProjectTo<H>(_mapper.ConfigurationProvider).AsEnumerable();
        }
        public IEnumerable<H> GetAll<H>(string order, int pageIndex, int limit)
        {
            return Query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ProjectTo<H>(_mapper.ConfigurationProvider).AsEnumerable();
        }
        /*public async Task<IEnumerable<H>> GetAllAsync<H>(string order = "")
        {
            return await Query.OrderBy(order).ProjectTo<H>().ToListAsync();
        }
        public async Task<IEnumerable<H>> GetAllAsync<H>(string order, int pageIndex, int limit)
        {
            return await Query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ProjectTo<H>().ToListAsync();
        }
        public async Task<IEnumerable<H>> GetAllAsync<H, TKey>(Expression<Func<T, TKey>> order, int pageIndex, int limit)
        {
            return await Query.OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ProjectTo<H>().ToListAsync();
        }*/
        public T FindById(Guid id)
        {
            IQueryable<T> query = Query;
            return query.FirstOrDefault(x => x.Id == id);
        }
        public async Task<T> FindByIdAsync(Guid id)
        {
            IQueryable<T> query = Query;
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<T> FindByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Query;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }
        public H GetSingle<H>(Expression<Func<T, bool>> predicate, string order = "")
        {
            if (predicate == null)
            {
                return Query.OrderBy(order).ProjectTo<H>(_mapper.ConfigurationProvider).FirstOrDefault();
            }
            return Query.Where(predicate).OrderBy(order).ProjectTo<H>(_mapper.ConfigurationProvider).FirstOrDefault();
        }
        public async Task<H> GetSingleAsync<H>(Expression<Func<T, bool>> predicate, string order = "")
        {
            IQueryable<T> query = Query;
            if (predicate == null)
            {
                return await Query.OrderBy(order).ProjectTo<H>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            }
            return await Query.Where(predicate).OrderBy(order).ProjectTo<H>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public IEnumerable<H> FindBy<H>(Expression<Func<T, bool>> predicate, string order = "")
        {
            return Query.Where(predicate).OrderBy(order).ProjectTo<H>(_mapper.ConfigurationProvider).AsEnumerable();
        }
        public IEnumerable<H> FindBy<H>(string order, int pageIndex, int limit, Expression<Func<T, bool>> predicate)
        {
            return Query.Where(predicate).OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ProjectTo<H>(_mapper.ConfigurationProvider).AsEnumerable();
        }

        public IEnumerable<H> FindBy<H>(string predicate, object[] parameters, string order = "")
        {
            return Query.Where<T>(predicate, parameters).OrderBy(order).ProjectTo<H>(_mapper.ConfigurationProvider).AsEnumerable();
        }
        public IEnumerable<H> FindBy<H>(string order, int pageIndex, int limit, string predicate, object[] parameters)
        {
            return Query.Where<T>(predicate, parameters).OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ProjectTo<H>(_mapper.ConfigurationProvider).AsEnumerable();
        }
        public async Task<IEnumerable<H>> FindByAsync<H>(string predicate, object[] parameters, string order = "")
        {
            return await Query.Where<T>(predicate, parameters).OrderBy(order).ProjectTo<H>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<IEnumerable<H>> FindByAsync<H>(string order, int pageIndex, int limit, string predicate, object[] parameters)
        {
            return await Query.Where<T>(predicate, parameters).OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ProjectTo<H>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<IEnumerable<H>> FindByAsync<H>(Expression<Func<T, bool>> predicate, string order = "")
        {
            return await Query.Where(predicate).OrderBy(order).ProjectTo<H>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<IEnumerable<H>> FindByAsync<H>(string order, int pageIndex, int limit, Expression<Func<T, bool>> predicate)
        {
            return await Query.Where(predicate).OrderBy(order).Skip((pageIndex - 1) * limit).Take(limit).ProjectTo<H>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<IEnumerable<H>> FindByAsync<H, TKey>(Expression<Func<T, TKey>> order, int pageIndex, int limit, Expression<Func<T, bool>> predicate)
        {
            return await Query.Where(predicate)
                 .OrderBy(order)
                 .Skip((pageIndex - 1) * limit)
                 .Take(limit)
                 .ProjectTo<H>(_mapper.ConfigurationProvider)
                 .ToListAsync();
        }
        public int CountAll()
        {
            return Query.Count();
        }
        public async Task<int> CountAllAsync()
        {
            return await Query.CountAsync();
        }
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Query.Where(predicate).Count();
        }
        public int Count(string predicate, object[] parameters)
        {
            return Query.Where(predicate, parameters).Count();
        }
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return Query.Where(predicate).Any();
        }
        public bool Any(string predicate, object[] parameters)
        {
            return Query.Where(predicate, parameters).Any();
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await Query.Where(predicate).AnyAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await Query.Where(predicate).CountAsync();
        }
        public async Task<int> CountAsync(string predicate, object[] parameters)
        {
            return await Query.Where(predicate, parameters).CountAsync();
        }
        public async Task<bool> AnyAsync(string predicate, object[] parameters)
        {
            return await Query.Where(predicate, parameters).AnyAsync();
        }


        public void Load(T entity, params Expression<Func<T, object>>[] includeProperties)
        {
            foreach (var includeProperty in includeProperties)
            {
                _context.Entry(entity).Reference(includeProperty).Load();
            }

        }

        // Ham dung cho get cac record isdeleted = true
        public async Task<T> GetSingleAsyncIsNotDeleted(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            query = query.OrderBy(order);
            if (predicate == null)
            {
                return await query.FirstOrDefaultAsync();
            }
            return await query.Where(predicate).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetAllWithoutDeleted(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            if (typeof(ISoftIdEntity).IsAssignableFrom(typeof(T)))
            {
                query = query.Where("IsDeleted=@0 OR IsDeleted=@1", new object[] { false, true });
            }
            return await query.Where(predicate).OrderBy(order).ToListAsync();
        }

        private string GenerateReferenceNumber(string name)
        {
            var refNumber = string.Empty;
            lock (GlobalData.Instance.GlobalLock)
            {
                if (_refs.ContainsKey(name))
                {
                    var refEnitty = _refs[name];
                    if (refEnitty.IsNewYearReset && refEnitty.CurrentYear != DateTime.Now.Year || refEnitty.CurrentYear == 0)
                    {
                        refEnitty.CurrentNumber = 1;
                        refEnitty.CurrentYear = DateTime.Now.Year;
                    }
                    else
                    {
                        refEnitty.CurrentNumber++;
                    }
                    //Update via sql
                    _context.Database.ExecuteSqlRaw("Update [ReferenceNumbers] set CurrentNumber = @p0, CurrentYear =@p1 where Id= @p2", refEnitty.CurrentNumber, refEnitty.CurrentYear, refEnitty.Id);
                    refNumber = refEnitty.Formula;
                    var tokens = FindFieldTokens(refNumber);
                    foreach (var token in tokens)
                    {
                        switch (token.ToLower())
                        {
                            case "{year}":
                                refNumber = refNumber.Replace(token, refEnitty.CurrentYear.ToString());
                                break;
                            //For Autonumber field
                            default:
                                var tokenParts = token.Trim(new char[] { '{', '}' }).Split(new char[] { ':' });
                                if (tokenParts.Length > 1)
                                {
                                    refNumber = refNumber.Replace(token, refEnitty.CurrentNumber.ToString($"D{tokenParts[1]}"));
                                }
                                break;
                        }
                    }
                }
            }
            return refNumber;
        }

        public string GenerateNewReferenceNumber(string name)
        {
            var refNumber = string.Empty;
            if (_refs.ContainsKey(name))
            {
                var refEnitty = _context.ReferenceNumbers.FirstOrDefault(x => x.ModuleType.Equals(name));
                if (refEnitty != null)
                {
                    if (refEnitty.IsNewYearReset && refEnitty.CurrentYear != DateTime.Now.Year || refEnitty.CurrentYear == 0)
                    {
                        refEnitty.CurrentNumber = 1;
                        refEnitty.CurrentYear = DateTime.Now.Year;
                    }
                    else
                    {
                        refEnitty.CurrentNumber++;
                    }
                    //Update via sql
                    //_context.Database.ExecuteSqlRaw("Update [ReferenceNumbers] set CurrentNumber = @p0, CurrentYear =@p1 where Id= @p2", refEnitty.CurrentNumber, refEnitty.CurrentYear, refEnitty.Id);
                    refNumber = refEnitty.Formula;
                    var tokens = FindFieldTokens(refNumber);
                    foreach (var token in tokens)
                    {
                        switch (token.ToLower())
                        {
                            case "{year}":
                                refNumber = refNumber.Replace(token, refEnitty.CurrentYear.ToString());
                                break;
                            //For Autonumber field
                            default:
                                var tokenParts = token.Trim(new char[] { '{', '}' }).Split(new char[] { ':' });
                                if (tokenParts.Length > 1)
                                {
                                    refNumber = refNumber.Replace(token, refEnitty.CurrentNumber.ToString($"D{tokenParts[1]}"));
                                }
                                break;
                        }
                    }
                }
            }
            return refNumber;
        }

        public IEnumerable<string> FindTokens(string str, string pattern)
        {
            var regex = new Regex(pattern);
            var matches = regex.Matches(str);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    yield return match.Value;
                }
            }
        }
        public IEnumerable<string> FindFieldTokens(string str)
        {
            var tokens = FindTokens(str, @"\{[\d\w\s\:]*\}");
            foreach (var token in tokens)
            {
                yield return token;
            }
        }

        /*private string GenerateReferenceNumber(string name)
        {
            var refNumber = string.Empty;
            lock (GlobalData.Instance.GlobalLock)
            {
                if (_refs.ContainsKey(name))
                {
                    var refEnitty = _refs[name];


                    if (refEnitty.IsNewYearReset && refEnitty.CurrentYear != DateTime.Now.Year || refEnitty.CurrentYear == 0)
                    {
                        refEnitty.CurrentNumber = 1;
                        refEnitty.CurrentYear = DateTime.Now.Year;
                    }
                    else
                    {
                        refEnitty.CurrentNumber++;
                    }
                    //Update via sql
                    _context.Database.ExecuteSqlCommand("Update [ReferenceNumbers] set CurrentNumber = @p0, CurrentYear =@p1 where Id= @p2", refEnitty.CurrentNumber, refEnitty.CurrentYear, refEnitty.Id);
                    refNumber = refEnitty.Formula;
                    var tokens = FindFieldTokens(refNumber);
                    foreach (var token in tokens)
                    {
                        switch (token.ToLower())
                        {
                            case "{year}":
                                refNumber = refNumber.Replace(token, refEnitty.CurrentYear.ToString());
                                break;
                            //For Autonumber field
                            default:
                                var tokenParts = token.Trim(new char[] { '{', '}' }).Split(new char[] { ':' });
                                if (tokenParts.Length > 1)
                                {
                                    refNumber = refNumber.Replace(token, refEnitty.CurrentNumber.ToString($"D{tokenParts[1]}"));
                                }
                                break;
                        }
                    }
                }
            }
            return refNumber;
        }*/
    }
}
