using AutoMapper;
using Contractor.EntityFrameworkCore;
using Contractor.Filters;
using Contractor.Identities;
using Contractor.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Contractor.DataSeeds;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Contractor.Tools;
using Contractor.Lookups;
using Contractor.Correspondences;
using Contractor.Contracts;
using Contractor.Files;
using Contractor.Projects;
using Azure.Storage.Blobs;
using Contractor.Subscriptions;
using Contractor.GeneralEntities;
using Contractor.Tenders;
using Contractor.Profiles.Tenders;
using Contractor.Tools.Memory;
using Contractor.Hubs;
using Contractor.Chathub;
using Contractor.Profiles.Chathub;

namespace Contractor.API
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.js on", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region DI
            DbContextOptionsBuilder MsSQL(DbContextOptionsBuilder builder) =>
                builder.UseSqlServer(Configuration.GetConnectionString("dbconn"), sqlServerOptions =>
                {
                    sqlServerOptions.CommandTimeout(300);
                    sqlServerOptions.MinBatchSize(1).MaxBatchSize(1000);
                    sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);

                });
            services.AddDbContext<DatabaseContext>(options => MsSQL(options));
            services.AddMemoryCache();
            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));
            services.Configure<JWTSettings>(Configuration.GetSection("JWT"));
            services.Configure<FrontEndUrls>(Configuration.GetSection("FrontEndUrls"));
            services.Configure<AzureUrls>(Configuration.GetSection("AzureConfiguration"));
            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(Configuration.GetSection("JWT").GetValue("ResetPasswordTimeLimit", 5)));
            services.AddIdentity<User, Role>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddRoles<Role>()
                .AddUserManager<UserManager>()
                .AddSignInManager<SignInManager<User>>()
                .AddDefaultTokenProviders();

            #region DI Singleton
            services.AddSingleton(x => new BlobServiceClient(Configuration["AzureConfiguration:BlobStorage:ConnectionString"]));
            #endregion

            #region DI AppServices
            services.AddScoped<ICorrespondenceAppService, CorrespondenceAppService>();
            services.AddScoped<ITenderAppService, TenderAppService>();
            services.AddScoped<ISubUserAppService, SubUserAppService>();
            services.AddScoped<ISubscriptionAppService, SubscriptionAppService>();
            services.AddScoped<IFileAppService, FileAppService>();
            services.AddScoped<IAddressAppService, AddressAppService>();
            services.AddScoped<INationalityAppService, NationalityAppService>();
            services.AddScoped<IIncomeTypeAppService, IncomeTypeAppService>();
            services.AddScoped<IOutGoingTypeAppService, OutGoingTypeAppService>();
            services.AddScoped<ILookupAppService, LookupAppService>();
            services.AddScoped<IAccountManagementAppService, AccountManagementAppService>();
            services.AddScoped<IAuthenticateAppService, AuthenticateAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            services.AddScoped<IPageAppService, PageAppService>();
            services.AddScoped<IContractTypeAppService, ContractTypeAppService>();
            services.AddScoped<IProjectTypeAppService, ProjectTypeAppService>();
            services.AddScoped<IDraftProjectAppService, DraftProjectAppService>();
            services.AddScoped<IProjectAppService, ProjectAppService>();
            services.AddScoped<IChatMessageAppService, ChatMessageAppService>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddSignalR();

            #endregion

            #region DI Managers
            services.AddScoped<IMemoryCacheManager, MemoryCacheManager>();
            services.AddScoped<IBlobManager, BlobManager>();
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            #endregion

            #region DI Repositories
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<ICorrespondenceRepository, CorrespondenceRepository>();
            services.AddScoped<ITenderRepository, TenderRepository>();
            services.AddScoped<IAccessDefinitionRepository, AccessDefinitionRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<INationalityRepository, NationalityRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IOutGoingTypeRepository, OutGoingTypeRepository>();
            services.AddScoped<IIncomeTypeRepository, IncomeTypeRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IContractTypeRepository, ContractTypeRepository>();
            services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();
            services.AddScoped<IDraftProjectRepository, DraftProjectRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            #endregion
            #endregion

            #region Cors

            //TODO to be configured to the exact front end url
            services.AddCors(o => o.AddPolicy("default", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowCredentials()
                .AllowAnyHeader();
            }));

            #endregion

            #region Authentication
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            #endregion

            #region Controllers

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Add a security definition for bearer tokens
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                // Require bearer token authentication for all API endpoints
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            services.AddControllers(config =>
            {
                config.Filters.Add(new ValidateModelAttribute());
            }).AddJsonOptions(options =>
             options.JsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter()));

            #endregion

            #region mapper

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TenderProfile());
                mc.AddProfile(new SubscriptionProfile());
                mc.AddProfile(new FileProfile());
                mc.AddProfile(new LookupProfile());
                mc.AddProfile(new CorrespondenceProfile());
                mc.AddProfile(new IdentityProfile());
                mc.AddProfile(new RoleProfile());
                mc.AddProfile(new PageProfile());
                mc.AddProfile(new ContractsProfile());
                mc.AddProfile(new ProjectsProfile());
                mc.AddProfile(new ChathubProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCustomExceptionHandler(logger);
            app.UseCors("default");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            
            app.MigrateDatabase(logger);
            
            app.UseDataSeeder(logger);
            app.UseTransaction();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });
        }

    }
}
