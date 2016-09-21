using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Entities;
using SisVest.WebUI.Controllers;

namespace SisVest.Test.Controller
{
    [TestClass]
    public class VestibularControllerTest
    {
        private Mock<IVestibularRepository> mockVestibular;
        private VestibularController _vestibularController;


        [TestInitialize]
        public void TesteInitialize()
        {
            mockVestibular = new Mock<IVestibularRepository>();
            mockVestibular.Setup(a => a.Vestibulares).Returns(new[]
            {
                new Vestibular()
                {
                    IVestibularId = 1,
                    SDescricao = "Vest 2016",
                    DtInicioInscricao = new DateTime(01/10/2016),
                    DtFimInscricao = new DateTime(31/10/2016),
                    DtProva = new DateTime(01/11/2016)
                },

                new Vestibular()
                {
                    IVestibularId = 2,
                    SDescricao = "Vest 2017",
                    DtInicioInscricao = new DateTime(01/01/2017),
                    DtFimInscricao = new DateTime(31/01/2017),
                    DtProva = new DateTime(01/01/2017).AddDays(7)
                },

                 new Vestibular()
                {
                    IVestibularId =3,
                    SDescricao = "Vest 2017 2º Chamada",
                    DtInicioInscricao = new DateTime(01/06/2017),
                    DtFimInscricao = new DateTime(30/06/2017),
                    DtProva = new DateTime(30/06/2017).AddDays(7)
                }
            }.AsQueryable());

            mockVestibular.Setup(a => a.Inserir(It.IsAny<Vestibular>()));
            mockVestibular.Setup(a => a.Inserir(It.Is<Vestibular>(v => v.SDescricao == "FUVEST 2018")))
                .Throws<InvalidOperationException>();

            mockVestibular.Setup(a => a.Excluir(It.Is<int>(i => i == 1)))
                .Throws<InvalidOperationException>();

            _vestibularController = new VestibularController(mockVestibular.Object,null);
        }


        [TestMethod]
        public void Pode_Retornar_Todos_Vestibulares_Index_Test()
        {
            //Ação
            var result = _vestibularController.Index() as ViewResult;

            //Assertivas
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(List<Vestibular>));
            Assert.AreEqual( ((List<Vestibular>)result.Model).Count, 3);
        }

