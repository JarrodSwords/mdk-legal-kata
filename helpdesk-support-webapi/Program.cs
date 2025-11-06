using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MdkLegal.HelpDesk.Support.Infrastructure.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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

builder.Services
    .AddRouting(
        x =>
        {
            x.LowercaseQueryStrings = true;
            x.LowercaseUrls = true;
        }
    ).AddControllers();

builder.Services
    .AddEndpointsApiExplorer() // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    .AddSwaggerGen(
        x =>
        {
            x.CustomSchemaIds(
                y =>
                {
                    var tokens = y.ToString().Split('.');
                    var sb = new StringBuilder();

                    for (var i = 2; i < tokens.Length; i++)
                        sb.AppendFormat(".{0}", tokens[i]);

                    return sb.ToString().TrimStart('.');
                }
            );
            x.OrderActionsBy(y => y.RelativePath);
            x.AddSecurityDefinition(
                "Bearer",
                new()
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                }
            );
        }
    );

builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();

app.UseSwagger(
    c => { c.RouteTemplate = "/api/swagger/{documentName}/swagger.json"; }
);
app.UseSwaggerUI(
    c =>
    {
        c.RoutePrefix = "api";
        c.SwaggerEndpoint("swagger/v1/swagger.json", "THE API V1");
        c.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
        {
            ["activated"] = false
        };
    }
);

app.UseRouting();
app.UseCors();
app.MapControllers();

app.Run();
