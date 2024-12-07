
using SurvayBasket.Service.Account;
using SurvayBasket.Service.AuthService;
using SurvayBasket.Service.ResultService;

namespace SurvayBasket.DependancyInjection
{
    public static class DepenInjection
    {
        public static IServiceCollection AddDependancyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.

            services.AddControllers();
            services.AddSwagger();



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


            services.AddExceptionHandler<ExceptionHandlerMiddlWare>();
            services.AddProblemDetails();

            // To map MailSettings class with MailSettings in appsetting file
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

            // Allow Dependancy Injection For usermanager,signinmanager,rolemanager
            services.AddIdentity<ApplicationUser, IdentityRole>()
                                          .AddEntityFrameworkStores<ApplicationDbContext>()
                                          .AddDefaultTokenProviders();

            services.AddScoped<ApplicationUser>();
            services.AddTokenValidation(configuration);
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

            return services;
        }
    }
}
