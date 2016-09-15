using System.Collections.Generic;
using SisVest.DomainModel.Abstract;
using System.Linq;

namespace SisVest.WebUI.Models
{
    public class CursoModel
    {
        private ICursoRepository _cursoRepository;

        public CursoModel(ICursoRepository repository)
        {
            _cursoRepository = repository;
        }

        public int ICursoId { get; set; }
        
        public string SDescricao { get; set; }
        
        public int IVagas { get; set; }

        public int iTotalCandidatos{ get; set; }

        public int iTotalCandidatosAprovados { get; set; }

        public IList<CursoModel> RetornaTodos()
        {
            var result = _cursoRepository.Cursos.ToList();
            IList<CursoModel> cursoModelList = new List<CursoModel>();

            foreach (var curso in result)
            {
                cursoModelList.Add(new CursoModel(_cursoRepository)
                {
                    ICursoId = curso.ICursoId,
                    SDescricao = curso.SDescricao,
                    IVagas = curso.IVagas,
                    iTotalCandidatos = curso.CandidatosList.Count,
                    iTotalCandidatosAprovados = _cursoRepository.CandidatosAprovadas(curso.ICursoId).Count()
                });
            }

            return cursoModelList;
        }


    }
}