using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using BLL;
using DAL.Models;
using DAL.Core;
using DAL.Core.Interfaces;
using DAL;
using DAL.DTOs;
using DAL.Repositories.Interfaces;
using AutoMapper;
using WebApi.ViewModels;
using OpenIddict.Core;
using AspNet.Security.OpenIdConnect.Primitives;
using DotnetCorePoC.Authorization;
using OpenIddict.Abstractions;
using AppPermissions = DAL.Core.ApplicationPermissions;

namespace WebApi
{
    public class Startup
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            ConfigureDatabase(services);

            // services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //     .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;

                //    //// Password settings
                //    //options.Password.RequireDigit = true;
                //    //options.Password.RequiredLength = 8;
                //    //options.Password.RequireNonAlphanumeric = false;
                //    //options.Password.RequireUppercase = true;
                //    //options.Password.RequireLowercase = false;

                //    //// Lockout settings
                //    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //    //options.Lockout.MaxFailedAccessAttempts = 10;

                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    options.UseMvc();
                    options.EnableTokenEndpoint("/connect/token");
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    options.AcceptAnonymousClients();
                    options.DisableHttpsRequirement(); // Note: Comment this out in production

                    options.RegisterScopes(
                        OpenIdConnectConstants.Scopes.OpenId,
                        OpenIdConnectConstants.Scopes.Email,
                        OpenIdConnectConstants.Scopes.Phone,
                        OpenIdConnectConstants.Scopes.Profile,
                        OpenIdConnectConstants.Scopes.OfflineAccess,
                        OpenIddictConstants.Scopes.Roles);

                    // options.UseRollingTokens(); //Uncomment to renew refresh tokens on every refreshToken request
                    // Note: to use JWT access tokens instead of the default encrypted format, the following lines are required:
                    // options.UseJsonWebTokens();
                })
                .AddValidation(); //Only compatible with the default token format. For JWT tokens, use the Microsoft JWT bearer handler.

            services.AddAuthorization(options =>
            {
                // options.AddPolicy(DotnetCorePoC.Authorization.Policies.ViewAllUsersPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, AppPermissions.ViewUsers));
                // options.AddPolicy(DotnetCorePoC.Authorization.Policies.ManageAllUsersPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, AppPermissions.ManageUsers));

                // options.AddPolicy(DotnetCorePoC.Authorization.Policies.ViewAllRolesPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, AppPermissions.ViewRoles));
                // options.AddPolicy(DotnetCorePoC.Authorization.Policies.ViewRoleByRoleNamePolicy, policy => policy.Requirements.Add(new ViewRoleAuthorizationRequirement()));
                // options.AddPolicy(DotnetCorePoC.Authorization.Policies.ManageAllRolesPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, AppPermissions.ManageRoles));

                // options.AddPolicy(DotnetCorePoC.Authorization.Policies.AssignAllowedRolesPolicy, policy => policy.Requirements.Add(new AssignRolesAuthorizationRequirement()));
                // options.AddPolicy("admin", policy => {policy.RequireRole("administrator");});
            });

            services.AddCors(c => {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            services.AddAutoMapper(typeof(Startup));

            var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfile());
                });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen();

            // services.AddAuthentication()
            //     .AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../ClientApp/dist";
            });

            services.AddSingleton<IConfig>(new Config() {
                SavedPDFPath = _env.WebRootPath + Configuration.GetSection("Config").GetSection("SavedPDFPath").Value
            });
            services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
            services.AddScoped<IBizLogic, BizLogic>();
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            app.Use(async (context, next) =>
            {
                if (context.Request.IsHttps || context.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttps)
                {
                    await next();
                }
                else
                {
                    string queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
                    var https = "https://" + context.Request.Host + context.Request.Path + queryString;
                    context.Response.Redirect(https);
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>builder.AllowAnyOrigin());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            // app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Swagger UI";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "../ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("WebApi"));
                options.UseOpenIddict();
            });
        }
    }
}
