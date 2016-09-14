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
        public int IVestibularId { get; set; }

        [Required(ErrorMessage = "Descrição é obirgatória")]
        public string SDescricao { get; set; }

        [Required(ErrorMessage = "Data de Inicio deve ser informada.")]
        public DateTime DtInicioInscricao { get; set; }

        [Required(ErrorMessage = "Data de Final deve ser informada.")]
        public DateTime DtFimInscricao { get; set; }

        [Required(ErrorMessage = "Data da Prova deve ser informada.")]
        public DateTime DtProva { get; set; }

        public virtual ICollection<Candidato> CandidatosList { get; set; }

        public override bool Equals(object obj)
        {
            var vestibularParam = (Vestibular)obj;
            return this.IVestibularId == vestibularParam.IVestibularId || this.SDescricao == vestibularParam.SDescricao;
        }

    }
}