using IziWork.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Business.IRepositories
{
    public interface IRepositoryFactory
    {
        IGenericRepository<T> GetRepository<T>() where T : class, IEntity, new();
        IGenericRepository<T> GetRepository<T>(bool forceAllItems) where T : class, IEntity, new();
    }
}
