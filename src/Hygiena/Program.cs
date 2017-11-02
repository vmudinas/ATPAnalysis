using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Hygiena
{
    public class Program
    {
        //Delete me
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                // .UseUrls("http://+:5000")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}