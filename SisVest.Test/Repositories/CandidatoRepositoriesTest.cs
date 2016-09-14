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
        private ICandidatoRepository candidatoRepository;
        private VestContext vestContext = new VestContext();
        private Candidato candidatoInserir;
        private Curso cursoInserir;
        private Vestibular vestibularInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            //Cria Vestibular
            vestibularInserir = (new Vestibular()
            {
                dtInicioInscricao = new DateTime(2017, 02, 01),
                dtFimInscricao = new DateTime(2017, 03, 31),
                dtProva = new DateTime(2017, 03, 31).AddDays(7),
                sDescricao = "Vestibular UNIB 2017"
            });

            vestContext.Vestibulares.Add(vestibularInserir);
            vestContext.SaveChanges();

            //Cria Curso
            cursoInserir = (new Curso()
            {
                sDescricao = "Analise de Sistemas",
                iVagas = 50
            });

            vestContext.Cursos.Add(cursoInserir);
            vestContext.SaveChanges();


            //Cria Candidato

            candidatoRepository = new EFCandidatoRepository(vestContext);
            
            candidatoInserir = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Ricardo Sá",
                sEmail = "r.humberto.sa@gmail.com",
                sCpf  = "29589563856",
                sTelefone = "991186933",
                sSexo = "M",
                sSenha = "123456",
                dtNascimento = new DateTime(1982,04,11),
                Vestibular = vestibularInserir
            };

        }

        [TestMethod]
        public void Pode_Consultar_LINQ_Usando_Repositorio_Test()
        {
            //Ambiente    
            vestContext.Candidatos.Add(candidatoInserir);
            vestContext.SaveChanges();

            //Ação
            var candidatos = candidatoRepository.candidatos;

            var retorno = (from c in candidatos
                           where c.iCandidatoId.Equals(candidatoInserir.iCandidatoId) 
                           select c).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(candidatos, typeof(IQueryable<Candidato>));
            Assert.AreEqual(retorno, candidatoInserir);

        }

        [TestMethod]
        public void Pode_Realizar_Inscricao_Candidato_Test()
        {
           
            //Ação
            candidatoRepository.RealizarInscricao(candidatoInserir);
            

            var result = (from c in candidatoRepository.candidatos
                           where c.iCandidatoId.Equals(candidatoInserir.iCandidatoId)
                           select c).FirstOrDefault();

            //Assertivas            
            Assert.AreEqual(result, candidatoInserir);
            Assert.AreEqual(vestibularInserir, result.Vestibular);
            Assert.AreEqual(cursoInserir, result.Curso);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Sem_Email()
        {
            //Ambiente
            candidatoInserir.sEmail = null;
            
            //Ação            
            candidatoRepository.RealizarInscricao(candidatoInserir);

            //Assertivas         

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Com_CPF_Jah_Existente_Test()
        {
            //Ambiente

            candidatoRepository.RealizarInscricao(candidatoInserir);

            //Inserir 2º Candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Michelle",
                sEmail = "michelle.brg@hotmail.com",
                sCpf = candidatoInserir.sCpf,
                sTelefone = "991186933",
                sSexo = "F",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 04, 28),
                Vestibular = vestibularInserir
            };
            //Ação            
            candidatoRepository.RealizarInscricao(candidatoInserir2);
            //Assertivas 
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Com_Email_Jah_Existente_Test()
        {
            //Ambiente

            candidatoRepository.RealizarInscricao(candidatoInserir);

            //Inserir 2º Candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Michelle",
                sEmail = candidatoInserir.sEmail,
                sCpf = "29749642813",
                sTelefone = "991186933",
                sSexo = "F",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 04, 28),
                Vestibular = vestibularInserir
            };

            //Ação            
            candidatoRepository.RealizarInscricao(candidatoInserir2);
            
            //Assertivas 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Sem_Curso_Test()
        {
            //Ambiente
            candidatoInserir.Curso = null;

            //Ação            
            candidatoRepository.RealizarInscricao(candidatoInserir);
            
            //Assertivas 
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Realizar_Inscricao_Candidato_Sem_Vestibular_Test()
        {
            //Ambiente
            candidatoInserir.Vestibular = null;

            //Ação            
            candidatoRepository.RealizarInscricao(candidatoInserir);

            //Assertivas 
        }

        [TestMethod]
        public void Pode_Atualizar_Cadastro_Candidato_Test()
        {
            //Ambiente
            var emailEsperadado = candidatoInserir.sEmail;

            candidatoRepository.RealizarInscricao(candidatoInserir);

            var candidatoAtualizarCadastro = (from c in candidatoRepository.candidatos
                where c.iCandidatoId == candidatoInserir.iCandidatoId
                select c).FirstOrDefault();

            candidatoAtualizarCadastro.sEmail = "teste@teset.com.br";

            //Ação            

            candidatoRepository.AtualizarCadasto(candidatoAtualizarCadastro);

            var retorno = (from c in candidatoRepository.candidatos
                where c.iCandidatoId.Equals(candidatoInserir.iCandidatoId)
                select c).FirstOrDefault();

            //Assertivas 

            Assert.AreEqual(candidatoInserir.iCandidatoId, retorno.iCandidatoId);
            Assert.AreNotEqual(emailEsperadado, retorno.sEmail);

        }

        [TestMethod]
        public void Pode_Excluir_Candidato_Com_Sucesso_Test()
        {
            //Ambiente
            candidatoRepository.RealizarInscricao(candidatoInserir);

            
            //Ação            
            candidatoRepository.ExcluirCadastro(candidatoInserir.iCandidatoId);
            
            //Assertivas 
            var result = from c in candidatoRepository.candidatos
                where c.iCandidatoId.Equals(candidatoInserir.iCandidatoId)
                select c;

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Pode_Aprovar_Candidato_Test()
        {
            //Ambiente
            
                //Modificando curso para que ele tenha 3 vagas
            cursoInserir.iVagas = 3;

            vestContext.SaveChanges();

            //Cria segundo Candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Miguel Sá",
                sEmail = "miguelsa@gmail.com",
                sCpf = "295895638xy",
                sTelefone = "991186933",
                sSexo = "M",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 07, 04),
                Vestibular = vestibularInserir
            };
            
            //Cria 3º Candidato
            var candidatoInserir3 = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Michelle Sá",
                sEmail = "michelle@gmail.com",
                sCpf = "29749642813",
                sTelefone = "991186933",
                sSexo = "F",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 04, 28),
                Vestibular = vestibularInserir
            };

            //Realizar Inscrição dos Candidatos
            candidatoRepository.RealizarInscricao(candidatoInserir);
            candidatoRepository.RealizarInscricao(candidatoInserir2);
            candidatoRepository.RealizarInscricao(candidatoInserir3);

            //Ação            
              //Aprovar Candidatos
            candidatoRepository.Aprovar(candidatoInserir.iCandidatoId);
            candidatoRepository.Aprovar(candidatoInserir2.iCandidatoId);
            candidatoRepository.Aprovar(candidatoInserir3.iCandidatoId);

            //Assertivas 
            var result = (from cur in vestContext.Cursos
                          from can in cur.CandidatosList
                         where cur.iCursoId.Equals(cursoInserir.iCursoId) && can.bAprovado
                         select can);

            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.ToList().Contains(candidatoInserir));
            Assert.IsTrue(result.ToList().Contains(candidatoInserir2));
            Assert.IsTrue(result.ToList().Contains(candidatoInserir3));
        }



        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Aprovar_Candidato_Em_Curso_Sem_Vaga_Test()
        {
            //Ambiente

            //Modificando curso para que ele tenha 3 vagas
            cursoInserir.iVagas = 2;

            vestContext.SaveChanges();

            //Cria segundo Candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Miguel Sá",
                sEmail = "miguelsa@gmail.com",
                sCpf = "295895638xy",
                sTelefone = "991186933",
                sSexo = "M",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 07, 04),
                Vestibular = vestibularInserir
            };

            //Cria 3º Candidato
            var candidatoInserir3 = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Michelle Sá",
                sEmail = "michelle@gmail.com",
                sCpf = "29749642813",
                sTelefone = "991186933",
                sSexo = "F",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 04, 28),
                Vestibular = vestibularInserir
            };

            //Realizar Inscrição dos Candidatos
            candidatoRepository.RealizarInscricao(candidatoInserir);
            candidatoRepository.RealizarInscricao(candidatoInserir2);
            candidatoRepository.RealizarInscricao(candidatoInserir3);

            //Aprovar Candidatos
            candidatoRepository.Aprovar(candidatoInserir.iCandidatoId);
            candidatoRepository.Aprovar(candidatoInserir2.iCandidatoId);

            //Ação           
            //Ao realizar aprovação do 3º candidato deve levantar exception
            candidatoRepository.Aprovar(candidatoInserir3.iCandidatoId);

            //Assertivas 
        }

        [TestMethod]
        public void Pode_Retornar_Candidato_Por_Id_Test()
        {
            //Ambiente
            candidatoRepository.RealizarInscricao(candidatoInserir);
            
            //Ação           
            var result = candidatoRepository.Retornar(candidatoInserir.iCandidatoId);

            //Assertivas 
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(Candidato));
            Assert.AreEqual(candidatoInserir, result);
        }


        [TestMethod]
        public void Pode_Retornar_Todos_Os_Candidatos_Test()
        {
            //Ambiente

            
            //Cria segundo Candidato
            var candidatoInserir2 = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Miguel Sá",
                sEmail = "miguelsa@gmail.com",
                sCpf = "295895638xy",
                sTelefone = "991186933",
                sSexo = "M",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 07, 04),
                Vestibular = vestibularInserir
            };

            //Cria 3º Candidato
            var candidatoInserir3 = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Michelle Sá",
                sEmail = "michelle@gmail.com",
                sCpf = "29749642813",
                sTelefone = "991186933",
                sSexo = "F",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 04, 28),
                Vestibular = vestibularInserir
            };

            //Realizar Inscrição dos Candidatos
            candidatoRepository.RealizarInscricao(candidatoInserir);
            candidatoRepository.RealizarInscricao(candidatoInserir2);
            candidatoRepository.RealizarInscricao(candidatoInserir3);
            

            //Ação           
            var candidatos = candidatoRepository.RetornarTodos();

            //Assertivas 
            Assert.AreEqual(3, candidatos.Count);
            Assert.IsTrue(candidatos.Contains(candidatoInserir));
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
                Curso = cursoInserir,
                sNome = "Miguel Sá",
                sEmail = "miguelsa@gmail.com",
                sCpf = "295895638xy",
                sTelefone = "991186933",
                sSexo = "M",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 07, 04),
                Vestibular = vestibularInserir
            };

            //Cria 3º Candidato
            var candidatoInserir3 = new Candidato()
            {
                Curso = cursoInserir,
                sNome = "Michelle Sá",
                sEmail = "michelle@gmail.com",
                sCpf = "29749642813",
                sTelefone = "991186933",
                sSexo = "F",
                sSenha = "123456",
                dtNascimento = new DateTime(1982, 04, 28),
                Vestibular = vestibularInserir
            };

            //Realizar Inscrição dos Candidatos
            candidatoRepository.RealizarInscricao(candidatoInserir);
            candidatoRepository.RealizarInscricao(candidatoInserir2);
            candidatoRepository.RealizarInscricao(candidatoInserir3);


            //Ação           
            var candidatos =
                candidatoRepository.RetornarCandidatossPorVestibularPorCurso(vestibularInserir.iVestibularId,
                    cursoInserir.iCursoId);

            //Assertivas 
            Assert.AreEqual(3, candidatos.Count);
            Assert.IsTrue(candidatos.Contains(candidatoInserir));
            Assert.IsTrue(candidatos.Contains(candidatoInserir2));
            Assert.IsTrue(candidatos.Contains(candidatoInserir3));
        }

        [TestCleanup]
        public void LimparCenario()
        {
            //Remove Candidatos
            var candidatosParaRemover = from c in vestContext.Candidatos select c;
            foreach (var candidatos in candidatosParaRemover)
            {
                vestContext.Candidatos.Remove(candidatos);

            }
            vestContext.SaveChanges();

            //Remove Cursos
            var cursosParaRemover = from c in vestContext.Cursos select c;

            foreach (var cursos in cursosParaRemover)
            {
                vestContext.Cursos.Remove(cursos);

            }
            vestContext.SaveChanges();

            //Remove Vestibulares
            var vestibularesParaRemover = from v in vestContext.Vestibulares select v;

            foreach (var vestibulares in vestibularesParaRemover)
            {
                vestContext.Vestibulares.Remove(vestibulares);

            }
            vestContext.SaveChanges();
            
        }
    }
}
