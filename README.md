# Worker Service de Cadastro de Matrículas

## Tags
`.net-8` `rabbitmq` `worker-service` `ddd` `cadastro` `matrículas`

Este é um Worker Service desenvolvido em .NET 8 utilizando a arquitetura de Domínio Orientado a DDD (Domain-Driven Design). O serviço recebe o cadastro de matrículas através do RabbitMQ, realiza a validação para verificar se o aluno está matriculado no curso e, em seguida, efetua o cadastro.

## Funcionamento

1. O serviço está configurado para escutar as mensagens na fila RabbitMQ específica para o cadastro de matrículas.

2. Quando uma nova mensagem é recebida, o serviço processa a matrícula, verificando se o aluno está devidamente matriculado no curso.

3. Se a matrícula for válida, o serviço a cadastra.

## Tecnologias Utilizadas

- .NET 8
- RabbitMQ
- Worker Service
- DDD (Domain-Driven Design)
