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
        private ICursoRepository  cursoRepository;
        private VestContext vestContext = new VestContext();
        private Curso cursoInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            cursoRepository = new EFCursoRepository(vestContext);

            cursoInserir = (new Curso()
            {
                sDescricao = "Analise de Sistemas",
                iVagas = 100
            });
        }

        [TestMethod]
        public void Pode_Consultar_Usando_LINQ_Repositorio_Test()
        {
            //Ambiente    
            vestContext.Cursos.Add(cursoInserir);
            vestContext.SaveChanges();
            //Ação

            var cursos = cursoRepository.cursos;

            var retorno = (from a in cursos
                           where a.sDescricao.Equals(cursoInserir.sDescricao)
                           select a).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(cursos, typeof(IQueryable<Curso>));
            Assert.AreEqual(retorno, cursoInserir);

        }

        [TestMethod]
        public void Pode_Inserir_Test()
        {
            //Ambiente


            //Ação

            cursoRepository.Inserir(cursoInserir);
            vestContext.SaveChanges();

            var retorno = (from a in cursoRepository.cursos
                           where a.sDescricao.Equals(cursoInserir.sDescricao)
                           select a).FirstOrDefault();

            //Assertivas            
            Assert.AreEqual(retorno, cursoInserir);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Curso_Com_Mesma_Descricao_Test()
        {
            //Ambiente

            var cursoInserir2 = (new Curso()
            {
                sDescricao = cursoInserir.sDescricao,
                iVagas = 100
            });

            cursoRepository.Inserir(cursoInserir);
            //Ação            
            cursoRepository.Inserir(cursoInserir2);


            //Assertivas         

        }


        [TestMethod]
        public void Pode_Altera_Test()
        {
            //Ambiente    
            cursoRepository.Inserir(cursoInserir);

            var descriaoEsperada = cursoInserir.sDescricao;

            var cursoAlterar = (from a in cursoRepository.cursos
                                where a.iCursoId == cursoInserir.iCursoId
                                select a).FirstOrDefault();

            cursoAlterar.sDescricao = "Ciencias da Computação";

            //Ação
            cursoRepository.Alterar(cursoAlterar);

            var retorno = (from a in cursoRepository.cursos
                           where a.iCursoId.Equals(cursoInserir.iCursoId)
                           select a).FirstOrDefault();
            //Assertivas
            Assert.AreEqual(retorno.iCursoId, cursoAlterar.iCursoId);
            Assert.AreNotEqual(descriaoEsperada, cursoAlterar.sDescricao);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Curso_Com_Mesma_Descricao_Test()
        {
            //Ambiente
            cursoRepository.Inserir(cursoInserir);

            var cursoAlterar = (from a in cursoRepository.cursos
                                where a.iCursoId == cursoInserir.iCursoId
                                select a).FirstOrDefault();

            cursoAlterar.sDescricao = "Ciencias da Computação";

            //Ação
            cursoRepository.Alterar(cursoAlterar);

            //Assertivas      

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Curso_Com_Mesma_Descricao_Ja_Persistida_Test()
        {
            //Ambiente
            cursoRepository.Inserir(cursoInserir);

            var cursoInserir2 = new Curso()
            {
                sDescricao = "POS ENGENHARIA SOFTWARE"
            };

            cursoRepository.Inserir(cursoInserir2);

            var cursoAlterar = (from c in cursoRepository.cursos
                                where c.iCursoId.Equals(cursoInserir.iCursoId)
                                select c).FirstOrDefault();

            cursoAlterar.sDescricao = cursoInserir2.sDescricao;

            //Ação

            cursoRepository.Alterar(cursoAlterar);
            //Assertivas      

        }


        [TestMethod]
        public void Pode_Excluir_Test()
        {
            //Ambiente
            cursoRepository.Inserir(cursoInserir);


            //Ação
            cursoRepository.Excluir(cursoInserir.iCursoId);

            //Assertivas            
            var result = from c in vestContext.Cursos
                         where c.iCursoId.Equals(cursoInserir.iCursoId)
                         select c;

            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Dados_Que_Nao_Existe_Test()
        {
            //Ambiente


            //Ação
            cursoRepository.Excluir(10050);

            //Assertivas            

        }



        [TestMethod]
        public void Retornar_Curso_Por_ID_Test()
        {
            //Ambiente
            cursoRepository.Inserir(cursoInserir);

            //Ação

            var result = cursoRepository.Retornar(cursoInserir.iCursoId);

            //Assertivas            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Curso));
            Assert.AreEqual(cursoInserir, result);

        }


        [TestCleanup]
        public void LimparCenario()
        {
            var CursosParaRemover = from a in vestContext.Cursos
                                    select a;

            foreach (var curso in CursosParaRemover)
            {
                vestContext.Cursos.Remove(curso);

            }

            vestContext.SaveChanges();
        }
    }
}
