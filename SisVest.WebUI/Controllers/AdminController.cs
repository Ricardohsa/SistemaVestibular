using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;
using SisVest.WebUI.Infraestrutura.Filter;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;

namespace SisVest.WebUI.Controllers
{
    [CustomAutenticacao("administrador")]
    public class AdminController : Controller
    {
        private IAdimRepository _adimRepository;
        private IAutenticacaoProvider _autenticacaoProvider;

        public AdminController(IAdimRepository adimRepository, IAutenticacaoProvider autenticacao)
        {
            _adimRepository = adimRepository;
            _autenticacaoProvider = autenticacao;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(_adimRepository.Admins.ToList());
        }


        public ActionResult Alterar(int id)
        {
            return View(_adimRepository.Retornar(id));
        }


        [HttpPost]
        public ActionResult Alterar(Admin admin)
        {
            if (ModelState.IsValid)
            {
                _adimRepository.Alterar(admin);
                TempData["Mensagem"] = "Administrador alterado com sucesso.";

                return RedirectToAction("Index");
            }

            return View(admin);
        }

        public ActionResult Excluir(int id)
        {
           
            return View(_adimRepository.Retornar(id));
        }

        //POST: 

        [HttpPost]
        public ActionResult Excluir(Admin admin)
        {
            try
            {
                if (_autenticacaoProvider.UsuarioAutenticado.Login == admin.SLogin)
                    TempData["Mensagem"] = "Você não pode excluir a si mesmo.";
                else
                {
                    _adimRepository.Excluir(admin.IAdminId);
                    TempData["Mensagem"] = "Administrador excluido com sucesso.";
                }
            }
            catch (Exception ex)
            {

                TempData["Mensagem"] = ex.Message;
            }

            return RedirectToAction("Index");
        }


        public ActionResult Inserir()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Inserir(Admin admin)
        {
            ModelState["IAdminId"].Errors.Clear();

            if (!ModelState.IsValid) return View(admin);

            try
            {
                _adimRepository.Inserir(admin);
                TempData["Mensagem"] = "Administrador cadastrado com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}