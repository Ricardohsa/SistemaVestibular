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
    public class EFCandidatoRepository :ICandidatoRepository
    {
        private VestContext vestContext;

        public EFCandidatoRepository(VestContext context)
        {
            vestContext = context;
        }

        public IQueryable<Candidato> candidatos => vestContext.Candidatos.AsQueryable();

        public void RealizarInscricao(Candidato candidato)
        {
            var result = vestContext.Candidatos.Where(c => c.sCpf.Equals(candidato.sCpf));

            if (result.Any())
                throw new InvalidOperationException("Já existe um Candidato usando esse CPF.");

            var result2 = vestContext.Candidatos.Where(c => c.sEmail.Equals(candidato.sEmail));

            if (result2.Any())
                throw new InvalidOperationException("Já existe um Candidato usando esse Email.");
            
            try
            {
                vestContext.Candidatos.Add(candidato);
                vestContext.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                var msgErro = string.Empty;
                var erros = vestContext.GetValidationErrors();

                msgErro = erros.SelectMany(erro => erro.ValidationErrors).Aggregate(msgErro, (current, detalheErro) => current + (detalheErro.ErrorMessage + "\n"));
                vestContext.Entry(candidato).State = EntityState.Detached;
                throw new InvalidOperationException(msgErro);
            }
        }

        public void AtualizarCadasto(Candidato candidato)
        {
            vestContext.SaveChanges();
        }

        public void ExcluirCadastro(int iCandidatoID)
        {
            var candidato = vestContext.Candidatos.FirstOrDefault(c => c.iCandidatoId.Equals(iCandidatoID));

            vestContext.Candidatos.Remove(candidato);
            vestContext.SaveChanges();
        }

        public void Aprovar(int iCandidatoID)
        {
            var candidato = vestContext.Candidatos.FirstOrDefault(c => c.iCandidatoId == iCandidatoID);
            var totalVagasPorCurso =
                vestContext.Cursos.Where(c => c.iCursoId == candidato.Curso.iCursoId)
                    .Select(c => new {c.iVagas})
                    .FirstOrDefault();

            var result = (from cur in vestContext.Cursos
                from cand in cur.CandidatosList
                where cur.iCursoId == candidato.Curso.iCursoId && cand.bAprovado
                select cand).Count();
            
            if (totalVagasPorCurso != null && result == totalVagasPorCurso.iVagas)
                throw new InvalidOperationException("O curso já está lotado e não pode mais receber aprovação");

            if (candidato != null) candidato.bAprovado = true;
            vestContext.SaveChanges();
        }

        public Candidato Retornar(int iCandidatoId)
        {
            return vestContext.Candidatos.FirstOrDefault(c => c.iCandidatoId.Equals(iCandidatoId));
        }

        public IList<Candidato> RetornarTodos()
        {
            return vestContext.Candidatos.ToList();
        }

        public IList<Candidato> RetornarCandidatossPorVestibularPorCurso(int iVestibularID, int iCursoID)
        {
            return vestContext.Candidatos.Where(
                c => c.Curso.iCursoId.Equals(iCursoID) && c.Vestibular.iVestibularId.Equals(iVestibularID)).ToList();
        }
    }
}
