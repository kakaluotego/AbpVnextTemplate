using Localization.Resources.AbpUi;
using Template.Localization;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Template
{
    [DependsOn(
        typeof(TemplateApplicationContractsModule),
        typeof(AbpIdentityHttpApiModule)
    )]
    public class TemplateHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<TemplateResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
