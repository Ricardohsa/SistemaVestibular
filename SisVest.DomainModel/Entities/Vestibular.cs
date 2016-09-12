using System;
using System.Collections.Generic;

namespace SisVest.DomainModel.Entities
{
    public class Vestibular
    {
        public int iVestibularId { get; set; }

        public string sDescricao { get; set; }

        public DateTime dtInicioInscricao { get; set; }

        public DateTime dtFimInscricao { get; set; }

        public DateTime dtProva { get; set; }

        public virtual ICollection<Candidato> CandidatosList { get; set; }

        public override bool Equals(object obj)
        {
            var vestibularParam = (Vestibular)obj;
            if (this.iVestibularId == vestibularParam.iVestibularId && this.sDescricao == vestibularParam.sDescricao) 

                return true;

            return false;
        }

    }
}