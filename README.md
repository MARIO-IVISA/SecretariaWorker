# Worker Service de Cadastro de Matrículas

## Tags
`.net-8` `rabbitmq` `worker-service` `ddd` `cadastro` `matrículas`

Este é um Worker Service desenvolvido em .NET 8 que utiliza o Azure Service Bus. O serviço recebe os dados do aluno e do curso por meio do Azure Service Bus, realiza a validação para verificar o status do aluno e, em seguida, efetua o envio de e-mail.

## Funcionamento

1. O serviço está configurado para escutar as mensagens na fila específica do Azure Service Bus para o envio de e-mail.

2. Quando uma nova mensagem é recebida, o serviço processa o e-mail e o envia conforme o status do aluno: matriculado, aprovado ou reprovado.

## Tecnologias Utilizadas

- .NET 8
- Azure Service Bus
- Worker Service
