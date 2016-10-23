using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Dbsoft.Epm.Web.Infrastructure;
using DBSoft.EPM.DAL.Factories;
using DBSoft.EPM.Logic;
using DBSoft.EVEAPI.Crest;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Module = Autofac.Module;

namespace Dbsoft.Epm.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);

            // Create the Autofac container builder.
            var builder = new ContainerBuilder();

            // Add any Autofac modules or registrations.
            builder.RegisterModule(new AutofacModule(Configuration));

            // Populate the services.
            builder.Populate(services);

            // Build the container.
            ApplicationContainer = builder.Build();

            // Create and return the service provider.
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            var oAuthOptions = new OAuthOptions
            {
                AuthenticationScheme = "EVE Online",
                ClientId = Configuration["Authentication:EveSso:ClientId"],
                ClientSecret = Configuration["Authentication:EveSso:ClientSecret"],
                // This must not match the actual callback path. The middleware forwards the call to this endpoint to the action.
                CallbackPath = new PathString("/Account/ExternalLoginCallback"),
                AuthorizationEndpoint = "https://login.eveonline.com/oauth/authorize",
                TokenEndpoint = "https://login.eveonline.com/oauth/token",
                Scope = { "publicData" },
                UserInformationEndpoint = "https://login.eveonline.com/oauth/verify",
                Events = GetEvents(),
                AutomaticAuthenticate = true
            };
            app.UseOAuthAuthentication(oAuthOptions);


            // This needs to be last
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            CacheConfig.Configure();
            MapperConfig.ConfigureMappings();
        }

        private IOAuthEvents GetEvents()
        {
            return new OAuthEvents
            {
                OnCreatingTicket = async notification =>
                {
                    await GetValue(notification);
                }
            };
        }

        private static async Task GetValue(OAuthCreatingTicketContext notification)
        {
            var response = new AuthResponse
            {
                AccessToken = notification.AccessToken,
                RefreshToken = notification.RefreshToken,
                TokenType = notification.TokenType,
                ExpiresIn = notification.ExpiresIn.GetValueOrDefault().Minutes
            };

            var user = await UserAuth.GetUser(response);

            var claim = new Claim(ClaimTypes.NameIdentifier, user.CharacterName,
                ClaimValueTypes.String, notification.Options.ClaimsIssuer);
            notification.Identity.AddClaim(claim);
            claim = new Claim(ClaimTypes.UserData, notification.RefreshToken,
                ClaimValueTypes.String, notification.Options.ClaimsIssuer);
            notification.Identity.AddClaim(claim);
        }
    }
    public class AutofacModule : Module
    {
        private readonly IConfigurationRoot _configuration;

        public AutofacModule(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
            // It was then registered with Autofac using the Populate method in ConfigureServices.
            //builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
            //    .As<IValuesService>()
            //    .InstancePerLifetimeScope();
            builder.Register(c => _configuration).As<IConfigurationRoot>();
            // Dbsoft.Epm.Web
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces();
            // DBSoft.EPM.DAL
            builder.RegisterAssemblyTypes(typeof(EpmEntitiesFactory).Assembly)
                .AsImplementedInterfaces();
            // DBSoft.EVE.API
            builder.RegisterAssemblyTypes(typeof(UserAuth).Assembly)
                .Where(f => !f.Name.StartsWith("Mock"))
                .AsImplementedInterfaces();
            // DBSoft.EPM.Logic
            builder.RegisterAssemblyTypes(typeof(ImportManager).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterType<ImportManager>().As<IImportManager>().SingleInstance();
        }
    }
}
