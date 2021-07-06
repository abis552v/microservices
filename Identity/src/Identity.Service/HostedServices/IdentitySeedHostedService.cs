using System.Threading;
using System.Threading.Tasks;
using Identity.Service.Entities;
using Identity.Service.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.Service.HostedServices
{
    public class IdentitySeedHostedService : IHostedService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IdentitySettings identitySettings;

        public IdentitySeedHostedService(IServiceScopeFactory serviceScopeFactory, Microsoft.Extensions.Options.IOptions<IdentitySettings> identitySettingsOptions)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            identitySettings = identitySettingsOptions.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await CreateRoleIfNotExistsAsync(Roles.Admin, roleManager);
            await CreateRoleIfNotExistsAsync(Roles.User, roleManager);

            var adminUser = await userManager.FindByEmailAsync(identitySettings.AdminUserEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = identitySettings.AdminUserEmail, Email = identitySettings.AdminUserEmail };
                await userManager.CreateAsync(adminUser, identitySettings.AdminUserPassword);
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;


        private static async Task CreateRoleIfNotExistsAsync(string role,
                                                        RoleManager<ApplicationRole> roleManager)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role });
            }
        }
    }
}