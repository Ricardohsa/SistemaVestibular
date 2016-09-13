using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Repositories
{
    [TestClass]
    public class VestibularRepositories
    {
        private IVestibularRepository vestibularRepository;
        private VestContext vestContext = new VestContext();
        private Vestibular vestibularInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            vestibularRepository = new EFVestibularRepository(vestContext);

            vestibularInserir = (new Vestibular()
            {

                dtInicioInscricao = new DateTime(2016, 3, 25),
                dtFimInscricao = new DateTime(2016, 3, 25).AddDays(5),
                dtProva = new DateTime(2016, 3, 25).AddDays(7),
                sDescricao = "Vestibular 2017"

            });
        }

        [TestMethod]
        public void Pode_Consultar_Usando_LINQ_Repositorio_Test()
        {
            //Ambiente    
            vestContext.Vestibulares.Add(vestibularInserir);
            vestContext.SaveChanges();
            //Ação

            var vestibulares = vestibularRepository.vestibulares;

            var retorno = (from a in vestibulares
                           where a.sDescricao.Equals(vestibularInserir.sDescricao)
                           select a).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(vestibulares, typeof(IQueryable<Vestibular>));
            Assert.AreEqual(retorno, vestibularInserir);

        }

        [TestMethod]
        public void Pode_Inserir_Vestibular_Test()
        {
            //Ambiente


            //Ação

            vestibularRepository.Inserir(vestibularInserir);
            vestContext.SaveChanges();

            var retorno = (from a in vestibularRepository.vestibulares
                           where a.sDescricao.Equals(vestibularInserir.sDescricao)
                           select a).FirstOrDefault();

            //Assertivas            
            Assert.AreEqual(retorno, vestibularInserir);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Vestibular_Com_Mesma_Descricao_Test()
        {
            //Ambiente

            var vestibularInserir2 = (new Vestibular()
            {
                dtInicioInscricao = new DateTime(2016, 3, 25),
                dtFimInscricao = new DateTime(2016, 3, 25).AddDays(5),
                dtProva = new DateTime(2016, 3, 25).AddDays(7),
                sDescricao = vestibularInserir.sDescricao

            });

            vestibularRepository.Inserir(vestibularInserir);
            //Ação            
            vestibularRepository.Inserir(vestibularInserir2);


            //Assertivas         

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Vestibular_Sem_Informar_Datas_Descricao_Test()
        {
            //Ambiente

            var vestibularInserir2 = (new Vestibular()
            {
                sDescricao = "20122"
            });

            vestibularRepository.Inserir(vestibularInserir2);
            //Ação            


            //Assertivas         

        }


        [TestMethod]
        public void Pode_Altera_Test()
        {
            //Ambiente    
            var descriaoEsperada = vestibularInserir.sDescricao;

            vestibularRepository.Inserir(vestibularInserir);


            var vestibularAlterar = (from a in vestibularRepository.vestibulares
                                     where a.iVestibularId == vestibularInserir.iVestibularId
                                     select a).FirstOrDefault();

            vestibularAlterar.sDescricao = "Vestibular 2012";

            //Ação
            vestibularRepository.Alterar(vestibularAlterar);

            var retorno = (from a in vestibularRepository.vestibulares
                           where a.iVestibularId.Equals(vestibularInserir.iVestibularId)
                           select a).FirstOrDefault();
            //Assertivas
            Assert.AreEqual(retorno.iVestibularId, vestibularAlterar.iVestibularId);
            Assert.AreNotEqual(descriaoEsperada, vestibularAlterar.sDescricao);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Vestibular_Com_Mesma_Descricao_Test()
        {
            //Ambiente
            vestibularRepository.Inserir(vestibularInserir);

            var vestibularAlterar = (from a in vestibularRepository.vestibulares
                                     where a.iVestibularId == vestibularInserir.iVestibularId
                                     select a).FirstOrDefault();

            vestibularAlterar.sDescricao = "Vestibular 2017";

            //Ação
            vestibularRepository.Alterar(vestibularAlterar);

            //Assertivas      

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Curso_Com_Mesma_Descricao_Ja_Persistida_Test()
        {
            //Ambiente
            vestibularRepository.Inserir(vestibularInserir);

            var vestibularInserir2 = new Vestibular()
            {
                sDescricao = "2019"
            };

            vestibularRepository.Inserir(vestibularInserir2);

            var cursoAlterar = (from c in vestibularRepository.vestibulares
                                where c.iVestibularId.Equals(vestibularInserir.iVestibularId)
                                select c).FirstOrDefault();

            cursoAlterar.sDescricao = vestibularInserir2.sDescricao;

            //Ação

            vestibularRepository.Alterar(cursoAlterar);
            //Assertivas      

        }


        [TestMethod]
        public void Pode_Excluir_Test()
        {
            //Ambiente
            vestibularRepository.Inserir(vestibularInserir);


            //Ação
            vestibularRepository.Excluir(vestibularInserir.iVestibularId);

            //Assertivas            
            var result = from c in vestContext.Vestibulares
                         where c.iVestibularId.Equals(vestibularInserir.iVestibularId)
                         select c;

            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Dados_Que_Nao_Existe_Test()
        {
            //Ambiente


            //Ação
            vestibularRepository.Excluir(10050);

            //Assertivas            

        }



        [TestMethod]
        public void Pode_Retornar_Candidatos_Por_Vestibular()
        {
            //Ambiente
            vestContext.Vestibulares.Add(vestibularInserir);

            //Criar Curso
            var cursoInserir = new Curso()
            {
                sDescricao = "Analise de Sistemas",
                iVagas = 100
            };

            vestContext.Cursos.Add(cursoInserir);
            vestContext.SaveChanges();


            //Criar Candidato1
            var candidato = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1982, 4, 11),
                sCpf = "295.895.638.56",
                sEmail = "r.humberto.sa@gmail.com",
                Vestibular = vestibularInserir,
                sNome = "Ricardo",
                sSenha = "123",
                sSexo = "M",
                sTelefone = "11"
            };

            vestContext.Candidatos.Add(candidato);
            vestContext.SaveChanges();

            //Criar Candidato1
            var candidato1 = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1982, 4, 28),
                sCpf = "297.496.428.13",
                sEmail = "michelle.sa@gmail.com",
                Vestibular = vestibularInserir,
                sNome = "Michelle",
                sSenha = "123",
                sSexo = "F",
                sTelefone = "11"
            };

            vestContext.Candidatos.Add(candidato1);
            vestContext.SaveChanges();
            //Ação

            var candidatos = vestibularRepository.RetornarCandidatosPorVesntibular(vestibularInserir.iVestibularId);

            //Assertivas            
            Assert.AreEqual(2,candidatos.Count);
            Assert.IsTrue(candidatos.Contains(candidato));
            Assert.IsTrue(candidatos.Contains(candidato1));

        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Candidato_Algum_Inscrito_Test()
        {
            //Ambiente
            vestContext.Vestibulares.Add(vestibularInserir);

            //Criar Curso
            var cursoInserir = new Curso()
            {
                sDescricao = "Analise de Sistemas",
                iVagas = 100
            };

            vestContext.Cursos.Add(cursoInserir);
            vestContext.SaveChanges();


            //Criar Candidato1
            var candidato = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1982, 4, 11),
                sCpf = "295.895.638.56",
                sEmail = "r.humberto.sa@gmail.com",
                Vestibular = vestibularInserir,
                sNome = "Ricardo",
                sSenha = "123",
                sSexo = "M",
                sTelefone = "11"
            };

            vestContext.Candidatos.Add(candidato);
            vestContext.SaveChanges();

            //Criar Candidato1
            var candidato1 = new Candidato()
            {
                Curso = cursoInserir,
                dtNascimento = new DateTime(1982, 4, 28),
                sCpf = "297.496.428.13",
                sEmail = "michelle.sa@gmail.com",
                Vestibular = vestibularInserir,
                sNome = "Michelle",
                sSenha = "123",
                sSexo = "F",
                sTelefone = "11"
            };

            vestContext.Candidatos.Add(candidato1);
            vestContext.SaveChanges();

            //Ação
            vestibularRepository.Excluir(candidato1.iCandidatoId);

            //Assertivas            

        }

        [TestCleanup]
        public void LimparCenario()
        {
            var VestibuarParaRemover = from a in vestContext.Vestibulares
                                       select a;

            foreach (var vestibular in VestibuarParaRemover)
            {
                vestContext.Vestibulares.Remove(vestibular);

            }

            vestContext.SaveChanges();
        }
    }
}
