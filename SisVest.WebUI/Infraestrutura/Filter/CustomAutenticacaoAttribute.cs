using System.EnterpriseServices;
using System.Web;
using System.Web.Mvc;
using Ninject;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;

namespace SisVest.WebUI.Infraestrutura.Filter
{
    public class CustomAutenticacaoAttribute : AuthorizeAttribute
    {
        [Inject]
        public IAutenticacaoProvider _autenticacaoProvider { get; set; }

        private string grupoEscolhido;

        public CustomAutenticacaoAttribute(string grupo)
        {
            grupoEscolhido = grupo;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return _autenticacaoProvider.Autenticado && _autenticacaoProvider.UsuarioAutenticado.Grupo == grupoEscolhido;
        }
    }
}