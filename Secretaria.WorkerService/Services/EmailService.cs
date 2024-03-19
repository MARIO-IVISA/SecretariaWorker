using MassTransit.Internals.GraphValidation;
using Projeto02.Services.Api.Helpers;
using Secretaria.WorkerService.Interfaces;
using Secretaria.WorkerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaria.WorkerService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailHelper _emailHelper;

        public EmailService(EmailHelper emailHelper)
        {
            _emailHelper = emailHelper;
        }
        public string Matricula(AlunoModel model)
        {
            DateTime dataAtual = DateTime.Now;
            var texto = $@"
        <div>
                Prezado {model.Nome},

            Gostaríamos de informar que uma nova matrícula foi realizada com sucesso em nosso sistema. Abaixo estão os detalhes da matrícula:

            Nome do Aluno: {model.Nome}
            Curso Matriculado: {model.NomeCurso}
            Data da Matrícula: {dataAtual.ToString("dd/MM/yyyy")}
            Ficamos felizes em receber este novo aluno e esperamos que a experiência dele conosco seja enriquecedora e proveitosa.
       </div>
                
            ";

            _emailHelper.Send(model.Email, "Nova Matrícula Realizada", texto);

            return "Email enviado com sucesso";
        }
        public string Aprovado(AlunoModel model)
        {
            DateTime dataAtual = DateTime.Now;
            var texto = $@"
            <div>
                    Prezado(a) {model.Nome},

                    É com grande prazer que informamos que você foi aprovado(a) no curso ""{model.NomeCurso}"". Parabéns pela sua conquista!

                    Detalhes do Curso:

                    Nome do Curso: {model.NomeCurso}
                    Data de Conclusão: {dataAtual.ToString("dd/MM/yyyy")}
           </div>
                
            ";

            _emailHelper.Send(model.Email, "Aprovação no Curso", texto);

            return "Email enviado com sucesso";
        }
        public string ReAprovado(AlunoModel model)
        {
            DateTime dataAtual = DateTime.Now;
            var texto = $@"
            <div>
                   Prezado(a) {model.Nome},

                    Lamentamos informar que você foi reprovado(a) no curso ""{model.NomeCurso}"".

                    Detalhes do Curso:

                    Nome do Curso: {model.NomeCurso}
                    Data de Reprovação: {dataAtual.ToString("dd/MM/yyyy")}
           </div>
                
            ";

            _emailHelper.Send(model.Email, "Reprovação no Curso", texto);

            return "Email enviado com sucesso";
        }

    }
}
