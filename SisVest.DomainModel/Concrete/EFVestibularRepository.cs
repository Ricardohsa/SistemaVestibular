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
    public class EFVestibularRepository : IVestibularRepository
    {
        private VestContext vestContext;

        public EFVestibularRepository(VestContext context)
        {
            vestContext = context;
        }

        public IQueryable<Vestibular> vestibulares => vestContext.Vestibulares.AsQueryable();

        public void Inserir(Vestibular vestibular)
        {
            var retorno = from v in vestibulares
                where v.sDescricao.ToUpper().Equals(vestibular.sDescricao)
                select v;

            if (retorno.Any())
                throw new InvalidOperationException("Vestibular já cadastrado com esta Descrição.");

            try
            {
                vestContext.Vestibulares.Add(vestibular);
                vestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                var msgErro = string.Empty;
                var erros = vestContext.GetValidationErrors();

                msgErro = erros.SelectMany(erro => erro.ValidationErrors).Aggregate(msgErro, (current, detalheErro) => current + (detalheErro.ErrorMessage + "\n"));
                vestContext.Entry(vestibular).State = EntityState.Detached;
                throw new InvalidOperationException(msgErro);
            }
            
        }

        public void Alterar(Vestibular vestibular)
        {
            var retorno = from c in vestibulares
                          where (c.iVestibularId.Equals(vestibular.iVestibularId) || c.sDescricao.ToUpper().Equals(vestibular.sDescricao) )
                          select c;

            if (retorno.Any())
                throw new InvalidOperationException("Já existe um Vestibular cadastrado com essa descrição."); 

            vestContext.SaveChanges();
        }

        public void Excluir(int iVestibularId)
        {
            var result = from c in vestibulares
                         where c.iVestibularId.Equals(iVestibularId)
                         select c;

            if (result.Any())
                throw new InvalidOperationException("Vestibular não localizado no repositório.");

            var result2 = (from v in vestibulares
                from c in v.CandidatosList
                where v.iVestibularId.Equals(iVestibularId)
                select c);

            if (result2.Any())
                throw new InvalidOperationException("Há Candidatos instritos nesse Vestibular.");
            
            vestContext.Vestibulares.Remove(result.FirstOrDefault());
            vestContext.SaveChanges();
        }

        public IList<Candidato> RetornarCandidatosPorVesntibular(int iVestibularId)
        {
            var result = from v in vestibulares
                from c in v.CandidatosList
                where v.iVestibularId.Equals(iVestibularId)
                select c;

            return result.Any() ? result.ToList() : null;
        }
    }
}
