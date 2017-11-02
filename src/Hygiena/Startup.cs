using System;
using System.Threading.Tasks;
using Hygiena.Controllers;
using Infrastructure.DataContext;
using Infrastructure.Services;
using Infrastructure.Services.Abstraction;
using Infrastructure.UserModel.Models;
using Infrastructure.UserModel.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Hygiena
{
    public class Startup
    {
        private const string TokenAudience = "ExampleAudience";
        private const string TokenIssuer = "ExampleIssuer";
        private RsaSecurityKey _key;
        private TokenAuthOptions _tokenOptions;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            // To be Added for External Login providers  .AddUserSecrets();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var keyParams = RsaKeyUtils.GetRandomKey();

            // Create the key, and a set of token options to record signing credentials 
            // using that key, along with the other parameters we will need in the 
            // token controller.
            _key = new RsaSecurityKey(keyParams);
            _tokenOptions = new TokenAuthOptions
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256Signature)
            };
            
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.CookieName = ".MyApplication";
            });

            // Save the token options into an instance so they're accessible to the 
            // controller.
            services.AddSingleton(_tokenOptions);

            //services.AddNodeServices();
 
             services.AddNodeServices(options => {
                options.InvocationTimeoutMilliseconds = 3600000;
              
            });

            // Enable the use of an [Authorize("Bearer")] attribute on methods and classes to protect.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            // Add framework services.
            services.AddEntityFramework().AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                ));


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            // Add framework services.
            services.AddMvc();
            services.AddMvc(options =>
            {
                //Use 443 for deployment on Azure and different port for local IIS
                //Coment out for self hosted 
                //options.SslPort = 443;
                //options.Filters.Add(new RequireHttpsAttribute());

                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddSingleton<IConfiguration>(sp => Configuration);

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IDataService, DataService>();
         // No Sql   
         //   var serviceProvider = services.BuildServiceProvider();
        //    var  nodeServices = NodeServicesFactory.CreateNodeServices(new NodeServicesOptions(serviceProvider));
         //   services.AddTransient<IDataService, DataService>(c=> new DataService(@"filename=.\wwwroot\database\hygienaDatabase.db;", nodeServices));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Configuration.GetSection("Logging"));


            app.UseExceptionHandler("/Home/Error");


            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = _key,
                    ValidAudience = _tokenOptions.Audience,
                    ValidIssuer = _tokenOptions.Issuer,

                    // When receiving a token, check that it is still valid.
                    ValidateLifetime = true,

                    // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                    // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                    // machines which should have synchronized time, this can be set to zero. Where external tokens are
                    // used, some leeway here could be useful.
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            });

            app.UseStaticFiles();
            app.UseIdentity();
            //app.UseStatusCodePagesWithRedirects("~/errors/{0}");
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Navigation}/{action=MainView}/{id?}");
            });
        }
    }

    public class JwtIssuerOptions
    {
        /// <summary>
        ///     "iss" (Issuer) Claim
        /// </summary>
        /// <remarks>
        ///     The "iss" (issuer) claim identifies the principal that issued the
        ///     JWT.  The processing of this claim is generally application specific.
        ///     The "iss" value is a case-sensitive string containing a StringOrURI
        ///     value.  Use of this claim is OPTIONAL.
        /// </remarks>
        public string Issuer { get; set; }

        /// <summary>
        ///     "sub" (Subject) Claim
        /// </summary>
        /// <remarks>
        ///     The "sub" (subject) claim identifies the principal that is the
        ///     subject of the JWT.  The claims in a JWT are normally statements
        ///     about the subject.  The subject value MUST either be scoped to be
        ///     locally unique in the context of the issuer or be globally unique.
        ///     The processing of this claim is generally application specific.  The
        ///     "sub" value is a case-sensitive string containing a StringOrURI
        ///     value.  Use of this claim is OPTIONAL.
        /// </remarks>
        public string Subject { get; set; }

        /// <summary>
        ///     "aud" (Audience) Claim
        /// </summary>
        /// <remarks>
        ///     The "aud" (audience) claim identifies the recipients that the JWT is
        ///     intended for.  Each principal intended to process the JWT MUST
        ///     identify itself with a value in the audience claim.  If the principal
        ///     processing the claim does not identify itself with a value in the
        ///     "aud" claim when this claim is present, then the JWT MUST be
        ///     rejected.  In the general case, the "aud" value is an array of case-
        ///     sensitive strings, each containing a StringOrURI value.  In the
        ///     special case when the JWT has one audience, the "aud" value MAY be a
        ///     single case-sensitive string containing a StringOrURI value.  The
        ///     interpretation of audience values is generally application specific.
        ///     Use of this claim is OPTIONAL.
        /// </remarks>
        public string Audience { get; set; }

        /// <summary>
        ///     "nbf" (Not Before) Claim (default is UTC NOW)
        /// </summary>
        /// <remarks>
        ///     The "nbf" (not before) claim identifies the time before which the JWT
        ///     MUST NOT be accepted for processing.  The processing of the "nbf"
        ///     claim requires that the current date/time MUST be after or equal to
        ///     the not-before date/time listed in the "nbf" claim.  Implementers MAY
        ///     provide for some small leeway, usually no more than a few minutes, to
        ///     account for clock skew.  Its value MUST be a number containing a
        ///     NumericDate value.  Use of this claim is OPTIONAL.
        /// </remarks>
        public DateTime NotBefore { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     "iat" (Issued At) Claim (default is UTC NOW)
        /// </summary>
        /// <remarks>
        ///     The "iat" (issued at) claim identifies the time at which the JWT was
        ///     issued.  This claim can be used to determine the age of the JWT.  Its
        ///     value MUST be a number containing a NumericDate value.  Use of this
        ///     claim is OPTIONAL.
        /// </remarks>
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     Set the time span the token will be valid for (default is 5 min/300 seconds)
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        ///     "exp" (Expiration Time) Claim (returns IssuedAt + ValidFor)
        /// </summary>
        /// <remarks>
        ///     The "exp" (expiration time) claim identifies the expiration time on
        ///     or after which the JWT MUST NOT be accepted for processing.  The
        ///     processing of the "exp" claim requires that the current date/time
        ///     MUST be before the expiration date/time listed in the "exp" claim.
        ///     Implementers MAY provide for some small leeway, usually no more than
        ///     a few minutes, to account for clock skew.  Its value MUST be a number
        ///     containing a NumericDate value.  Use of this claim is OPTIONAL.
        /// </remarks>
        public DateTime Expiration => IssuedAt.Add(ValidFor);

        /// <summary>
        ///     "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        /// <remarks>
        ///     The "jti" (JWT ID) claim provides a unique identifier for the JWT.
        ///     The identifier value MUST be assigned in a manner that ensures that
        ///     there is a negligible probability that the same value will be
        ///     accidentally assigned to a different data object; if the application
        ///     uses multiple issuers, collisions MUST be prevented among values
        ///     produced by different issuers as well.  The "jti" claim can be used
        ///     to prevent the JWT from being replayed.  The "jti" value is a case-
        ///     sensitive string.  Use of this claim is OPTIONAL.
        /// </remarks>
        public Func<Task<string>> JtiGenerator =>
            () => Task.FromResult(Guid.NewGuid().ToString());

        /// <summary>
        ///     The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}