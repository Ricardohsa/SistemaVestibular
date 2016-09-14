using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Repositories
{
    [TestClass]
    public class CandidatoRepositoriesTest
    {
        private ICandidatoRepository _candidatoRepository;
        private VestContext _vestContext = new VestContext();
        private Candidato _candidatoInserir;
        private Curso _cursoInserir;
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

            //Cria Curso
            _cursoInserir = (new Curso()
            {
                SDescricao = "Analise de Sistemas",
                IVagas = 50
            });

            _vestContext.Cursos.Add(_cursoInserir);
            _vestContext.SaveChanges();


            //Cria Candidato

            _candidatoRepository = new EfCandidatoRepository(_vestContext);
            
            _candidatoInserir = new Candidato()
            {
                Curso = _cursoInserir,
                SNome = "Ricardo Sá",
                SEmail = "r.humberto.sa@gmail.com",
                SCpf  = "29589563856",
                STelefone = "991186933",
                SSexo = "M",
                SSenha = "123456",
                DtNascimento = new DateTime(1982,04,11),
                Vestibular = _vestibularInserir
            };

        }

        [TestMethod]
        public void Pode_Consultar_LINQ_Usando_Repositorio_Test()
        {
            //Ambiente    
            _vestContext.Candidatos.Add(_candidatoInserir);
            _vestContext.SaveChanges();

            //Ação
            var candidatos = _candidatoRepository.Candidatos;

            var retorno = (from c in candidatos
                           where c.ICandidatoId.Equals(_candidatoInserir.ICandidatoId) 
                           select c).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(candidatos, typeof(IQueryable<Candidato>));
            Assert.AreEqual(retorno, _candidatoInserir);

        }

        [TestMethod]
        public void Pode_Realizar_Inscricao_Candidato_Test()
        {
           
            //Ação
            _candidatoRepository.RealizarInscricao(_candidatoInserir);
            

            var result = (from c in _candidatoRepository.Candidatos
                           where c.ICandidatoId.Equals(_candidatoInserir.ICandidatoId)
                           select c).FirstOrDefault();

            //Assertivas            
            Assert.AreEqual(result, _candidatoInserir);
            Assert.AreEqual(_vestibularInserir, result.Vestibular);
            Assert.AreEqual(_cursoInserir, result.Curso);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Sem_Email()
        {
            //Ambiente
            _candidatoInserir.SEmail = null;
            
            //Ação            
            _candidatoRepository.RealizarInscricao(_candidatoInserir);

            //Assertivas         

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Com_CPF_Jah_Existente_Test()
        {
            //Ambiente

            _candidatoRepository.RealizarInscricao(_candidatoInserir);

            //Inserir 2º Candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = _cursoInserir,
                SNome = "Michelle",
                SEmail = "michelle.brg@hotmail.com",
                SCpf = _candidatoInserir.SCpf,
                STelefone = "991186933",
                SSexo = "F",
                SSenha = "123456",
                DtNascimento = new DateTime(1982, 04, 28),
                Vestibular = _vestibularInserir
            };
            //Ação            
            _candidatoRepository.RealizarInscricao(candidatoInserir2);
            //Assertivas 
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Com_Email_Jah_Existente_Test()
        {
            //Ambiente

            _candidatoRepository.RealizarInscricao(_candidatoInserir);

            //Inserir 2º Candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = _cursoInserir,
                SNome = "Michelle",
                SEmail = _candidatoInserir.SEmail,
                SCpf = "29749642813",
                STelefone = "991186933",
                SSexo = "F",
                SSenha = "123456",
                DtNascimento = new DateTime(1982, 04, 28),
                Vestibular = _vestibularInserir
            };

            //Ação            
            _candidatoRepository.RealizarInscricao(candidatoInserir2);
            
            //Assertivas 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Sem_Curso_Test()
        {
            //Ambiente
            _candidatoInserir.Curso = null;

            //Ação            
            _candidatoRepository.RealizarInscricao(_candidatoInserir);
            
            //Assertivas 
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Sem_Vestibular_Test()
        {
            //Ambiente
            _candidatoInserir.Vestibular = null;

            //Ação            
            _candidatoRepository.RealizarInscricao(_candidatoInserir);

            //Assertivas 
        }

        [TestMethod]
        public void Pode_Atualizar_Cadastro_Candidato_Test()
        {
            //Ambiente
            var emailEsperadado = _candidatoInserir.SEmail;

            _candidatoRepository.RealizarInscricao(_candidatoInserir);

            var candidatoAtualizarCadastro = (from c in _candidatoRepository.Candidatos
                where c.ICandidatoId == _candidatoInserir.ICandidatoId
                select c).FirstOrDefault();

            candidatoAtualizarCadastro.SEmail = "teste@teset.com.br";

            //Ação            

            _candidatoRepository.AtualizarCadasto(candidatoAtualizarCadastro);

            var retorno = (from c in _candidatoRepository.Candidatos
                where c.ICandidatoId.Equals(_candidatoInserir.ICandidatoId)
                select c).FirstOrDefault();

            //Assertivas 

            Assert.AreEqual(_candidatoInserir.ICandidatoId, retorno.ICandidatoId);
            Assert.AreNotEqual(emailEsperadado, retorno.SEmail);

        }

        [TestMethod]
        public void Pode_Excluir_Candidato_Com_Sucesso_Test()
        {
            //Ambiente
            _candidatoRepository.RealizarInscricao(_candidatoInserir);

            
            //Ação            
            _candidatoRepository.ExcluirCadastro(_candidatoInserir.ICandidatoId);
            
            //Assertivas 
            var result = from c in _candidatoRepository.Candidatos
                where c.ICandidatoId.Equals(_candidatoInserir.ICandidatoId)
                select c;

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Pode_Aprovar_Candidato_Test()
        {
            //Ambiente
            
                //Modificando curso para que ele tenha 3 vagas
            _cursoInserir.IVagas = 3;

            _vestContext.SaveChanges();

            //Cria segundo Candidato
            var candidatoInserir2 = new Candidato()
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
            var candidatoInserir3 = new Candidato()
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

            //Realizar Inscrição dos Candidatos
            _candidatoRepository.RealizarInscricao(_candidatoInserir);
            _candidatoRepository.RealizarInscricao(candidatoInserir2);
            _candidatoRepository.RealizarInscricao(candidatoInserir3);

            //Ação            
              //Aprovar Candidatos
            _candidatoRepository.Aprovar(_candidatoInserir.ICandidatoId);
            _candidatoRepository.Aprovar(candidatoInserir2.ICandidatoId);
            _candidatoRepository.Aprovar(candidatoInserir3.ICandidatoId);

            //Assertivas 
            var result = (from cur in _vestContext.Cursos
                          from can in cur.CandidatosList
                         where cur.ICursoId.Equals(_cursoInserir.ICursoId) && can.BAprovado
                         select can);

            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.ToList().Contains(_candidatoInserir));
            Assert.IsTrue(result.ToList().Contains(candidatoInserir2));
            Assert.IsTrue(result.ToList().Contains(candidatoInserir3));
        }



        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Aprovar_Candidato_Em_Curso_Sem_Vaga_Test()
        {
            //Ambiente

            //Modificando curso para que ele tenha 3 vagas
            _cursoInserir.IVagas = 2;

            _vestContext.SaveChanges();

            //Cria segundo Candidato
            var candidatoInserir2 = new Candidato()
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
            var candidatoInserir3 = new Candidato()
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

            //Realizar Inscrição dos Candidatos
            _candidatoRepository.RealizarInscricao(_candidatoInserir);
            _candidatoRepository.RealizarInscricao(candidatoInserir2);
            _candidatoRepository.RealizarInscricao(candidatoInserir3);

            //Aprovar Candidatos
            _candidatoRepository.Aprovar(_candidatoInserir.ICandidatoId);
            _candidatoRepository.Aprovar(candidatoInserir2.ICandidatoId);

            //Ação           
            //Ao realizar aprovação do 3º candidato deve levantar exception
            _candidatoRepository.Aprovar(candidatoInserir3.ICandidatoId);

            //Assertivas 
        }

        [TestMethod]
        public void Pode_Retornar_Candidato_Por_Id_Test()
        {
            //Ambiente
            _candidatoRepository.RealizarInscricao(_candidatoInserir);
            
            //Ação           
            var result = _candidatoRepository.Retornar(_candidatoInserir.ICandidatoId);

            //Assertivas 
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(Candidato));
            Assert.AreEqual(_candidatoInserir, result);
        }


        [TestMethod]
        public void Pode_Retornar_Todos_Os_Candidatos_Test()
        {
            //Ambiente

            
            //Cria segundo Candidato
            var candidatoInserir2 = new Candidato()
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
            var candidatoInserir3 = new Candidato()
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

            //Realizar Inscrição dos Candidatos
            _candidatoRepository.RealizarInscricao(_candidatoInserir);
            _candidatoRepository.RealizarInscricao(candidatoInserir2);
            _candidatoRepository.RealizarInscricao(candidatoInserir3);
            

            //Ação           
            var candidatos = _candidatoRepository.RetornarTodos();

            //Assertivas 
            Assert.AreEqual(3, candidatos.Count);
            Assert.IsTrue(candidatos.Contains(_candidatoInserir));
            Assert.IsTrue(candidatos.Contains(candidatoInserir2));
            Assert.IsTrue(candidatos.Contains(candidatoInserir3));
        }

        [TestMethod]
        public void Pode_Retornar_Candidato_Por_Vestibular_Por_Curso()
        {
            //Ambiente


            //Cria segundo Candidato
            var candidatoInserir2 = new Candidato()
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
            var candidatoInserir3 = new Candidato()
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

            //Realizar Inscrição dos Candidatos
            _candidatoRepository.RealizarInscricao(_candidatoInserir);
            _candidatoRepository.RealizarInscricao(candidatoInserir2);
            _candidatoRepository.RealizarInscricao(candidatoInserir3);


            //Ação           
            var candidatos =
                _candidatoRepository.RetornarCandidatossPorVestibularPorCurso(_vestibularInserir.IVestibularId,
                    _cursoInserir.ICursoId);

            //Assertivas 
            Assert.AreEqual(3, candidatos.Count);
            Assert.IsTrue(candidatos.Contains(_candidatoInserir));
            Assert.IsTrue(candidatos.Contains(candidatoInserir2));
            Assert.IsTrue(candidatos.Contains(candidatoInserir3));
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
