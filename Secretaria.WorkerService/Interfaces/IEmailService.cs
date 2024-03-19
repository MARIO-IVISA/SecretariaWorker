using Secretaria.WorkerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaria.WorkerService.Interfaces
{
    public interface IEmailService
    {
        public string Matricula(AlunoModel model);
        public string Aprovado(AlunoModel model);
        public string ReAprovado(AlunoModel model);

    }
}
