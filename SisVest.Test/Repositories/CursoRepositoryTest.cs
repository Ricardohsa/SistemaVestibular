using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Repositories
{
    [TestClass]
    public class CursoRepositoryTest
    {
        private ICursoRepository  _cursoRepository;
        private ICandidatoRepository _candidatoRepository;
        private VestContext _vestContext = new VestContext();
        private Curso _cursoInserir;
        private Candidato _candidatoInserir;
        private Candidato _candidatoInserir2;
        private Candidato _candidatoInserir3;
        private Vestibular _vestibularInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            //Cria Vestibular
            _vestibularInserir = (new Vestibular()
            {
                DtInicioInscricao = new DateTime(2017, 02, 01),
                DtFimInscricao = new DateTime(2017, 03, 31),
                DtProva = new DateTime(2017, 03, 31).AddDays(7),
                SDescricao = "Vestibular UNIB 2017"
            });

            _vestContext.Vestibulares.Add(_vestibularInserir);
            _vestContext.SaveChanges();

            
            // Cria Curso
            _cursoRepository = new EfCursoRepository(_vestContext);
            _cursoInserir = (new Curso()
            {
                SDescricao = "Analise de Sistemas",
                IVagas = 50
            });

            _vestContext.Cursos.Add(_cursoInserir);
            _vestContext.SaveChanges();

            //Cria Candidatos
            _candidatoInserir = new Candidato()
            {
                Curso = _cursoInserir,
                SNome = "Ricardo Sá",
                SEmail = "r.humberto.sa@gmail.com",
                SCpf = "29589563856",
                STelefone = "991186933",
                SSexo = "M",
                SSenha = "123456",
                DtNascimento = new DateTime(1982, 04, 11),
                Vestibular = _vestibularInserir
            };

            _candidatoInserir2 = new Candidato()
            {
                Curso = _cursoInserir,
                SNome = "Miguel Sá",
                SEmail = "miguelsa@gmail.com",
                SCpf = "295895638xy",
                STelefone = "991186933",
                SSexo = "M",
                SSenha = "123456",
                DtNascimento = new DateTime(1982, 07, 04),
                Vestibular = _vestibularInserir
            };

            //Cria 3º Candidato
            _candidatoInserir3 = new Candidato()
            {
                Curso = _cursoInserir,
                SNome = "Michelle Sá",
                SEmail = "michelle@gmail.com",
                SCpf = "29749642813",
                STelefone = "991186933",
                SSexo = "F",
                SSenha = "123456",
                DtNascimento = new DateTime(1982, 04, 28),
                Vestibular = _vestibularInserir
            };

            _candidatoRepository = new EfCandidatoRepository(_vestContext);

            _candidatoRepository.RealizarInscricao(_candidatoInserir);
            _candidatoRepository.RealizarInscricao(_candidatoInserir2);
            _candidatoRepository.RealizarInscricao(_candidatoInserir3);

            _candidatoRepository.Aprovar(_candidatoInserir.ICandidatoId);
            _candidatoRepository.Aprovar(_candidatoInserir2.ICandidatoId);
            _candidatoRepository.Aprovar(_candidatoInserir3.ICandidatoId);
        }

        [TestMethod]
        public void Pode_Consultar_Usando_LINQ_Repositorio_Test()
        {
           
           
            //Ação

            var cursos = _cursoRepository.Cursos;

            var retorno = (from a in cursos
                           where a.SDescricao.Equals(_cursoInserir.SDescricao)
                           select a).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(cursos, typeof(IQueryable<Curso>));
            Assert.AreEqual(retorno, _cursoInserir);

        }

        [TestMethod]
        public void Pode_Inserir_Test()
        {
            //Ambiente
            var cursoInserir2 = (new Curso()
            {
                SDescricao = "Letras 20211",
                IVagas = 45
            });

            //Ação

            _cursoRepository.Inserir(cursoInserir2);
            _vestContext.SaveChanges();

            var retorno = (from a in _cursoRepository.Cursos
                           where a.SDescricao.Equals(cursoInserir2.SDescricao)
                           select a).FirstOrDefault();

            //Assertivas            
            Assert.AreEqual(retorno, cursoInserir2);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Curso_Com_Mesma_Descricao_Test()
        {
            //Ambiente

            var cursoInserir2 = (new Curso()
            {
                SDescricao = _cursoInserir.SDescricao,
                IVagas = 45
            });

            _cursoRepository.Inserir(_cursoInserir);
            //Ação            
            _cursoRepository.Inserir(cursoInserir2);


            //Assertivas         

        }


        [TestMethod]
        public void Pode_Altera_Test()
        {
            //Ambiente    
            var curso_alterar = new Curso()
            {
                SDescricao = "BIOLOGIA",
                IVagas = 5
            };

            _cursoRepository.Inserir(curso_alterar);

            var descriaoEsperada = curso_alterar.SDescricao;

            var cursoAlterar = (from a in _cursoRepository.Cursos
                                where a.ICursoId == curso_alterar.ICursoId
                                select a).FirstOrDefault();

            cursoAlterar.SDescricao = "Ciencias da Computação";

            //Ação
            _cursoRepository.Alterar(cursoAlterar);

            var retorno = (from a in _cursoRepository.Cursos
                           where a.ICursoId.Equals(curso_alterar.ICursoId)
                           select a).FirstOrDefault();
            //Assertivas
            Assert.AreEqual(retorno.ICursoId, cursoAlterar.ICursoId);
            Assert.AreNotEqual(descriaoEsperada, cursoAlterar.SDescricao);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Curso_Com_Mesma_Descricao_Test()
        {
            //Ambiente
            _cursoRepository.Inserir(_cursoInserir);

            var cursoAlterar = (from a in _cursoRepository.Cursos
                                where a.ICursoId == _cursoInserir.ICursoId
                                select a).FirstOrDefault();

            cursoAlterar.SDescricao = "Ciencias da Computação";

            //Ação
            _cursoRepository.Alterar(cursoAlterar);

            //Assertivas      

        }



        [TestMethod]
        public void Pode_Excluir_Test()
        {
            //Ambiente
           
            //Ação
            _cursoRepository.Excluir(_cursoInserir.ICursoId);

            //Assertivas            
            var result = from c in _vestContext.Cursos
                         where c.ICursoId.Equals(_cursoInserir.ICursoId)
                         select c;

            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Dados_Que_Nao_Existe_Test()
        {
            //Ambiente


            //Ação
            _cursoRepository.Excluir(10050);

            //Assertivas            

        }



        [TestMethod]
        public void Retornar_Curso_Por_ID_Test()
        {
            //Ambiente
           

            //Ação

            var result = _cursoRepository.RetornarPorId(_cursoInserir.ICursoId);

            //Assertivas            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Curso));
            Assert.AreEqual(_cursoInserir, result);

        }

        [TestMethod]
        public void Pode_Retornar_Candidatos_Aprovados_Test()
        {
            //Ambiente


            //Ação
            var result = _cursoRepository.CandidatosAprovadas(_cursoInserir.ICursoId);

            //Assertivas            
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.ToList().Contains(_candidatoInserir));
            Assert.IsTrue(result.ToList().Contains(_candidatoInserir2));
            Assert.IsTrue(result.ToList().Contains(_candidatoInserir3));
        }

        [TestCleanup]
        public void LimparCenario()
        {
            //Remove Candidatos
            var candidatosParaRemover = from c in _vestContext.Candidatos select c;
            foreach (var candidatos in candidatosParaRemover)
            {
                _vestContext.Candidatos.Remove(candidatos);

            }
            _vestContext.SaveChanges();

            //Remove Cursos
            var cursosParaRemover = from c in _vestContext.Cursos select c;

            foreach (var cursos in cursosParaRemover)
            {
                _vestContext.Cursos.Remove(cursos);

            }
            _vestContext.SaveChanges();

            //Remove Vestibulares
            var vestibularesParaRemover = from v in _vestContext.Vestibulares select v;

            foreach (var vestibulares in vestibularesParaRemover)
            {
                _vestContext.Vestibulares.Remove(vestibulares);

            }
            _vestContext.SaveChanges();
        }
    }
}
