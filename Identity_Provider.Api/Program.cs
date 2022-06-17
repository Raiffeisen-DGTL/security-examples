using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Identity_Provider.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.ConfigureKestrel(options =>
              {
                options.ConfigureHttpsDefaults(httpsOptions =>
                    {
                      httpsOptions.ServerCertificate = new X509Certificate2("C:\\aspnetapp.pfx", "crypticpassword");
                      httpsOptions.SslProtocols = SslProtocols.Tls12;
                    });
              });

              webBuilder.UseStartup<Startup>();
            });
  }
}
