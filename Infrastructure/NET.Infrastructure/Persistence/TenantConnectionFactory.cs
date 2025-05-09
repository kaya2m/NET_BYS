using Microsoft.Extensions.Logging;
using NET.Application.Common.Interfaces;
using NET.Infrastructure.Persistence;
using System.Data;
using System.Data.Common;

public class TenantConnectionFactory : ITenantConnectionFactory, IDisposable
{
    private readonly DapperContext _context;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly ILogger<TenantConnectionFactory> _logger;

    public TenantConnectionFactory(
        DapperContext context,
        ICurrentTenantService currentTenantService,
        ILogger<TenantConnectionFactory> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _currentTenantService = currentTenantService ?? throw new ArgumentNullException(nameof(currentTenantService));
        _logger = logger;
    }

    public IDbConnection CreateConnection()
    {
        try
        {
            var connection = _context.CreateConnection();

            if (_currentTenantService.TenantId.HasValue)
            {
                EnsureConnectionOpen(connection);
                SetTenantContext(connection);
            }

            return connection;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tenant-specific database connection");
            throw;
        }
    }

    private void EnsureConnectionOpen(IDbConnection connection)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }
    }

    private void SetTenantContext(IDbConnection connection)
    {
        using (var cmd = connection.CreateCommand())
        {
            // Use parameterized query to prevent SQL injection
            cmd.CommandText = "SET app.current_tenant_id = @TenantId;";

            var parameter = cmd.CreateParameter();
            parameter.ParameterName = "@TenantId";
            parameter.Value = _currentTenantService.TenantId.Value;

            cmd.Parameters.Add(parameter);
            cmd.ExecuteNonQuery();

            _logger.LogInformation($"Tenant context set for TenantId: {_currentTenantService.TenantId.Value}");
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}