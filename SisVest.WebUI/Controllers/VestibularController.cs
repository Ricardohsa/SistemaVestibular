using System;
using System.Linq;
using System.Web.Mvc;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;


namespace SisVest.WebUI.Controllers
{
    public class VestibularController : Controller
    {
        private IVestibularRepository _repository;
        private IAutenticacaoProvider _autenticacaoProvider;

        
        public VestibularController(IVestibularRepository vestibularRepository, IAutenticacaoProvider autenticacaoProvider)
        {
            _repository = vestibularRepository;
            _autenticacaoProvider = autenticacaoProvider;
        }

        // GET: Curso
        
        public ActionResult Index()
        {
            if (!(_autenticacaoProvider.Autenticado && _autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            return View(_repository.Vestibulares.ToList());
        }

        public ActionResult Alterar(int id)
        {
            if (!(_autenticacaoProvider.Autenticado && _autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            return View(_repository.Vestibulares.Where(v => v.IVestibularId.Equals(id)).FirstOrDefault());
        }

        //POST: Alterar
        [HttpPost]
        public ActionResult Alterar(Vestibular vestibular)
        {
            if (!ModelState.IsValid)
                return View(vestibular);

            _repository.Alterar(vestibular);

            TempData["Mensagem"] = "Vesrtibular alterado com sucesso.";

            return RedirectToAction("Index");
        }

        public ActionResult Excluir(int id)
        {
            if (!(_autenticacaoProvider.Autenticado && _autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            return View(_repository.Vestibulares.Where(v => v.IVestibularId.Equals(id)) .FirstOrDefault());
        }

        //POST: 

        [HttpPost]
        public ActionResult Excluir(Vestibular vestibular)
        {
            try
            {
                _repository.Excluir(vestibular.IVestibularId);
                TempData["Mensagem"] = "O Vestibular " + vestibular.SDescricao + "excluido com sucesso."; 
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = ex.Message;
            }

            return RedirectToAction("Index");
        }


        public ActionResult Inserir()
        {
            if (!(_autenticacaoProvider.Autenticado && _autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))
                HttpContext.Response.StatusCode = 401;
            return View();
        }

        [HttpPost]
        public ActionResult Inserir(Vestibular vestibular)
        {
            ModelState["IVestibularId"].Errors.Clear();

            if (!ModelState.IsValid) return View(vestibular);

            try
            {
                _repository.Inserir(vestibular);
                TempData["Mensagem"] = "Vestibular cadastrado com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}