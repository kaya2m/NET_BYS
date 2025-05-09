namespace NET.Application.Common.Interfaces
{
    /// <summary>
    /// Mevcut kimliği doğrulanmış kullanıcı bilgilerine erişim sağlayan servis arayüzü
    /// </summary>
    public interface ICurrentUserService
    {
        int? UserId { get; }
        int? TenantId { get; }
        string UserName { get; }
        string Email { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}