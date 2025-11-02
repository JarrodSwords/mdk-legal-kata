using Autofac;
using Autofac.Extensions.DependencyInjection;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using Microsoft.EntityFrameworkCore;
using AutofacModule = MdkLegal.HelpDesk.Support.WebApi.AutofacModule;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<Context>(
    options => { options.UseSqlServer(builder.Configuration.GetConnectionString("MdkLegal")); }
);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(
        x => x.RegisterAssemblyModules(AutofacModule.Assemblies)
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();

app.Run();
