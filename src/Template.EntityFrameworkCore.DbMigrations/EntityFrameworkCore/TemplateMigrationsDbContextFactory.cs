using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Template.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class TemplateMigrationsDbContextFactory : IDesignTimeDbContextFactory<TemplateMigrationsDbContext>
    {
        public TemplateMigrationsDbContext CreateDbContext(string[] args)
        {
            TemplateEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var EnableDb = configuration["ConnectionStrings:Enable"];

            var builder = new DbContextOptionsBuilder<TemplateMigrationsDbContext>();
            switch (EnableDb)
            {
                case "MySQL":
                    builder.UseMySql(configuration.GetConnectionString(EnableDb), ServerVersion.AutoDetect(configuration.GetConnectionString(EnableDb)));
                    break;
                case "SqlServer":
                    builder.UseSqlServer(configuration.GetConnectionString(EnableDb));
                    break;
                case "Oracle":
                    builder.UseOracle(configuration.GetConnectionString(EnableDb));
                    break;
                case "Sqlite":
                    builder.UseSqlite(configuration.GetConnectionString(EnableDb));
                    break;
            }

            return new TemplateMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Template.HttpApi.Host/"))
                .AddJsonFile("appsettings.json", optional: false,reloadOnChange:true);

            return builder.Build();
        }
    }
}
