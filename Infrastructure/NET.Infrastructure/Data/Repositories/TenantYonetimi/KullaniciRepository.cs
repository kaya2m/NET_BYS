using System;
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
    /// Kullanici repository implementasyonu
    /// </summary>
    public class KullaniciRepository : TenantBaseRepository<Kullanici>, IKullaniciRepository
    {
        protected override string TableName => "Kullanicilar";

        private static class KullaniciQueries
        {
            public const string GetByEmail = @"
                SELECT * FROM Kullanicilar 
                WHERE Email = @Email AND TenantId = @TenantId";

            public const string ExistsByEmail = @"
                SELECT COUNT(1) FROM Kullanicilar 
                WHERE Email = @Email AND TenantId = @TenantId";

            public const string GetCountByTenant = @"
                SELECT COUNT(1) FROM Kullanicilar 
                WHERE TenantId = @TenantId";

            public const string Insert = @"
                INSERT INTO Kullanicilar (
                    TenantId, Ad, Soyad, Email, TelefonNo, Sifre, RolId, 
                    SonGirisTarihi, Aktif, PasswordResetToken, PasswordResetExpires, 
                    EmailDogrulandiMi, CreatedDate, CreatedBy)
                VALUES (
                    @TenantId, @Ad, @Soyad, @Email, @TelefonNo, @Sifre, @RolId, 
                    @SonGirisTarihi, @Aktif, @PasswordResetToken, @PasswordResetExpires, 
                    @EmailDogrulandiMi, @CreatedDate, @CreatedBy)
                RETURNING Id";

            public const string Update = @"
                UPDATE Kullanicilar 
                SET Ad = @Ad, 
                    Soyad = @Soyad, 
                    Email = @Email, 
                    TelefonNo = @TelefonNo, 
                    RolId = @RolId, 
                    SonGirisTarihi = @SonGirisTarihi, 
                    Aktif = @Aktif, 
                    PasswordResetToken = @PasswordResetToken, 
                    PasswordResetExpires = @PasswordResetExpires, 
                    EmailDogrulandiMi = @EmailDogrulandiMi,
                    LastModifiedDate = @LastModifiedDate, 
                    LastModifiedBy = @LastModifiedBy
                WHERE Id = @Id AND TenantId = @TenantId";

            public const string Authenticate = @"
                SELECT * FROM Kullanicilar 
                WHERE Email = @Email 
                AND Sifre = @Sifre 
                AND TenantId = @TenantId 
                AND Aktif = TRUE";

            public const string ResetPassword = @"
                UPDATE Kullanicilar 
                SET Sifre = @Sifre, 
                    PasswordResetToken = NULL, 
                    PasswordResetExpires = NULL,
                    LastModifiedDate = @LastModifiedDate, 
                    LastModifiedBy = @LastModifiedBy
                WHERE Id = @Id AND TenantId = @TenantId";

            public const string CreatePasswordResetToken = @"
                UPDATE Kullanicilar 
                SET PasswordResetToken = @Token, 
                    PasswordResetExpires = @Expires,
                    LastModifiedDate = @LastModifiedDate, 
                    LastModifiedBy = @LastModifiedBy
                WHERE Email = @Email AND TenantId = @TenantId";

            public const string GetByPasswordResetToken = @"
                SELECT * FROM Kullanicilar 
                WHERE PasswordResetToken = @Token 
                AND PasswordResetExpires > @Now
                AND TenantId = @TenantId";
        }

        public KullaniciRepository(
            ITenantConnectionFactory connectionFactory,
            IDateTime dateTime,
            ICurrentTenantService tenantService)
            : base(connectionFactory, dateTime, tenantService)
        {
        }

        public async Task<Kullanici> GetByEmailAsync(string email, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Kullanici>(
                KullaniciQueries.GetByEmail,
                new { Email = email, TenantId = tenantId });
        }

        public async Task<bool> ExistsByEmailAsync(string email, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                KullaniciQueries.ExistsByEmail,
                new { Email = email, TenantId = tenantId });
            return count > 0;
        }

        public async Task<bool> ExistsByIdAsync(int id, int tenantId)
        {
            return await ExistsAsync(id, tenantId);
        }

        public async Task<int> GetKullaniciCountByTenantIdAsync(int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                KullaniciQueries.GetCountByTenant,
                new { TenantId = tenantId });
        }

        public override async Task<Kullanici> AddAsync(Kullanici kullanici)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                kullanici.TenantId,
                kullanici.Ad,
                kullanici.Soyad,
                kullanici.Email,
                kullanici.TelefonNo,
                kullanici.Sifre,
                kullanici.RolId,
                kullanici.SonGirisTarihi,
                kullanici.Aktif,
                kullanici.PasswordResetToken,
                kullanici.PasswordResetExpires,
                kullanici.EmailDogrulandiMi,
                CreatedDate = _dateTime.UtcNow,
                CreatedBy = "System" // Oturum açan kullanıcıdan gelebilir
            };

            var id = await connection.ExecuteScalarAsync<int>(KullaniciQueries.Insert, parameters);

            // Yeni id ile kullanıcıyı getir
            return await GetByIdAsync(id, kullanici.TenantId);
        }

        public override async Task<bool> UpdateAsync(Kullanici kullanici)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                kullanici.Id,
                kullanici.TenantId,
                kullanici.Ad,
                kullanici.Soyad,
                kullanici.Email,
                kullanici.TelefonNo,
                kullanici.RolId,
                kullanici.SonGirisTarihi,
                kullanici.Aktif,
                kullanici.PasswordResetToken,
                kullanici.PasswordResetExpires,
                kullanici.EmailDogrulandiMi,
                LastModifiedDate = _dateTime.UtcNow,
                LastModifiedBy = "System" // Oturum açan kullanıcıdan gelebilir
            };

            var result = await connection.ExecuteAsync(KullaniciQueries.Update, parameters);
            return result > 0;
        }

        public async Task<Kullanici> AuthenticateAsync(string email, string hashedPassword, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Kullanici>(
                KullaniciQueries.Authenticate,
                new { Email = email, Sifre = hashedPassword, TenantId = tenantId });
        }

        public async Task<bool> ResetPasswordAsync(int id, string yeniSifreHash, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                Id = id,
                Sifre = yeniSifreHash,
                TenantId = tenantId,
                LastModifiedDate = _dateTime.UtcNow,
                LastModifiedBy = "System"
            };

            var result = await connection.ExecuteAsync(KullaniciQueries.ResetPassword, parameters);
            return result > 0;
        }

        public async Task<bool> CreatePasswordResetTokenAsync(string email, string token, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                Email = email,
                Token = token,
                Expires = _dateTime.UtcNow.AddHours(24), // Token 24 saat geçerli
                TenantId = tenantId,
                LastModifiedDate = _dateTime.UtcNow,
                LastModifiedBy = "System"
            };

            var result = await connection.ExecuteAsync(KullaniciQueries.CreatePasswordResetToken, parameters);
            return result > 0;
        }

        public async Task<Kullanici> GetByPasswordResetTokenAsync(string token, int tenantId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Kullanici>(
                KullaniciQueries.GetByPasswordResetToken,
                new { Token = token, Now = _dateTime.UtcNow, TenantId = tenantId });
        }
    }
}