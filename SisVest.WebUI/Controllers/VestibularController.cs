using System;
using System.Linq;
using System.Web.Mvc;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;


namespace SisVest.WebUI.Controllers
{
    [Authorize]
    public class VestibularController : Controller
    {
        private IVestibularRepository _repository;

        
        public VestibularController(IVestibularRepository vestibularRepository)
        {
            _repository = vestibularRepository;
        }

        // GET: Curso
        
        public ActionResult Index()
        {
            return View(_repository.Vestibulares.ToList());
        }

        public ActionResult Alterar(int id)
        {
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