using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Template.EntityFrameworkCore
{
    [DependsOn(
        typeof(TemplateEntityFrameworkCoreModule)
        )]
    public class TemplateEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TemplateMigrationsDbContext>();
        }
    }
}
