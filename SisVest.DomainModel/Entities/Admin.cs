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

        [Required]
        public string SLogin { get; set; }

        [Required]
        public string SSenha { get; set; }

        [Required]
        public string SNomeTratamento { get; set; }

        [Required]
        public string SEmail { get; set; }

        public override bool Equals(object obj)
        {
            var adminParam = (Admin)obj;
            
            return IAdminId == adminParam.IAdminId || this.SLogin == adminParam.SLogin || this.SEmail == adminParam.SEmail;
        }

    }
}
