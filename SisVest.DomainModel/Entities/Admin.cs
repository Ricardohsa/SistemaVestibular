using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisVest.DomainModel.Entities
{
    public class Admin
    {
        public int iAdminId { get; set; }

        public string sLogin { get; set; }

        public string sSenha { get; set; }

        public string sNomeTratamento { get; set; }

        public string sEmail { get; set; }

        public override bool Equals(object obj)
        {
            var adminParam = (Admin)obj;
            
            if (this.iAdminId == adminParam.iAdminId || this.sLogin == adminParam.sLogin || this.sEmail == adminParam.sEmail)

                return true;

            return false;
        }

    }
}
