using NET.Application.Common.Interfaces;
using NET.Domain.Common;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Temel repository sınıfı.
    /// Dapper kullanarak DB-First yaklaşımı ile veritabanı operasyonlarını gerçekleştirir.
    /// </summary>
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        protected readonly ITenantConnectionFactory _connectionFactory;
        protected readonly IDateTime _dateTime;

        /// <summary>
        /// Entity'nin veritabanındaki tablo adı
        /// </summary>
        protected abstract string TableName { get; }

        /// <summary>
        /// Temel sorgularda kullanılacak sütunların listesi
        /// </summary>
        protected virtual string BaseColumns => "*";

        /// <summary>
        /// SQL sorgularının saklandığı sınıf
        /// </summary>
        protected abstract class Queries
        {
            public static string GetById => "SELECT {0} FROM {1} WHERE Id = @Id";
            public static string GetAll => "SELECT {0} FROM {1}";
            public static string Insert => "INSERT INTO {0} ({1}) VALUES ({2}) RETURNING Id";
            public static string Update => "UPDATE {0} SET {1} WHERE Id = @Id";
            public static string Delete => "DELETE FROM {0} WHERE Id = @Id";
            public static string Exists => "SELECT COUNT(1) FROM {0} WHERE Id = @Id";
        }

        public BaseRepository(ITenantConnectionFactory connectionFactory, IDateTime dateTime)
        {
            _connectionFactory = connectionFactory;
            _dateTime = dateTime;
        }

        /// <summary>
        /// Id ile entity getirir
        /// </summary>
        public virtual async Task<T> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = string.Format(Queries.GetById, BaseColumns, TableName);
            return await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
        }

        /// <summary>
        /// Tüm entity'leri getirir
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = string.Format(Queries.GetAll, BaseColumns, TableName);
            return await connection.QueryAsync<T>(query);
        }

        /// <summary>
        /// Yeni bir entity ekler
        /// </summary>
        public virtual async Task<T> AddAsync(T entity)
        {
            using var connection = _connectionFactory.CreateConnection();

            // Entity'yi veritabanına ekle
            // Konkret implemente eden sınıfların override etmesi gerekir
            throw new NotImplementedException("AddAsync metodu entity-specific sınıflar tarafından implemente edilmelidir");
        }

        /// <summary>
        /// Mevcut entity'yi günceller
        /// </summary>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            using var connection = _connectionFactory.CreateConnection();

            // Entity'yi veritabanında güncelle
            // Konkret implemente eden sınıfların override etmesi gerekir
            throw new NotImplementedException("UpdateAsync metodu entity-specific sınıflar tarafından implemente edilmelidir");
        }

        /// <summary>
        /// Entity'yi siler
        /// </summary>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = string.Format(Queries.Delete, TableName);
            var result = await connection.ExecuteAsync(query, new { Id = id });

            return result > 0;
        }

        /// <summary>
        /// Entity'nin varolup olmadığını kontrol eder
        /// </summary>
        public virtual async Task<bool> ExistsAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = string.Format(Queries.Exists, TableName);
            var count = await connection.ExecuteScalarAsync<int>(query, new { Id = id });

            return count > 0;
        }

        /// <summary>
        /// SQL sorgusu ile veri çeker
        /// </summary>
        protected async Task<IEnumerable<T>> QueryAsync(string sql, object param = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<T>(sql, param);
        }

        /// <summary>
        /// SQL sorgusu ile tek bir veri çeker veya default değer döner
        /// </summary>
        protected async Task<T> QuerySingleOrDefaultAsync(string sql, object param = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
        }

        /// <summary>
        /// SQL sorgusu ile ilk veriyi çeker veya default değer döner
        /// </summary>
        protected async Task<T> QueryFirstOrDefaultAsync(string sql, object param = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        /// <summary>
        /// SQL sorgusu ile komut çalıştırır
        /// </summary>
        protected async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, param);
        }

        /// <summary>
        /// SQL sorgusu ile scalar değer döner
        /// </summary>
        protected async Task<TResult> ExecuteScalarAsync<TResult>(string sql, object param = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<TResult>(sql, param);
        }

        /// <summary>
        /// Stored procedure çalıştırır
        /// </summary>
        protected async Task<IEnumerable<T>> ExecuteStoredProcedureAsync(string storedProcedureName, object param = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<T>(storedProcedureName, param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Stored procedure çalıştırır ve tek bir veri döner
        /// </summary>
        protected async Task<T> ExecuteStoredProcedureSingleOrDefaultAsync(string storedProcedureName, object param = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(storedProcedureName, param, commandType: CommandType.StoredProcedure);
        }
    }

    /// <summary>
    /// Multi-tenant entity'ler için temel repository sınıfı
    /// </summary>
    /// <typeparam name="T">TenantEntity tipinde bir entity</typeparam>
    public abstract class TenantBaseRepository<T> : BaseRepository<T> where T : TenantEntity
    {
        private readonly ICurrentTenantService _tenantService;

        public TenantBaseRepository(
            ITenantConnectionFactory connectionFactory,
            IDateTime dateTime,
            ICurrentTenantService tenantService)
            : base(connectionFactory, dateTime)
        {
            _tenantService = tenantService;
        }

        /// <summary>
        /// Id ve tenant id ile entity getirir
        /// </summary>
        public virtual async Task<T> GetByIdAsync(int id, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = $"SELECT {BaseColumns} FROM {TableName} WHERE Id = @Id AND TenantId = @TenantId";
            return await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id, TenantId = tenantId });
        }

        /// <summary>
        /// Tenant id ile ilişkili tüm entity'leri getirir
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllByTenantIdAsync(int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = $"SELECT {BaseColumns} FROM {TableName} WHERE TenantId = @TenantId";
            return await connection.QueryAsync<T>(query, new { TenantId = tenantId });
        }

        /// <summary>
        /// Entity'yi siler
        /// </summary>
        public virtual async Task<bool> DeleteAsync(int id, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = $"DELETE FROM {TableName} WHERE Id = @Id AND TenantId = @TenantId";
            var result = await connection.ExecuteAsync(query, new { Id = id, TenantId = tenantId });

            return result > 0;
        }

        /// <summary>
        /// Entity'nin varolup olmadığını kontrol eder
        /// </summary>
        public virtual async Task<bool> ExistsAsync(int id, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = $"SELECT COUNT(1) FROM {TableName} WHERE Id = @Id AND TenantId = @TenantId";
            var count = await connection.ExecuteScalarAsync<int>(query, new { Id = id, TenantId = tenantId });

            return count > 0;
        }

        /// <summary>
        /// SQL sorgusuna tenant filtresi ekler
        /// </summary>
        protected string WithTenantFilter(string query, bool whereExists = false)
        {
            return whereExists
                ? $"{query} AND TenantId = @TenantId"
                : $"{query} WHERE TenantId = @TenantId";
        }
    }
}
