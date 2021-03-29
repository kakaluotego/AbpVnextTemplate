using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Template.MultiTenancy;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Template
{
    [DependsOn(
        typeof(TemplateDomainSharedModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpEmailingModule)
    )]
    public class TemplateDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

        }
    }
}
