using MassTransit;
using Secretaria.Api.Settings;
using Secretaria.Application.Contracts;
using Secretaria.Application.Services;
using Secretaria.Domain.Contracts.Datas;
using Secretaria.Domain.Contracts.Services;
using Secretaria.Domain.Entities;
using Secretaria.Domain.Service;
using Secretaria.Infrastructure.Repositories;
using Secretaria.WorkerService;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        AppSettings.ConnectionStringsMensageria = configuration.GetSection("Rabbitmq:ConnectionString").Value ?? string.Empty;
        AppSettings.NomeFila = configuration.GetSection("Rabbitmq:NomeFila").Value ?? string.Empty;
        AppSettings.ConnectionStrings = configuration.GetSection("ConnectionStrings:SecretariaConnection").Value ?? string.Empty;

        services.AddTransient<IMatriculaApplicationService, MatriculaApplicationService>();
        services.AddTransient<IMatriculaDomainService, MatriculaDomainService>();
        services.AddTransient<IMatriculaRepository, MatriculaRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
