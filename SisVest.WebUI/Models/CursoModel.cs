using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisVest.WebUI.Models
{
    public class CursoModel
    {   
       public int ICursoId { get; set; }
        
       public string SDescricao { get; set; }
        
       public int IVagas { get; set; }

       public int iTotalCandidatos{ get; set; }

       public int iTotalCandidatosAprovados { get; set; }
    }
}