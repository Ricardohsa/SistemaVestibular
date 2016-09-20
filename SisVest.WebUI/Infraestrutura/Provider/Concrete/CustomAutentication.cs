using System;
using System.Linq;
using System.Web;
using SisVest.DomainModel.Abstract;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using SisVest.WebUI.Models;

namespace SisVest.WebUI.Infraestrutura.Provider.Concrete
{
    public class CustomAutentication:IAutenticacaoProvider
    {
        private IAdimRepository _adimRepository;

        public CustomAutentication(IAdimRepository repository)
        {
            _adimRepository = repository;
        }

        public bool Autenticar(AutenticacaoModel autenticacaoModel, out string msgErro, string grupo="administrador")
        {
            msgErro = string.Empty;
            var usuario =
                _adimRepository.Admins.Where(u => u.SLogin.Equals(autenticacaoModel.Login)).FirstOrDefault();

            if ( usuario == null)
            {
                msgErro = "Usuário não cadastrado no Sistema";
                return false;
            }
            if (usuario.SSenha != autenticacaoModel.Senha)
            {
                msgErro = "Senha incorreta";
                return false;
            }

            HttpContext.Current.Session["autenticacao"] = new AutenticacaoModel()
            {
                Grupo = grupo,
                Senha = autenticacaoModel.Senha,
                Login = autenticacaoModel.Login,
                SNomeTratamento = usuario.SNomeTratamento
            };

            return true;
        }

       public void Desautentica()
        {
            HttpContext.Current.Session.Remove("autenticacao");
        }

        public bool Autenticado
        {
            get
            {
                return HttpContext.Current.Session["autenticacao"] != null &&
                       HttpContext.Current.Session["autenticacao"].GetType() == typeof(AutenticacaoModel);
            }
            
        }

        public AutenticacaoModel UsuarioAutenticado
        {
            get
            {
                if (Autenticado)
                    return (AutenticacaoModel) HttpContext.Current.Session["autenticacao"];
                return null;
            }
            
        }
    }
}