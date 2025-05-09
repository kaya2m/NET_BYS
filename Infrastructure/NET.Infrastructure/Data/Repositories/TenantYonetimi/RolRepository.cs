using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using NET.Application.Common.Interfaces;
using NET.Domain.TenantYonetimi.Entities;
using NET.Domain.TenantYonetimi.Interfaces;
using NET.Infrastructure.Persistence;

namespace NET.Infrastructure.Data.Repositories.TenantYonetimi
{
    /// <summary>
    /// Rol repository implementasyonu
    /// </summary>
    public class RolRepository : TenantBaseRepository<Rol>, IRolRepository
    {
        protected override string TableName => "Roller";

        private static class RolQueries
        {
            public const string Insert = @"
                INSERT INTO Roller (
                    TenantId, Ad, Aciklama, CreatedDate, CreatedBy)
                VALUES (
                    @TenantId, @Ad, @Aciklama, @CreatedDate, @CreatedBy)
                RETURNING Id";

            public const string Update = @"
                UPDATE Roller 
                SET Ad = @Ad, 
                    Aciklama = @Aciklama,
                    LastModifiedDate = @LastModifiedDate, 
                    LastModifiedBy = @LastModifiedBy
                WHERE Id = @Id AND TenantId = @TenantId";

            public const string GetIzinlerByRolId = @"
                SELECT i.* FROM Izinler i
                INNER JOIN RolIzinleri ri ON i.Id = ri.IzinId
                WHERE ri.RolId = @RolId AND ri.TenantId = @TenantId";

            public const string AddRolIzin = @"
                INSERT INTO RolIzinleri (
                    TenantId, RolId, IzinId, CreatedDate, CreatedBy)
                VALUES (
                    @TenantId, @RolId, @IzinId, @CreatedDate, @CreatedBy)";

            public const string RemoveRolIzin = @"
                DELETE FROM RolIzinleri
                WHERE RolId = @RolId AND IzinId = @IzinId AND TenantId = @TenantId";

            public const string HasIzin = @"
                SELECT COUNT(1) FROM RolIzinleri ri
                INNER JOIN Izinler i ON ri.IzinId = i.Id
                WHERE ri.RolId = @RolId AND i.Kod = @IzinKodu AND ri.TenantId = @TenantId";
        }

        public RolRepository(
            ITenantConnectionFactory connectionFactory,
            IDateTime dateTime,
            ICurrentTenantService tenantService)
            : base(connectionFactory, dateTime, tenantService)
        {
        }

        public async Task<bool> ExistsByIdAsync(int id, int tenantId)
        {
            return await ExistsAsync(id, tenantId);
        }

        public override async Task<Rol> AddAsync(Rol rol)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                rol.TenantId,
                rol.Ad,
                rol.Aciklama,
                CreatedDate = _dateTime.UtcNow,
                CreatedBy = "System" // Oturum açan kullanıcıdan gelebilir
            };

            var id = await connection.ExecuteScalarAsync<int>(RolQueries.Insert, parameters);

            // Yeni id ile rol'ü getir
            return await GetByIdAsync(id, rol.TenantId);
        }

        public override async Task<bool> UpdateAsync(Rol rol)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                rol.Id,
                rol.TenantId,
                rol.Ad,
                rol.Aciklama,
                LastModifiedDate = _dateTime.UtcNow,
                LastModifiedBy = "System" // Oturum açan kullanıcıdan gelebilir
            };

            var result = await connection.ExecuteAsync(RolQueries.Update, parameters);
            return result > 0;
        }

        public async Task<IEnumerable<Izin>> GetIzinlerByRolIdAsync(int rolId, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Izin>(
                RolQueries.GetIzinlerByRolId,
                new { RolId = rolId, TenantId = tenantId });
        }

        public async Task<bool> AddRolIzinAsync(RolIzin rolIzin)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                rolIzin.TenantId,
                rolIzin.RolId,
                rolIzin.IzinId,
                CreatedDate = _dateTime.UtcNow,
                CreatedBy = "System" // Oturum açan kullanıcıdan gelebilir
            };

            var result = await connection.ExecuteAsync(RolQueries.AddRolIzin, parameters);
            return result > 0;
        }

        public async Task<bool> RemoveRolIzinAsync(int rolId, int izinId, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                RolId = rolId,
                IzinId = izinId,
                TenantId = tenantId
            };

            var result = await connection.ExecuteAsync(RolQueries.RemoveRolIzin, parameters);
            return result > 0;
        }

        public async Task<bool> HasIzinAsync(int rolId, string izinKodu, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                RolId = rolId,
                IzinKodu = izinKodu,
                TenantId = tenantId
            };

            var count = await connection.ExecuteScalarAsync<int>(RolQueries.HasIzin, parameters);
            return count > 0;
        }
    }
}