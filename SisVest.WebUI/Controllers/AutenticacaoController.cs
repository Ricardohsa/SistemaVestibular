using System;
using System.Collections.Generic;   
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using SisVest.WebUI.Models;

namespace SisVest.WebUI.Controllers
{
    public class AutenticacaoController : Controller
    {
        private IAutenticacaoProvider _autenticacaoProvider;

        public AutenticacaoController(IAutenticacaoProvider autenticacaoParam)
        {
            _autenticacaoProvider = autenticacaoParam;
        }

        // GET: Autenticacao
        public ActionResult Entrar()
        {
            ViewBag.Autenticado = _autenticacaoProvider.Autenticado;

            return View(_autenticacaoProvider.UsuarioAutenticado);
        }

        [HttpPost]
        public ActionResult Entrar(AutenticacaoModel autenticacaoModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string msgErro;

                if (_autenticacaoProvider.Autenticar(autenticacaoModel, out msgErro, "administrador"))
                {
                    return Redirect(returnUrl ?? Url.Action("Entrar","Autenticacao"));
                }
                ModelState.AddModelError("", msgErro);
            }
            ViewBag.Autenticado = _autenticacaoProvider.Autenticado;
            return View();
        }

        public ActionResult Sair()
        {
            _autenticacaoProvider.Desautentica();

            return RedirectToAction("Entrar");
        }
    }
}