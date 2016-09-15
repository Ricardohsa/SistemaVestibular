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
    public class EfVestibularRepository : IVestibularRepository
    {
        private VestContext _vestContext;

        public EfVestibularRepository(VestContext context)
        {
            _vestContext = context;
        }

        public IQueryable<Vestibular> Vestibulares => _vestContext.Vestibulares.AsQueryable();

        public void Inserir(Vestibular vestibular)
        {
            var retorno = from v in Vestibulares
                where v.SDescricao.ToUpper().Equals(vestibular.SDescricao)
                select v;

            if (retorno.Any())
                throw new InvalidOperationException("Vestibular já cadastrado com esta Descrição.");

            try
            {
                _vestContext.Vestibulares.Add(vestibular);
                _vestContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                var msgErro = string.Empty;
                var erros = _vestContext.GetValidationErrors();

                msgErro = erros.SelectMany(erro => erro.ValidationErrors).Aggregate(msgErro, (current, detalheErro) => current + (detalheErro.ErrorMessage + "\n"));
                _vestContext.Entry(vestibular).State = EntityState.Detached;
                throw new InvalidOperationException(msgErro);
            }
            
        }

        public void Alterar(Vestibular vestibular)
        {
            var retorno = from c in Vestibulares
                          where (c.SDescricao.ToUpper().Equals(vestibular.SDescricao) && !c.IVestibularId.Equals(vestibular.IVestibularId))
                          select c;

            if (retorno.Any())
                throw new InvalidOperationException("Já existe um Vestibular cadastrado com essa descrição.");

            _vestContext.Entry(vestibular).State = EntityState.Modified;
            _vestContext.SaveChanges();
        }

        public void Excluir(int iVestibularId)
        {
            var result = from c in Vestibulares
                         where c.IVestibularId.Equals(iVestibularId)
                         select c;

            if (result.Count() < 0)
                throw new InvalidOperationException("Vestibular não localizado no repositório.");

            var result2 = (from v in Vestibulares
                from c in v.CandidatosList
                where v.IVestibularId.Equals(iVestibularId)
                select c);

            if (result2.Any())
                throw new InvalidOperationException("Há Candidatos instritos nesse Vestibular.");
            
            _vestContext.Vestibulares.Remove(result.FirstOrDefault());
            _vestContext.SaveChanges();
        }

        public IList<Candidato> RetornarCandidatosPorVesntibular(int iVestibularId)
        {
            var result = from v in Vestibulares
                from c in v.CandidatosList
                where v.IVestibularId.Equals(iVestibularId)
                select c;

            return result.Any() ? result.ToList() : null;
        }
    }
}
