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

        private string _msgErro;

        private string grupoEscolhido;

        public CustomAutenticacaoAttribute(string grupo)
        {
            grupoEscolhido = grupo;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!_autenticacaoProvider.Autenticado)
                _msgErro = "Voê precisa estar autenticado para acessar está pagina";
            if ( (!string.IsNullOrEmpty(grupoEscolhido))  &&
                    _autenticacaoProvider.Autenticado && _autenticacaoProvider.UsuarioAutenticado.Grupo == grupoEscolhido)
            {
                _msgErro = "Você não tem permissão para acessar esta pagina com suas credenciais";
                return false;
            }
            
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Controller.TempData["Mensagem"] = _msgErro;
        }

        
    }
}