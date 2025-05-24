using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Repositories.Repositories;
using PetTrack.Services.Infrastructure;
using PetTrack.Services.Services;

namespace PetTrack.Services
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices(configuration);
            services.AddRepository();
        }
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<JwtTokenGenerator>();

        }

        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
