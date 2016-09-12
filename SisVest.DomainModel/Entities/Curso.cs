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

        public string sDescricao { get; set; }

        public int iVagas { get; set; }

        public virtual ICollection<Candidato> CandidatosList { get; set; }

        public override bool Equals(object obj)
        {
            var cursoParam = (Curso)obj;
            if (this.iCursoId == cursoParam.iCursoId || this.sDescricao == cursoParam.sDescricao)              

                return true;

            return false;
        }
    }
}