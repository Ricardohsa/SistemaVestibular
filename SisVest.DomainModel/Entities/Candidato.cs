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
        public int iCandidatoId { get; set; }

        [Required]
        public string sNome { get; set; }

        public string sTelefone { get; set; }

        [Required(ErrorMessage = "Informe o Email do Candidato.")]
        public string sEmail { get; set; }

        public DateTime dtNascimento { get; set; }

        public string sSenha { get; set; }

        public string sSexo { get; set; }

        [Required(ErrorMessage = "Informe o CPF do Candidato.")]
        public string  sCpf { get; set; }

        [Required(ErrorMessage = "Informe o Vestibular do Candidato.")]
        public virtual Vestibular Vestibular { get; set; }

        [Required(ErrorMessage = "Informe o Curso do Candidato.")]
        public virtual Curso Curso { get; set; }

        public bool bAprovado { get; set; }

        public override bool Equals(object obj)
        {
            var candidatoParam = (Candidato)obj;

            return this.iCandidatoId == candidatoParam.iCandidatoId || this.sCpf ==  candidatoParam.sCpf ||
                   this.sEmail == candidatoParam.sEmail;
        }

    }
}
