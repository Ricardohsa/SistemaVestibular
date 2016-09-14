using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisVest.DomainModel.Entities
{
    [Table("tbVestibular")]
    public class Vestibular
    {
        [Key]
        public int iVestibularId { get; set; }

        [Required(ErrorMessage = "Descrição é obirgatória")]
        public string sDescricao { get; set; }

        [Required(ErrorMessage = "Data de Inicio deve ser informada.")]
        public DateTime dtInicioInscricao { get; set; }

        [Required(ErrorMessage = "Data de Final deve ser informada.")]
        public DateTime dtFimInscricao { get; set; }

        [Required(ErrorMessage = "Data da Prova deve ser informada.")]
        public DateTime dtProva { get; set; }

        public virtual ICollection<Candidato> CandidatosList { get; set; }

        public override bool Equals(object obj)
        {
            var vestibularParam = (Vestibular)obj;
            return this.iVestibularId == vestibularParam.iVestibularId || this.sDescricao == vestibularParam.sDescricao;
        }

    }
}