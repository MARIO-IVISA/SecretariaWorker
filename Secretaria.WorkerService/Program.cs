using MassTransit;
using Projeto02.Services.Api.Helpers;

using Secretaria.WorkerService;
using Secretaria.WorkerService.AppSettings;
using Secretaria.WorkerService.Interfaces;
using Secretaria.WorkerService.Services;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        AppSettings.ConnectionStringsMensageria = configuration.GetSection("AzureServiceBus:ConnectionString").Value ?? string.Empty;
        AppSettings.NomeFila = configuration.GetSection("AzureServiceBus:NomeFila").Value ?? string.Empty;

        services.AddTransient<IEmailService, EmailService>();

        services.AddHostedService<Worker>();

        services.AddTransient<EmailHelper>(
            map => new EmailHelper(
                    configuration.GetValue<string>("EmailSerrings:Conta"),
                    configuration.GetValue<string>("EmailSerrings:Senha"),
                    configuration.GetValue<string>("EmailSerrings:Smtp"),
                    configuration.GetValue<int>("EmailSerrings:Porta")
                )
);
    })

    .Build();

await host.RunAsync();
