using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OAuth2._0
{
  public class Startup
  {
    public class Data
    {
      public string Email { get; set; }
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();


      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddGitHub(options =>
        {
          options.ClientId = "249af7bef21796b8c6af";
          options.ClientSecret = "c2e3eee3898571336de6d97742e832c3c505567d";

          options.ClaimActions.MapJsonKey(ClaimTypes.Country, "location");

          options.Events.OnCreatingTicket = async context =>
          {
            options.Backchannel.DefaultRequestHeaders.Authorization =
              new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, context.AccessToken);

            var response = await options.Backchannel.GetAsync(options.UserEmailsEndpoint);
            var content = await response.Content.ReadFromJsonAsync<Data[]>();

            context.Principal.AddIdentity(new ClaimsIdentity(new[]
            {
              new Claim(ClaimTypes.Email, content.First().Email)
            }));
          };

        })
        .AddCookie(options =>
        {
          options.ForwardChallenge = "GitHub";
        });


      services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
            {
              endpoints.MapControllers();
            });
    }
  }
}
