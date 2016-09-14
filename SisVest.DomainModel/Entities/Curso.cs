using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SisVest.DomainModel.Entities
{
    [Table("tbCurso")]
    public class Curso
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ICursoId { get; set; }

        [Required]
        public string SDescricao { get; set; }

        [Required]
        public int IVagas { get; set; }

        public virtual ICollection<Candidato> CandidatosList { get; set; }

        public override bool Equals(object obj)
        {
            var cursoParam = (Curso)obj;
            return this.ICursoId == cursoParam.ICursoId || this.SDescricao == cursoParam.SDescricao;
        }
    }
}