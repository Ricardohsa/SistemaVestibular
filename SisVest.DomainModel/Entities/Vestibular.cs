using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SisVest.DomainModel.Entities
{
    [Table("tbVestibular")]
    public class Vestibular
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int IVestibularId { get; set; }

        [Required(ErrorMessage = "Descrição é obirgatória")]
        [Display(Name = "Descrição")]
        public string SDescricao { get; set; }

        [Required(ErrorMessage = "Data de Inicio deve ser informada.")]
        [Display(Name = "Inicio da Inscrição")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DtInicioInscricao { get; set; }

        [Required(ErrorMessage = "Data de Final deve ser informada.")]
        [Display(Name = "Fim da Inscrição")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DtFimInscricao { get; set; }

        [Required(ErrorMessage = "Data da Prova deve ser informada.")]
        [Display(Name = "Data da Prova")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DtProva { get; set; }

        public virtual ICollection<Candidato> CandidatosList { get; set; }

        public override bool Equals(object obj)
        {
            var vestibularParam = (Vestibular)obj;
            return this.IVestibularId == vestibularParam.IVestibularId || this.SDescricao == vestibularParam.SDescricao;
        }

    }
}