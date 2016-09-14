using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisVest.DomainModel.Entities
{
    [Table("tbAdmin")]
    public class Admin
    {
        [Key]
        public int iAdminId { get; set; }

        [Required]
        public string sLogin { get; set; }

        [Required]
        public string sSenha { get; set; }

        [Required]
        public string sNomeTratamento { get; set; }

        [Required]
        public string sEmail { get; set; }

        public override bool Equals(object obj)
        {
            var adminParam = (Admin)obj;
            
            return iAdminId == adminParam.iAdminId || this.sLogin == adminParam.sLogin || this.sEmail == adminParam.sEmail;
        }

    }
}
