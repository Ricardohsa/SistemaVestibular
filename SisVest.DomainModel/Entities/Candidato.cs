using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisVest.DomainModel.Entities
{
    [Table("tbCandidato")]
    public class Candidato
    {
        [Key]
        public int ICandidatoId { get; set; }

        [Required]
        public string SNome { get; set; }

        public string STelefone { get; set; }

        [Required(ErrorMessage = "Informe o Email do Candidato.")]
        public string SEmail { get; set; }

        public DateTime DtNascimento { get; set; }

        public string SSenha { get; set; }

        public string SSexo { get; set; }

        [Required(ErrorMessage = "Informe o CPF do Candidato.")]
        public string  SCpf { get; set; }

        [Required(ErrorMessage = "Informe o Vestibular do Candidato.")]
        public virtual Vestibular Vestibular { get; set; }

        [Required(ErrorMessage = "Informe o Curso do Candidato.")]
        public virtual Curso Curso { get; set; }

        public bool BAprovado { get; set; }

        public override bool Equals(object obj)
        {
            var candidatoParam = (Candidato)obj;

            return this.ICandidatoId == candidatoParam.ICandidatoId || this.SCpf ==  candidatoParam.SCpf ||
                   this.SEmail == candidatoParam.SEmail;
        }

    }
}
