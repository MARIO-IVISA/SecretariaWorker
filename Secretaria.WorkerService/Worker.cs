using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Secretaria.Api.Settings;
using Secretaria.Application.Contracts;
using Secretaria.Application.Models;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Secretaria.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMatriculaApplicationService _matriculaAppService;
        private IConnection _rabbitMQConnection;
        private IModel _channel;

        public Worker(ILogger<Worker> logger, IMatriculaApplicationService matriculaAppService)
        {
            _logger = logger;
            _matriculaAppService = matriculaAppService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(AppSettings.ConnectionStringsMensageria)
            };

            _rabbitMQConnection = factory.CreateConnection();
            _channel = _rabbitMQConnection.CreateModel();

            _channel.QueueDeclare(queue: AppSettings.NomeFila, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"Mensagem recebida: {message}");

                var matricula = JsonConvert.DeserializeObject<MatriculaCadastroModel>(message);

                if (matricula != null)
                {
                    await _matriculaAppService.Inserir(matricula);
                    _logger.LogInformation($"Mensagem processada: {message}");
                }
                else
                {
                    _logger.LogWarning($"Erro ao processar matrícula: Mensagem inválida.");
                }
            };

            _channel.BasicConsume(queue: AppSettings.NomeFila, autoAck: true, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Worker encerrado.");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMQConnection.Close();
            _channel.Close();
            await base.StopAsync(cancellationToken);
        }
    }
}
