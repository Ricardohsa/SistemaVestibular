
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SisVest.WebUI.Models
{
    public class AutenticacaoModel
    {
        [Required(ErrorMessage = "Login é obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Grupo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string SNomeTratamento { get; set; }

    }
}