        [TestMethod]
        public void Pode_Retornar_Um_Vestibular_Por_ID_Alterar_Test()
        {
            //Ambiente
            var idVestibular = 1;

            //Ação
            var result = _vestibularController.Alterar(idVestibular) as ViewResult;

            //Assertivas
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(Vestibular));
            Assert.AreEqual("Vest 2016", ((Vestibular)result.Model).SDescricao );

        }

        [TestMethod]
        public void Pode_Retornar_Um_Vestibular_Por_ID_Excluir_Test()
        {
            //Ambiente
            var idVestibular = 1;

            //Ação
            var result = _vestibularController.Excluir(idVestibular) as ViewResult;

            //Assertivas
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(Vestibular));
            Assert.AreEqual("Vest 2016", ((Vestibular)result.Model).SDescricao);
        }


        [TestMethod]
        public void Pode_Inserir_Vestibular_Test()
        {
            //Ambiente
            var vestibular = new Vestibular()
            {
                SDescricao = "FUVEST 2017",
                DtInicioInscricao = new DateTime(01/12/2016),
                DtFimInscricao = new DateTime(31 / 12 / 2016),
                DtProva = new DateTime(01 / 12 / 2016).AddDays(30)
            };

           //Ação
            var result = _vestibularController.Inserir(vestibular);
          

            //Assertivas
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Index",((RedirectToRouteResult)result).RouteValues.FirstOrDefault(a => a.Key == "action").Value);
            Assert.AreEqual("Vestibular cadastrado com sucesso.", _vestibularController.TempData.FirstOrDefault(a => a.Key == "Mensagem").Value);
        }

        [TestMethod]
        public void Nao_Pode_Inserir_Vestibular_Com_Erro_No_Repositorio_Test()
        {
            //Ambiente
            var vestibular = new Vestibular()
            {
                SDescricao = "FUVEST 2018",
                DtInicioInscricao = new DateTime(01 / 12 / 2016),
                DtFimInscricao = new DateTime(31 / 12 / 2016),
                DtProva = new DateTime(01 / 12 / 2016).AddDays(30)
            };

            //Ação
            var result = _vestibularController.Inserir(vestibular);


            //Assertivas
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues.FirstOrDefault(a => a.Key == "action").Value);
            Assert.AreNotEqual("Vestibular cadastrado com sucesso.", _vestibularController.TempData.FirstOrDefault(a => a.Key == "Mensagem").Value);
        }

        [TestMethod]
        public void  Nao_Pode_Inserir_Vestibular_Com_ModelState_Invalido_Test()
        {
            //Ambiente
            var vestibular = new Vestibular()
            {
                SDescricao = "",
                DtInicioInscricao = new DateTime(01 / 12 / 2016),
                DtFimInscricao = new DateTime(31 / 12 / 2016),
                DtProva = new DateTime(01 / 12 / 2016).AddDays(30)
            };
            _vestibularController.ModelState.AddModelError("SDescricao","Descrição não pode ser vazia");

            //Ação
            var result = _vestibularController.Inserir(vestibular);

            //Assertivas
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType( ((ViewResult)result).Model, typeof(Vestibular) );
            Assert.AreEqual(vestibular, ((ViewResult)result).Model);
            
        }



        [TestMethod]
        public void Deve_Redirecionar_Para_Index_Ao_Alterar_Vestibular_Test()
        {
            //Ambiente
            var vestibular = new Vestibular()
            {
                SDescricao = "FUVEST 2017",
                DtInicioInscricao = new DateTime(01 / 12 / 2016),
                DtFimInscricao = new DateTime(31 / 12 / 2016),
                DtProva = new DateTime(01 / 12 / 2016).AddDays(30)
            };

            //Ação
            var result = _vestibularController.Alterar(vestibular);


            //Assertivas
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues.FirstOrDefault(a => a.Key == "action").Value);
            Assert.AreEqual("Vestibular alterado com sucesso.", _vestibularController.TempData.FirstOrDefault(a => a.Key == "Mensagem").Value);
        }



        [TestMethod]
        public void Nao_Pode_Alterar_Vestibular_Com_ModelState_Invalido_Test()
        {
            //Ambiente
            var vestibular = new Vestibular()
            {
                
                SDescricao = "",
                DtInicioInscricao = new DateTime(01 / 12 / 2016),
                DtFimInscricao = new DateTime(31 / 12 / 2016),
                DtProva = new DateTime(01 / 12 / 2016).AddDays(30)
            };
            _vestibularController.ModelState.AddModelError("SDescricao", "Descrição não pode ser vazia");

            //Ação
            var result = _vestibularController.Alterar(vestibular);

            //Assertivas
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(((ViewResult)result).Model, typeof(Vestibular));
            Assert.AreEqual(vestibular, ((ViewResult)result).Model);

        }



        [TestMethod]
        public void Deve_Redirecionar_Para_Index_Ao_Excluir_Vestibular_Test()
        {
            //Ambiente
            var vestibular = new Vestibular()
            {
                SDescricao = "FUVEST 2017",
                DtInicioInscricao = new DateTime(01 / 12 / 2016),
                DtFimInscricao = new DateTime(31 / 12 / 2016),
                DtProva = new DateTime(01 / 12 / 2016).AddDays(30)
            };
            //Ação
            var result = _vestibularController.Excluir(vestibular);


            //Assertivas
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues.FirstOrDefault(a => a.Key == "action").Value);
            Assert.AreEqual("O Vestibular FUVEST 2017 excluido com sucesso.", _vestibularController.TempData.FirstOrDefault(a => a.Key == "Mensagem").Value);
        }

        [TestMethod]
        public void Nao_Devolver_Mensagem_De_Sucesso_Vestibular_Excluir_Test()
        {
            
            //Ambiente
            var vestibular = new Vestibular()
            {
                IVestibularId = 1,
                SDescricao = "FUVEST 2018",
                DtInicioInscricao = new DateTime(01 / 12 / 2016),
                DtFimInscricao = new DateTime(31 / 12 / 2016),
                DtProva = new DateTime(01 / 12 / 2016).AddDays(30)
            };

            //Ação
            var result = _vestibularController.Excluir(vestibular);


            //Assertivas
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues.FirstOrDefault(a => a.Key == "action").Value);
            Assert.AreNotEqual("Vestibular cadastrado com sucesso.", _vestibularController.TempData.FirstOrDefault(a => a.Key == "Mensagem").Value);

        }

        [TestMethod]
        public void Pode_Inserir_Test()
        {
            //Ação
            var result = _vestibularController.Inserir() as ViewResult;

            //Assertivas
            //Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

    }
}
