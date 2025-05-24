using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PetTrack.Core.Config;
using PetTrack.Repositories.Base;
using PetTrack.Repositories.SeedData;
using System.Text;
using PetTrack.Core.Helpers;

namespace PetTrack.DI
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.JwtSettingsConfig(configuration);
            services.AddAuthenJwt(configuration);
            services.ConfigSwagger();
            services.ConfigCors();
            services.InitSeedData();
            services.AddFirebase();
        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PetTrackDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void AddFirebase(this IServiceCollection services)
        {
            string credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase-sdk.json");

            if (!File.Exists(credentialPath))
            {
                throw new FileNotFoundException("Firebase credential file not found!", credentialPath);
            }

            var credential = GoogleCredential.FromFile(credentialPath)
                .CreateScoped("https://www.googleapis.com/auth/cloud-platform");

            FirebaseApp app;
            if (FirebaseApp.DefaultInstance == null)
            {
                app = FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
            }
            else
            {
                app = FirebaseApp.DefaultInstance;
            }

            services.AddSingleton(credential);

            services.AddSingleton(app);
            services.AddSingleton(provider => FirebaseAuth.GetAuth(provider.GetRequiredService<FirebaseApp>()));
            services.AddSingleton(provider => FirebaseMessaging.GetMessaging(provider.GetRequiredService<FirebaseApp>()));

            services.AddSingleton<FirebaseAuthHelper>();
        }

        public static void AddAuthenJwt(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSettings jwtSettings = services.BuildServiceProvider().GetRequiredService<JwtSettings>();
            services.AddAuthentication(e =>
            {
                e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(e =>
            {
                e.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey!))

                };
                e.SaveToken = true;
                e.RequireHttpsMetadata = true;
                e.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        var result = new { message = "You do not have permission!" };
                        return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                    }
                };
            });
        }
        public static void JwtSettingsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(option =>
            {
                JwtSettings jwtSettings = new JwtSettings
                {
                    SecretKey = configuration.GetValue<string>("JwtSettings:SecretKey"),
                    Issuer = configuration.GetValue<string>("JwtSettings:Issuer"),
                    Audience = configuration.GetValue<string>("JwtSettings:Audience"),
                    AccessTokenExpirationMinutes = configuration.GetValue<int>("JwtSettings:AccessTokenExpirationMinutes")
                };
                jwtSettings.IsValid();
                return jwtSettings;
            });

        }
        public static void ConfigCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("*")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
        }
        public static void ConfigSwagger(this IServiceCollection services)
        {
            // config swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "API"

                });

                // Thêm JWT Bearer Token vào Swagger
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header sử dụng scheme Bearer.",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Name = "Authorization",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }
        public static void InitSeedData(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PetTrackDbContext>();
            var initialiser = new SeedData(context);
            initialiser.Initialise().Wait();
        }
    }
}
