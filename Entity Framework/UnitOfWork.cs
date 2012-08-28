using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EntityFrameworkContext _context;
        private readonly ICurrentUser _currentUser;

        public UnitOfWork(ICurrentUser currentUser)
        {

        }

        public IRepository<T> RepositoryFor<T>() where T : Entity
        {
            return new Repository<T>(_context, _currentUser);
        }

        public void Attach(Entity entity)
        {
            _context.Entry(entity).State = EntityState.Unchanged;
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HandleUpdateExceptions(ex);
                throw;
            }
        }

        public T ExecuteSqlScalar<T>(string sql, params SqlParameter[] parameters)
        {
            return _context.Database.SqlQuery<T>(sql, parameters).FirstOrDefault();
        }
        
        private static void HandleUpdateExceptions(Exception ex)
        {
            if (ex.InnerException != null &&
                ex.InnerException.InnerException != null &&
                ex.InnerException.InnerException is SqlException)
            {
                var exceptionMessage = ex.InnerException.InnerException.Message;
                if (exceptionMessage.Contains("unique index"))
                {
                    throw new UniqueConstraintViolationException();
                }
                if (exceptionMessage.Contains("DELETE statement conflicted with the REFERENCE") || exceptionMessage.Contains("The DELETE statement conflicted with the SAME TABLE REFERENCE"))
                {
                    throw new ForeignKeyViolationException();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }

        ~UnitOfWork()
        {
            Dispose(true);
        }
    }
}