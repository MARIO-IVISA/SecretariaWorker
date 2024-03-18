using Secretaria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaria.Domain.Contracts.Services
{
    public interface IMatriculaDomainService : IDisposable
    {
        Task<Matricula> Inserir(Matricula matricula);
        Task<Matricula> BuscarPorAlunoCurso(Guid alunoId, Guid cursoId);
    }
}
