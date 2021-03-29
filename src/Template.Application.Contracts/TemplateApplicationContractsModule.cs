using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;

namespace Template
{
    [DependsOn(
        typeof(TemplateDomainSharedModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class TemplateApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
