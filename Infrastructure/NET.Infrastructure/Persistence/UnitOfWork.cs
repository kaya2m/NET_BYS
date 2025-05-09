using NET.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Infrastructure.Persistence
{
    /// <summary>
    /// UnitOfWork pattern implementasyonu.
    /// Dapper ile transaction yönetimini sağlar.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ITenantConnectionFactory _connectionFactory;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;

        public UnitOfWork(ITenantConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task BeginTransactionAsync()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            _transaction = _connection.BeginTransaction();
        }

        public async Task<bool> CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _transaction?.Commit();
                return true;
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;

                _connection?.Close();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;

            _connection?.Close();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Dapper, entity framework gibi bir context kullanmadığından,
            // bu metod asıl olarak transaction başlatmak ve işlemleri yürütmek için kullanılır.
            // Gerçek implementasyonunuzda bu metodun işlevini uyarlamanız gerekebilir.
            return 1;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _connection?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
