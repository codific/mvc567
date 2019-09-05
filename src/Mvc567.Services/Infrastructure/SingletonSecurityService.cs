using System;

namespace Mvc567.Services.Infrastructure
{
    public class SingletonSecurityService : ISingletonSecurityService
    {
        private DateTime firstFailedDateTime = DateTime.Now;

        public int AdminLoginFailedAttempts { get; private set; }
        
        public void IncrementAdminLoginFailedAttempts()
        {
            if (DateTime.Now - this.firstFailedDateTime > TimeSpan.FromMinutes(5))
            {
                AdminLoginFailedAttempts = 0;
            }
            if (AdminLoginFailedAttempts == 0)
            {
                firstFailedDateTime = DateTime.Now;
            }
            AdminLoginFailedAttempts++;
        }
    }
}
