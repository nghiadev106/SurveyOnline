using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SurveyOnline.API.Email;
using SurveyOnline.API.Helpers;
using SurveyOnline.API.Mappings;
using SurveyOnline.API.Middlewares;
using SurveyOnline.Application;
using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore.Identity;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Helpers;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using System;
using System.Text;

namespace SurveyOnline.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddOptions();                                      
            var mailsettings = Configuration.GetSection("MailSettings");  
            services.Configure<MailSettings>(mailsettings);
            services.AddDbContextPool<SurveyOnlineContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));

            // Conect to Database

            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Specifiying we are going to use Identity Framework
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Email Sending Service SendGrid
            services.AddSendGridEmailSender();
            // Email Sending Service MailKit
            services.AddTransient<ISendMailService, SendMailService>();

            services.AddTransient<IDatabaseHelper, DatabaseHelper>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDbFactory, DbFactory>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();          
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<ISurveyRepository, SurveyRepository>();
            services.AddTransient<ISurveyService, SurveyService>();
            services.AddTransient<IAnswerRepository, AnswerRepository>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<IUserAnswerRepository, UserAnswerRepository>();
            services.AddTransient<IUserAnswerService, UserAnswerService>();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            }); ;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SurveyOnline.API", Version = "v1" });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireLoggedIn", policy => policy.RequireRole("Admin", "Customer", "Guest").RequireAuthenticatedUser());
                options.AddPolicy("RequireAdminOrCustomer", policy => policy.RequireRole("Admin","Customer").RequireAuthenticatedUser());
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin").RequireAuthenticatedUser());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SurveyOnline.API v1"));
            }

            app.UseErrorWrapping();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
