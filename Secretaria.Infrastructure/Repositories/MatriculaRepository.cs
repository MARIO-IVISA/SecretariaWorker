using Microsoft.EntityFrameworkCore;
using Secretaria.Domain.Contracts.Datas;
using Secretaria.Domain.Entities;
using Secretaria.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using XAct;

namespace Secretaria.Infrastructure.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly SqlServerContext _contexto;

        public MatriculaRepository()
        {
            _contexto = new SqlServerContext();

        }
        public async Task<Matricula> Inserir(Matricula matricula)
        {
            _contexto.Matricula.Add(matricula);
            await _contexto.SaveChangesAsync();

            return matricula;
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }
        public async Task<Matricula> BuscarPorAlunoCurso(Guid alunoId, Guid cursoId)
        {
            return await _contexto.Matricula.SingleOrDefaultAsync(x => x.AlunosId == alunoId && x.CursoId == cursoId);

        }
    }
}
