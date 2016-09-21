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


        


    }
}
