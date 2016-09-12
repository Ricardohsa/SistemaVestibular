using SisVest.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisVest.DomainModel.Abstract
{
    public interface ICursoRepository
    {
        IQueryable<Curso> cursos { get; }

        void Inserir(Curso curso);

        void Alterar(Curso curso);

        void Excluir(Curso curso);

        Curso Retornar(int iCursoId);

    }
}
