using Secretaria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaria.Domain.Contracts.Datas
{
    public interface IMatriculaRepository : IBaseRepository<Matricula>
    {
        Task<Matricula> BuscarPorAlunoCurso(Guid alunoId, Guid cursoId);
    }
}
