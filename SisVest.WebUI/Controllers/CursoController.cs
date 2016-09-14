using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;
using SisVest.WebUI.Models;


namespace SisVest.WebUI.Controllers
{
    public class CursoController : Controller
    {
        private ICursoRepository _repository;

        public CursoController(ICursoRepository cursoRepository)
        {
            _repository = cursoRepository;
        }

        // GET: Curso
        public ActionResult Index()
        {
            var result = _repository.Cursos.ToList();
            IList<CursoModel> cursoModelList = new List<CursoModel>();

            foreach (var curso in result)
            {
                cursoModelList.Add( new CursoModel()
                {
                    ICursoId = curso.ICursoId,
                    SDescricao = curso.SDescricao,
                    IVagas = curso.IVagas,
                    iTotalCandidatos = curso.CandidatosList.Count,
                    iTotalCandidatosAprovados = _repository.CandidatosAprovadas(curso.ICursoId).Count()
                });
            }

            return View(cursoModelList.ToList());
        }

        public ActionResult Alterar(int id)
        {
            return View(_repository.RetornarPorId(id));
        }

        //POST: Alterar
        [HttpPost]
        public ActionResult Alterar(Curso curso)
        {
            _repository.Alterar(curso);

            TempData["Mensagem"] = "Curso alterado com sucesso.";

            return View("Index", _repository.Cursos.ToList());
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
            
            return View("Index", _repository.Cursos.ToList());
        }
    }
}