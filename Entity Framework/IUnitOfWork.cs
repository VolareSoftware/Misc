using System;
using System.Data.SqlClient;

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> RepositoryFor<T>() where T : Entity;
        void Attach(Entity entity);
        void Save();
        T ExecuteSqlScalar<T>(string sql, params SqlParameter[] parameters);
    }
}