using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using webapi.Controllers;

namespace webapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
          //IntegracaoController.Iniciar();
          BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                 .UseStartup<Startup>()
                 .UseIISIntegration()
                 .Build();
    }
}
