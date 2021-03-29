using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Template
{
    [DependsOn(
        typeof(TemplateDomainModule),
        typeof(TemplateApplicationContractsModule),
        typeof(AbpIdentityApplicationModule)
    )]
    public class TemplateApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TemplateApplicationModule>(validate:true);
                options.AddProfile<TemplateApplicationAutoMapperProfile>(validate:true);
            });
        }
    }
}
