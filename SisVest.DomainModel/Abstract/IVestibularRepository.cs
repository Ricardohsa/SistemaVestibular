using SisVest.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisVest.DomainModel.Abstract
{
    public interface IVestibularRepository
    {
        IQueryable<Vestibular> vestibulares { get; }

        void Inserir(Vestibular vestibular);

        void Alterar(Vestibular vestibular);

        void Excluir(int  iVestibularId);

        IList<Candidato> RetornarCandidatosPorVesntibular(int iVestibularId); 
        
    }
}
