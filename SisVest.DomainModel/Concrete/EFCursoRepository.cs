using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Concrete
{
    public class EfCursoRepository :ICursoRepository
    {
        private VestContext _vestContext;

        public EfCursoRepository(VestContext context)
        {
            _vestContext = context;
        }

        public IQueryable<Curso> Cursos => _vestContext.Cursos.AsQueryable();
        
        public void Inserir(Curso curso)
        {
            var retorno = from c in Cursos
                where c.SDescricao.ToUpper().Equals(curso.SDescricao)
                select c;

            if (retorno.Any())
                throw new InvalidOperationException("Curso já cadastrado com esta Descrição.");

            _vestContext.Cursos.Add(curso);
            _vestContext.SaveChanges();
        }

        public void Alterar(Curso curso)
        {
            var retorno = from c in Cursos
                          where (c.SDescricao.ToUpper().Equals(curso.SDescricao) && !c.ICursoId.Equals(curso.ICursoId))
                          select c;

            if (retorno.Any())
                throw new InvalidOperationException("Já existe um Curso cadastrado com essa descrição.");

            _vestContext.Entry(curso).State = EntityState.Modified;
            _vestContext.SaveChanges();
        }

        public void Excluir(int iCursoId)
        {
            var result = from c in Cursos
                where c.ICursoId.Equals(iCursoId)
                select c;

            if (result.Count() < 0)
                throw new InvalidOperationException("Curso não localizado no repositório.");

            _vestContext.Cursos.Remove(result.FirstOrDefault());
            _vestContext.SaveChanges();
        }

        public Curso RetornarPorId(int iCursoId)
        {
            return _vestContext.Cursos.FirstOrDefault(c => c.ICursoId == iCursoId);
        }

        public IQueryable<Candidato> CandidatosAprovadas(int iCursoId)
        {
            var result = from cur in _vestContext.Cursos
                from cand in cur.CandidatosList
                where cur.ICursoId == iCursoId && cand.BAprovado
                select cand;

            return result;
        }
    }
}
