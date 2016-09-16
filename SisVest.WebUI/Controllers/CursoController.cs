using System;
using System.Linq;
using System.Web.Mvc;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;
using SisVest.WebUI.Infraestrutura.Filter;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using SisVest.WebUI.Models;


namespace SisVest.WebUI.Controllers
{
    [CustomAutenticacao("aministrador")]
    public class CursoController : Controller
    {
        private ICursoRepository _repository;
        private CursoModel _cursoModel;
        private IAutenticacaoProvider _autenticacaoProvider;
        
        public CursoController(ICursoRepository cursoRepository, CursoModel cursoModel, IAutenticacaoProvider autenticacaoProvider)
        {
            _repository = cursoRepository;
            _cursoModel = cursoModel;
            _autenticacaoProvider = autenticacaoProvider;
        }

        // GET: Curso
        public ActionResult Index()
        {
            return View(_cursoModel.RetornaTodos().ToList());
        }
        
        public ActionResult Alterar(int id)
        {
            return View(_repository.RetornarPorId(id));
        }

        //POST: Alterar
        [HttpPost]
        public ActionResult Alterar(Curso curso)
        {
            if (ModelState.IsValid)
            {
                _repository.Alterar(curso);
                TempData["Mensagem"] = "Curso alterado com sucesso.";

                return RedirectToAction("Index");
            }

            return View(curso);
        }

        public ActionResult Excluir(int id)
        {
            return View(_repository.RetornarPorId(id));
        }

        //POST: 

        [HttpPost]
        public ActionResult Excluir(Curso curso)
        {
            try
            {
                _repository.Excluir(curso.ICursoId);
                TempData["Mensagem"] = "Curso excluido com sucesso.";
            }
            catch (Exception ex)
            {

                TempData["Mensagem"] = ex.Message;
            }

            return RedirectToAction("Index");

        }

        public ActionResult Inserir()
        {
            HtmlHelper.ClientValidationEnabled = false;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = false;

            if (!(_autenticacaoProvider.Autenticado && _autenticacaoProvider.UsuarioAutenticado.Grupo == "administrador"))

                HttpContext.Response.StatusCode = 401;

            return View();
        }

        [HttpPost]
        public ActionResult Inserir(Curso curso)
        {

            HtmlHelper.ClientValidationEnabled = false;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = false;

            ModelState["ICursoId"].Errors.Clear();

            if (!ModelState.IsValid) return View(curso);
            try
            {
                _repository.Inserir(curso);
                TempData["Mensagem"] = "Curso cadastrado com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}