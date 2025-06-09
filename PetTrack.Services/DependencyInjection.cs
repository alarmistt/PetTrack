using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.ModelViews.Validators;
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
            services.AddValidators();
            services.AddAutoMapper();
        }
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IClinicService, ClinicService>();
            services.AddScoped<IClinicScheduleService, ClinicScheduleService>();
            services.AddScoped<IServicePackageService, ServicePackageService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IWalletTransactionService, WalletTransactionService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IDomainHelperService, DomainHelperService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<JwtTokenGenerator>();
            services.AddScoped<IPaymentService, PaymentService>();
        }
         private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<UpdateUserRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<CreateClinicRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateClinicRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<CreateClinicScheduleRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateClinicScheduleRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<CreateServicePackageRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateServicePackageRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<CreateWalletRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateWalletRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<CreateWalletTransactionRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateWalletTransactionRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<CreateBankAccountRequestValidator>();
        }
    }
}
