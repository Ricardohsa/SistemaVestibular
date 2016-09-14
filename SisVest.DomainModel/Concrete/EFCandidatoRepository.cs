using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Concrete
{
    public class EfCandidatoRepository :ICandidatoRepository
    {
        private VestContext _vestContext;

        public EfCandidatoRepository(VestContext context)
        {
            _vestContext = context;
        }

        public IQueryable<Candidato> Candidatos => _vestContext.Candidatos.AsQueryable();

        public void RealizarInscricao(Candidato candidato)
        {
            var result = _vestContext.Candidatos.Where(c => c.SCpf.Equals(candidato.SCpf));

            if (result.Any())
                throw new InvalidOperationException("Já existe um Candidato usando esse CPF.");

            var result2 = _vestContext.Candidatos.Where(c => c.SEmail.Equals(candidato.SEmail));

            if (result2.Any())
                throw new InvalidOperationException("Já existe um Candidato usando esse Email.");
            
            try
            {
                _vestContext.Candidatos.Add(candidato);
                _vestContext.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                var msgErro = string.Empty;
                var erros = _vestContext.GetValidationErrors();

                msgErro = erros.SelectMany(erro => erro.ValidationErrors).Aggregate(msgErro, (current, detalheErro) => current + (detalheErro.ErrorMessage + "\n"));
                _vestContext.Entry(candidato).State = EntityState.Detached;
                throw new InvalidOperationException(msgErro);
            }
        }

        public void AtualizarCadasto(Candidato candidato)
        {
            _vestContext.Entry(candidato).State = EntityState.Modified;
            _vestContext.SaveChanges();
        }

        public void ExcluirCadastro(int iCandidatoId)
        {
            var candidato = _vestContext.Candidatos.FirstOrDefault(c => c.ICandidatoId.Equals(iCandidatoId));

            _vestContext.Candidatos.Remove(candidato);
            _vestContext.SaveChanges();
        }

        public void Aprovar(int iCandidatoId)
        {
            var candidato = _vestContext.Candidatos.FirstOrDefault(c => c.ICandidatoId == iCandidatoId);
            var totalVagasPorCurso =
                _vestContext.Cursos.Where(c => c.ICursoId == candidato.Curso.ICursoId)
                    .Select(c => new {iVagas = c.IVagas})
                    .FirstOrDefault();

            var result = (from cur in _vestContext.Cursos
                from cand in cur.CandidatosList
                where cur.ICursoId == candidato.Curso.ICursoId && cand.BAprovado
                select cand).Count();
            
            if (totalVagasPorCurso != null && result == totalVagasPorCurso.iVagas)
                throw new InvalidOperationException("O curso já está lotado e não pode mais receber aprovação");

            if (candidato != null) candidato.BAprovado = true;
            _vestContext.SaveChanges();
        }

        public Candidato Retornar(int iCandidatoId)
        {
            return _vestContext.Candidatos.FirstOrDefault(c => c.ICandidatoId.Equals(iCandidatoId));
        }

        public IList<Candidato> RetornarTodos()
        {
            return _vestContext.Candidatos.ToList();
        }

        public IList<Candidato> RetornarCandidatossPorVestibularPorCurso(int iVestibularId, int iCursoId)
        {
            return _vestContext.Candidatos.Where(
                c => c.Curso.ICursoId.Equals(iCursoId) && c.Vestibular.IVestibularId.Equals(iVestibularId)).ToList();
        }
    }
}
