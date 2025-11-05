using Autofac;
using Module = Autofac.Module;

namespace MdkLegal.HelpDesk.Support.Services;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(AutofacModule).Assembly).AsImplementedInterfaces();
    }
}
