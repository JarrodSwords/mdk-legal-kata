using Autofac;

namespace MdkLegal.HelpDesk.Support.Infrastructure.Ef;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(AutofacModule).Assembly).AsImplementedInterfaces();
    }
}
