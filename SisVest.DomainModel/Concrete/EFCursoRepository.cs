using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Concrete
{
    public class EFCursoRepository :ICursoRepository
    {
        private VestContext vestContext;

        public EFCursoRepository(VestContext context)
        {
            vestContext = context;
        }

        public IQueryable<Curso> cursos => vestContext.Cursos.AsQueryable();
        
        public void Inserir(Curso curso)
        {
            var retorno = from c in cursos
                where c.sDescricao.ToUpper().Equals(curso.sDescricao)
                select c;

            if (retorno.Count() > 0)
                throw new InvalidOperationException("Curso já cadastrado com esta Descrição.");

            vestContext.Cursos.Add(curso);
            vestContext.SaveChanges();
        }

        public void Alterar(Curso curso)
        {
            var retorno = from c in cursos
                          where c.iCursoId.Equals(curso.iCursoId) 
                          && !c.sDescricao.ToUpper().Equals(curso.sDescricao)
                          select c;

            if (retorno.Count() > 0)
                throw new InvalidOperationException("Já existe um Curso cadastrado com essa descrição.");

            vestContext.SaveChanges();
        }

        public void Excluir(int iCursoId)
        {
            var result = from c in cursos
                where c.iCursoId.Equals(iCursoId)
                select c;

            if (result.Count() == 0)
                throw new InvalidOperationException("Curso não localizado no repositório.");

            vestContext.Cursos.Remove(result.FirstOrDefault());
            vestContext.SaveChanges();
        }

        public Curso Retornar(int iCursoId)
        {
            return vestContext.Cursos.Where(c => c.iCursoId == iCursoId).FirstOrDefault();
        }
    }
}
