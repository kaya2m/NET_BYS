using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Application.Common.Interfaces;
using NET.Domain.FinansalYonetim.Interfaces;
using NET.Domain.IletisimYonetimi.Interfaces;
using NET.Domain.SiteYonetimi.Interfaces;
using NET.Domain.TenantYonetimi.Interfaces;
using NET.Infrastructure.Common.Services;
using NET.Infrastructure.Data.Repositories.TenantYonetimi;
using NET.Infrastructure.MultiTenant;
using NET.Infrastructure.Persistence;

namespace NET.Infrastructure
{
    /// <summary>
    /// Infrastructure katmanı servislerini yapılandıran sınıf
    /// </summary>
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Veritabanı bağlantı servisleri
            services.AddSingleton<DapperContext>();
            services.AddScoped<ITenantConnectionFactory, TenantConnectionFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Yardımcı servisler
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddScoped<ICurrentTenantService, CurrentTenantService>();

            // TenantYonetimi Repository'leri
            services.AddScoped<IKullaniciRepository, KullaniciRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            //services.AddScoped<IIzinRepository, IzinRepository>();

            //// SiteYonetimi Repository'leri
            //services.AddScoped<ISiteRepository, SiteRepository>();
            //services.AddScoped<IBlokRepository, BlokRepository>();
            //services.AddScoped<IDaireRepository, DaireRepository>();
            //services.AddScoped<ISakinRepository, SakinRepository>();

            //// FinansalYonetim Repository'leri
            //services.AddScoped<IAidatRepository, AidatRepository>();
            //services.AddScoped<IGiderRepository, GiderRepository>();
            //services.AddScoped<IOdemeRepository, OdemeRepository>();

            //// IletisimYonetimi Repository'leri
            //services.AddScoped<IDuyuruRepository, DuyuruRepository>();
            //services.AddScoped<ITalepRepository, TalepRepository>();
            //services.AddScoped<IBildirimRepository, BildirimRepository>();

            return services;
        }
    }
}