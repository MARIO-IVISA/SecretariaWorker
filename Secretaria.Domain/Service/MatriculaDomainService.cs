using Secretaria.Domain.Contracts.Datas;
using Secretaria.Domain.Contracts.Services;
using Secretaria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaria.Domain.Service
{
    public class MatriculaDomainService : IMatriculaDomainService
    {
        private readonly IMatriculaRepository _repository;

        public MatriculaDomainService(IMatriculaRepository repository)
        {
            _repository = repository;
        }

        public async Task<Matricula> Inserir(Matricula matricula)
        {
            return await _repository.Inserir(matricula);
        }
        public void Dispose()
        {
            _repository.Dispose();
        }
        public async Task<Matricula> BuscarPorAlunoCurso(Guid alunoId, Guid cursoId)
        {
            return await _repository.BuscarPorAlunoCurso(alunoId, cursoId);
        }
    }
}
