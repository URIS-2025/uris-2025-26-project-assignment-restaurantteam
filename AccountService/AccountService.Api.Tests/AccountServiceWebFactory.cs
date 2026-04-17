using Microsoft.AspNetCore.Mvc.Testing;

namespace AccountService.AccountService.Api.Tests
{
    internal class AccountServiceWebFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //builder
            base.ConfigureWebHost(builder);
        }
    }
}
