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
    /// Tenant repository implementasyonu.
    /// </summary>
    public class TenantRepository : BaseRepository<Tenant>, ITenantRepository
    {
        protected override string TableName => "Tenants";

        private static class TenantQueries
        {
            public const string GetByHost = "SELECT * FROM Tenants WHERE Host = @Host";
            public const string GetByApiKey = "SELECT * FROM Tenants WHERE ApiKey = @ApiKey";
            public const string ExistsByHost = "SELECT COUNT(1) FROM Tenants WHERE Host = @Host";
            public const string Insert = @"INSERT INTO Tenants (
                    Ad, Host, ApiKey, Aciklama, Logo, AktiflestirilmeTarihi, 
                    BitisTarihi, Aktif, MaxKullaniciSayisi, MaxSiteSayisi, MaxDaireSayisi,
                    CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy)
                VALUES (
                    @Ad, @Host, @ApiKey, @Aciklama, @Logo, @AktiflestirilmeTarihi, 
                    @BitisTarihi, @Aktif, @MaxKullaniciSayisi, @MaxSiteSayisi, @MaxDaireSayisi,
                    @CreatedDate, @CreatedBy, @LastModifiedDate, @LastModifiedBy)
                RETURNING Id";
            public const string Update = @"UPDATE Tenants 
                SET Ad = @Ad, 
                    Host = @Host, 
                    ApiKey = @ApiKey, 
                    Aciklama = @Aciklama, 
                    Logo = @Logo, 
                    AktiflestirilmeTarihi = @AktiflestirilmeTarihi, 
                    BitisTarihi = @BitisTarihi, 
                    Aktif = @Aktif, 
                    MaxKullaniciSayisi = @MaxKullaniciSayisi, 
                    MaxSiteSayisi = @MaxSiteSayisi, 
                    MaxDaireSayisi = @MaxDaireSayisi,
                    LastModifiedDate = @LastModifiedDate, 
                    LastModifiedBy = @LastModifiedBy
                WHERE Id = @Id";
        }

        public TenantRepository(ITenantConnectionFactory connectionFactory, IDateTime dateTime)
            : base(connectionFactory, dateTime)
        {
        }

        public async Task<Tenant> GetByHostAsync(string host)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Tenant>(TenantQueries.GetByHost, new { Host = host });
        }

        public async Task<Tenant> GetByApiKeyAsync(string apiKey)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Tenant>(TenantQueries.GetByApiKey, new { ApiKey = apiKey });
        }

        public async Task<bool> ExistsByHostAsync(string host)
        {
            using var connection = _connectionFactory.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(TenantQueries.ExistsByHost, new { Host = host });
            return count > 0;
        }

        public override async Task<Tenant> AddAsync(Tenant tenant)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                tenant.Ad,
                tenant.Host,
                tenant.ApiKey,
                tenant.Aciklama,
                tenant.Logo,
                tenant.AktiflestirilmeTarihi,
                tenant.BitisTarihi,
                tenant.Aktif,
                tenant.MaxKullaniciSayisi,
                tenant.MaxSiteSayisi,
                tenant.MaxDaireSayisi,
                CreatedDate = _dateTime.UtcNow,
                CreatedBy = "System", // Bu kısım, oturum açan kullanıcıdan gelebilir
                LastModifiedDate = (DateTime?)null,
                LastModifiedBy = (string)null
            };

            var id = await connection.ExecuteScalarAsync<int>(TenantQueries.Insert, parameters);

            // Yeni id ile tenant'ı getir
            return await GetByIdAsync(id);
        }

        public override async Task<bool> UpdateAsync(Tenant tenant)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new
            {
                tenant.Id,
                tenant.Ad,
                tenant.Host,
                tenant.ApiKey,
                tenant.Aciklama,
                tenant.Logo,
                tenant.AktiflestirilmeTarihi,
                tenant.BitisTarihi,
                tenant.Aktif,
                tenant.MaxKullaniciSayisi,
                tenant.MaxSiteSayisi,
                tenant.MaxDaireSayisi,
                LastModifiedDate = _dateTime.UtcNow,
                LastModifiedBy = "System" // Bu kısım, oturum açan kullanıcıdan gelebilir
            };

            var result = await connection.ExecuteAsync(TenantQueries.Update, parameters);
            return result > 0;
        }
    }
}
