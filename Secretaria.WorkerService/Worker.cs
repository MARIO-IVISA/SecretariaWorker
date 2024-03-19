using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Secretaria.Core.Enums;
using Secretaria.WorkerService.Interfaces;
using Secretaria.WorkerService.Models;
using Secretaria.WorkerService.Services;
using System.Text;

namespace Secretaria.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IQueueClient _queueClient;
        private readonly ServiceBusClient _clientCreateNoticia;
        private readonly IEmailService _emailService;
        public Worker(ILogger<Worker> logger, IEmailService emailService)
        {
            _logger = logger;
            _clientCreateNoticia = new ServiceBusClient(AppSettings.AppSettings.ConnectionStringsMensageria);
            var servico = new ServiceCollection();
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker iniciado: {time}", DateTimeOffset.Now);

                await RegisterOnMessageHandlerAndReceiveMessages(stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
            _logger.LogInformation("Worker encerrado.");
        }

        private async Task RegisterOnMessageHandlerAndReceiveMessages(CancellationToken cancellationToken)
        {
            var receiveNews = _clientCreateNoticia.CreateReceiver(AppSettings.AppSettings.NomeFila);
            while (!cancellationToken.IsCancellationRequested)
            {
                var messageCreateNewsTask = receiveNews.ReceiveMessageAsync();

                await Task.WhenAny(messageCreateNewsTask);

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                if (messageCreateNewsTask.IsCompletedSuccessfully)
                {
                    var messageCreateNews = messageCreateNewsTask.Result;
                    if (messageCreateNews != null)
                    {
                        await ProcessCreateMember(receiveNews, messageCreateNews);
                    }
                }
            }
        }
        private async Task ProcessCreateMember(ServiceBusReceiver receiver, ServiceBusReceivedMessage message)
        {
            try
            {
                await receiver.CompleteMessageAsync(message);
                var messageString = Encoding.UTF8.GetString(message.Body);
                var matricula = JsonConvert.DeserializeObject<AlunoModel>(messageString);

                if (matricula != null)
                {
                    if (matricula.StatusAprovacao == StatusAprovacao.Pendente)
                    {
                        _emailService.Matricula(matricula);
                    }
                    else if (matricula.StatusAprovacao == StatusAprovacao.Aprovado)
                    {
                        _emailService.Aprovado(matricula);
                    }
                    else if (matricula.StatusAprovacao == StatusAprovacao.Reprovado)
                    {
                        _emailService.ReAprovado(matricula);
                    }
                    _logger.LogInformation($"Mensagem processada: {message}");
                }
                else
                {
                    _logger.LogInformation($"Erro ao cadastrar matricula!");
                }
            }
            catch (MessageLockLostException ex)
            {
                _logger.LogError($"O bloqueio da mensagem foi perdido: {ex.Message}");
            }
        }

    }
}

