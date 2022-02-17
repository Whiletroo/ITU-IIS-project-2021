//< !--Author xpimen00-- >
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using AutoMapper;
using Charity.API.Mappers;
using Charity.DAL;
using Charity.DAL.Entities;
using Charity.DAL.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag.AspNetCore;

namespace Charity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerDocument();

            //if (_env.IsDevelopment())
            //{
            //    services.AddDbContextFactory<CharityDbContext, CharityDbContextDevFactory>();
            //}
            //else
            //{
            services.AddDbContextFactory<CharityDbContext, CharityDbContextFactory>();
            //}

            services.AddTransient<CharityDbContext>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<CharityDbContext>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(options =>
                    options.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

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
                        ValidIssuer = "http://localhost:5000",
                        ValidAudience = "http://localhost:5000",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
                    };
                });

            services.AddSingleton(mapper);

            services.AddScoped<IRepository<AdminEntity>, AdminRepository>();
            services.AddScoped<IRepository<VolunteeringEntity>, VolunteeringRepository>();
            services.AddScoped<IRepository<EnrollmentEntity>, EnrollmentRepository>();
            services.AddScoped<IRepository<ShelterEntity>, ShelterRepository>();
            services.AddScoped<IRepository<VolunteerEntity>, VolunteerRepository>();
            services.AddScoped<IRepository<TransactionEntity>, TransactionRepository>();
            services.AddScoped<IRepository<DonationEntity>, DonationRepository>();
            services.AddScoped<IRepository<ShelterAdminEntity>, ShelterAdminRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.DocumentTitle = "Charity Portal Swagger UI";
                settings.SwaggerRoutes.Add(new SwaggerUi3Route("v1.0", "/swagger/v1/swagger.json"));
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ApplicationDbInitializer.SeedUsersRoles(userManager, roleManager);
        }
    }
}
