


namespace SurvayBasket.DependancyInjection
{
    public static class DepenInjection
    {
        public static IServiceCollection AddDependancyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.

            services.AddControllers();
            services.AddSwagger();
            services.ApplyRateLimiting();



            services.AddPolices(configuration);
            // Add AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Add DBContext 
            services.AddDbContext<ApplicationDbContext>(options =>
            {

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IPollsService, PollsService>();

            services.AddSingleton<ITokenService, TokenService>();

            services.AddScoped<IQuestionService, QuestionService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IResultSerevice, ResultSerevice>();
            services.AddScoped<ICachingService, CachingService>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IRoleService, RoleService>();


            services.AddExceptionHandler<ExceptionHandlerMiddlWare>();
            services.AddProblemDetails();

            // To map MailSettings class with MailSettings in appsetting file
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

            // Allow Dependancy Injection For usermanager,signinmanager,rolemanager
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                                          .AddEntityFrameworkStores<ApplicationDbContext>()
                                          .AddDefaultTokenProviders();

            services.AddScoped<ApplicationUser>();
            services.AddScoped<ApplicationRole>();
            services.AddTokenValidation(configuration);

            services.AddHealthChecks()
                    .AddDbContextCheck<ApplicationDbContext>("DataBase");



            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();



            return services;
        }

        public static IServiceCollection AddTokenValidation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(
                Options =>
                {
                    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(O =>
                {
                    O.SaveToken = true;

                    O.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidIssuer"],

                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],

                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"]!))
                    };

                });

            services.Configure<IdentityOptions>(O =>
                {
                    O.SignIn.RequireConfirmedEmail = true;
                    O.User.RequireUniqueEmail = true;

                }
                );
            return services;


        }

        public static IServiceCollection AddPolices(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(Options =>
            {
                // u can use also AddDefaultPolicy() 

                Options.AddPolicy("ServayBasketPolicy", policy =>
                {
                    policy
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .WithOrigins(allowedOrigins!);
                });

                // Another Policy
                Options.AddPolicy("AnyPolicy", policy =>
                {
                    policy
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowAnyOrigin();
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(DefaultRoles.Member, policy => policy.RequireRole(DefaultRoles.Member));
            });

            return services;
        }

        public static IServiceCollection ApplyRateLimiting(this IServiceCollection services)
        {

            return services.AddRateLimiter(Options =>
               {
                   // in case of Many Requests have been sent
                   Options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                   // AddConcurrencyLimiter Configurations 
                   Options.AddConcurrencyLimiter("concurrency", Councurrencyoptions =>
                   {
                       // Numbers of requests
                       Councurrencyoptions.PermitLimit = 100;
                       // In queue 
                       Councurrencyoptions.QueueLimit = 50;
                       // Crireria is FIFI
                       Councurrencyoptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                   });

                   // IP Limiter
                   Options.AddPolicy("IPLimiter", httpContext =>
                   RateLimitPartition.GetFixedWindowLimiter(
                  partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                  factory: _ => new FixedWindowRateLimiterOptions()
                  {
                      PermitLimit = 15,
                      Window = TimeSpan.FromSeconds(60)
                  }
                   ));

                   // User Limiter
                   Options.AddPolicy("UserLimiter", httpContext =>
                   RateLimitPartition.GetFixedWindowLimiter(
                   partitionKey: httpContext.User.GetUserId()?.ToString(),
                   factory: _ => new FixedWindowRateLimiterOptions()
                   {
                       PermitLimit = 15,
                       Window = TimeSpan.FromSeconds(60)
                   }
                   ));
               });



        }


    }
}

