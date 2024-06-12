using BASE.Data.Implements;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Services.Implements;
using BASE.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BMBSOFT.GIS.ApiGateway.Configuration
{
    public class ConfigurationDependency
    {
        public static void Configuration(IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // User
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            // security matrix
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IScreenService, ScreenService>();
            services.AddScoped<IScreenRepository, ScreenRepository>();

            services.AddScoped<ISecurityMatrixService, SecurityMatrixService>();
            services.AddScoped<ISecurityMatrixRepository, SecurityMatrixRepository>();

            //log history
            services.AddScoped<ILogHistoryService, LogHistoryService>();
            services.AddScoped<ILogHistoryRepository, LogHistoryRepository>();
            //dextrack
            services.AddScoped<IVideosRepository, VideosRepository>();
            services.AddScoped<IComputerRepository, ComputerRepository>();
            services.AddScoped<IUserActionRepository, UserActionRepository>();
            services.AddScoped<IUserSessionRepository, UserSessionRepository>();
            services.AddScoped<BASE.Data.Interfaces.IUsersDTRepository, BASE.Data.Implements.UsersDTRepository>();
        }
    }
}
