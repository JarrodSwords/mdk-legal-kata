using System.Reflection;
using Autofac;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using MdkLegal.HelpDesk.Support.Read;
using MdkLegal.HelpDesk.Support.Services;
using Entity = MdkLegal.HelpDesk.Support.Infrastructure.Ef.Entity;
using Module = Autofac.Module;
using Ticket = MdkLegal.HelpDesk.Support.Domain.Ticket;

namespace MdkLegal.HelpDesk.Support.WebApi;

public class AutofacModule : Module
{
    public static readonly Assembly[] Assemblies =
    [
        typeof(Entity).Assembly, // kernel
        typeof(Program).Assembly, // api
        typeof(CreateTicket).Assembly, // services
        typeof(Context).Assembly, // infrastructure
        typeof(FindTicket).Assembly, // read
        typeof(Ticket).Assembly // domain
    ];

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes().AsImplementedInterfaces();
        builder.RegisterType<Program>().AsSelf();
    }
}
