using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using log4net.Repository.Hierarchy;
using Microsoft.IdentityModel.Logging;
using Template.Extensions;
using Template.Helper;

namespace Template
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteToFile(ex.Message+ex.StackTrace,ex);
                return 1;
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLog4Net()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac();
    }
}
