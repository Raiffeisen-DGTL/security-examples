using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity_Provider.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
            .AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = NegotiateDefaults.AuthenticationScheme;
            })
            .AddNegotiate(negotiateOptions =>
            {
                negotiateOptions.EnableLdap(ldapSettings =>
                {
                    ldapSettings.Domain = "raiffeisen.ru";
                    ldapSettings.MachineAccountName = "srv-t-fieldom";
                    ldapSettings.MachineAccountPassword = ",p4ZeSUkvjW.uirTIGvHP>+PE5uv(R";
                });
            });


            services
                .AddIdentityServer(Configuration.GetSection("IdentityServer"))
                .AddInMemoryApiScopes(Configuration.GetSection("IdentityServer:ApiScopes"))
                .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
                .AddInMemoryApiResources(Configuration.GetSection("IdentityServer:Resources"))
                .AddInMemoryIdentityResources(new List<IdentityResource>
                {
                            new IdentityResources.OpenId(), new IdentityResources.Profile()
                })
                .AddDeveloperSigningCredential();


            var corsOrigins = Configuration.GetSection("CorsOrigins").Get<string[]>();

            services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder
                                .WithOrigins(corsOrigins)
                                .AllowCredentials();
                        });
                    });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
