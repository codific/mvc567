namespace Codific.Mvc567.Services.Infrastructure
{
    public interface ISingletonSecurityService
    {
        int AdminLoginFailedAttempts { get; }

        void IncrementAdminLoginFailedAttempts();
    }
}
