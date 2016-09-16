using System.Dynamic;
using SisVest.WebUI.Models;

namespace SisVest.WebUI.Infraestrutura.Provider.Abstract
{
    public interface IAutenticacaoProvider
    {
        bool Autenticar(AutenticacaoModel autenticacaoModel, out string msgErro, string grupo = "Administrador");

        void Desautentica();

        bool Autenticado { get; }

        AutenticacaoModel UsuarioAutenticado { get; }
    }
}