using AutoMapper;
using Secretaria.Application.Contracts;
using Secretaria.Application.Models;
using Secretaria.Core.Enums;
using Secretaria.Domain.Contracts.Services;
using Secretaria.Domain.Entities;
using Secretaria.Infrastructure.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAct;

namespace Secretaria.Application.Services
{
    public class MatriculaApplicationService : IMatriculaApplicationService
    {
        private readonly IMatriculaDomainService _matriculaDomainService;
        private IMapper _mapper;
        public MatriculaApplicationService(IMatriculaDomainService matriculaDomainService, IMapper mapper)
        {
            _matriculaDomainService = matriculaDomainService;
            _mapper = mapper;
        }

        public async Task<Matricula> Inserir(MatriculaCadastroModel model)
        {
            var verificaMatricula = await _matriculaDomainService.BuscarPorAlunoCurso(model.AlunosId, model.CursoId);
            if (verificaMatricula != null)
                throw new ArgumentException("O aluno já está matriculado nesse curso.");

            Matricula noticia = _mapper.Map<Matricula>(model);
            return await _matriculaDomainService.Inserir(noticia);
        }
        public void Dispose()
        {
            _matriculaDomainService.Dispose();
        }
    }
}
