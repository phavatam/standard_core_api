using IziWork.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Repositories.Business.IRepositories
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetById(Guid Id);
        T Add(T Entity);
        T? Update(T Entity);
        void Delete(T Entity);
        void Delete(IEnumerable<T> entities);
        Task<bool> UpSert(T entity);
        Task<T> FindByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);
        T FindById(Guid id);
        Task<T> FindByIdAsync(Guid id);
        Task<H> GetSingleAsync<H>(Expression<Func<T, bool>> predicate, string order = "");
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties);
        T GetSingle(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll(string order = "", params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll(string order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties);
        /*Task<IEnumerable<T>> GetAllAsync(string order = "", params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllAsync(string order, int pageIndex, int limit, params Expression<Func<T, object>>[] includeProperties);*/
        Task<IEnumerable<H>> FindByAsync<H>(Expression<Func<T, bool>> predicate, string order = "");
        IEnumerable<H> FindBy<H>(Expression<Func<T, bool>> predicate, string order = "");
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> FindByAsync(string order, int pageIndex, int limit, string predicate, object[] parameters, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, string order = "", params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<H>> FindByAsync<H>(string order, int pageIndex, int limit, Expression<Func<T, bool>> predicate);
        Task<IEnumerable<H>> FindByAsync<H, TKey>(Expression<Func<T, TKey>> order, int pageIndex, int limit, Expression<Func<T, bool>> predicate);
        IEnumerable<H> FindBy<H>(string predicate, object[] parameters, string order = "");
        IEnumerable<H> FindBy<H>(string order, int pageIndex, int limit, string predicate, object[] parameters);
        Task<IEnumerable<H>> FindByAsync<H>(string predicate, object[] parameters, string order = "");
        Task<IEnumerable<H>> FindByAsync<H>(string order, int pageIndex, int limit, string predicate, object[] parameters);
        int CountAll();
        Task<int> CountAllAsync();
        int Count(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(string predicate, object[] parameters);
        string GenerateNewReferenceNumber(string predicate);
    }
}
