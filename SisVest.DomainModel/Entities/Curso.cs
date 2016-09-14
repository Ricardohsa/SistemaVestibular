using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisVest.DomainModel.Entities
{
    [Table("tbCurso")]
    public class Curso
    {
        [Key]
        public int iCursoId { get; set; }

        [Required]
        public string sDescricao { get; set; }

        [Required]
        public int iVagas { get; set; }

        public virtual ICollection<Candidato> CandidatosList { get; set; }

        public override bool Equals(object obj)
        {
            var cursoParam = (Curso)obj;
            return this.iCursoId == cursoParam.iCursoId || this.sDescricao == cursoParam.sDescricao;
        }
    }
}