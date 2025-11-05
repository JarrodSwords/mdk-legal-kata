using System.Reflection;
using Autofac;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using MdkLegal.HelpDesk.Support.Read;
using MdkLegal.HelpDesk.Support.Services;
using Module = Autofac.Module;
using Ticket = MdkLegal.HelpDesk.Support.Domain.Ticket;

namespace MdkLegal.HelpDesk.Support.WebApi;

public class AutofacModule : Module
{
    public static readonly Assembly[] Assemblies =
    [
        typeof(Program).Assembly, // api
        typeof(CreateTicketHandler).Assembly, // services
        typeof(Context).Assembly, // infrastructure
        typeof(FindTicket).Assembly, // read
        typeof(Ticket).Assembly // domain
    ];

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(AutofacModule).Assembly).AsImplementedInterfaces();
        builder.RegisterType<Program>().AsSelf();
    }
}
