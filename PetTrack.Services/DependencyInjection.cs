using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Repositories.Repositories;
using PetTrack.Services.Infrastructure;
using PetTrack.Services.Services;
using System.Reflection;


namespace PetTrack.Services
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices(configuration);
            services.AddRepository();
            services.AddAutoMapper();
        }
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<JwtTokenGenerator>();

        }
         private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
