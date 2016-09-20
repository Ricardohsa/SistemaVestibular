using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SisVest.DomainModel.Entities
{
    [Table("tbAdmin")]
    public class Admin
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int IAdminId { get; set; }

        [Required(ErrorMessage = "Login é obrigatório.")]
        [Display(Name = "Login")]
        public string SLogin { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório.")]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        public string SSenha { get; set; }

        [Required(ErrorMessage = "Nome do usuário é obrigatório.")]
        [Display(Name = "Nome do Usuário")]
        public string SNomeTratamento { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        [Display(Name = "Email")]
        public string SEmail { get; set; }

        public override bool Equals(object obj)
        {
            var adminParam = (Admin)obj;
            
            return IAdminId == adminParam.IAdminId || this.SLogin == adminParam.SLogin || this.SEmail == adminParam.SEmail;
        }

    }
}
