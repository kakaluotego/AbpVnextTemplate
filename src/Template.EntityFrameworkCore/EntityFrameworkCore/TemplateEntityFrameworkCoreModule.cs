using Microsoft.Extensions.DependencyInjection;
using Template.Configurations;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.EntityFrameworkCore.Oracle.Devart;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace Template.EntityFrameworkCore
{
    [DependsOn(
        typeof(TemplateDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpEntityFrameworkCoreSqliteModule),
        typeof(AbpEntityFrameworkCoreOracleDevartModule)

        )]
    public class TemplateEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            //配置额外属性
            TemplateEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TemplateDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also TemplateMigrationsDbContextFactory for EF Core tooling. */
                switch (AppSettings.EnableDb)
                {
                    case "MySQL":
                        options.UseMySQL();
                        break;
                    case "SqlServer":
                        options.UseSqlServer();
                        break;
                    case "Sqlite":
                        options.UseSqlite();
                        break;
                    case "Oracle":
                        options.UseOracle();
                        break;
                    default:
                        options.UseSqlServer();
                        break;
                }
            });
        }
    }
}
