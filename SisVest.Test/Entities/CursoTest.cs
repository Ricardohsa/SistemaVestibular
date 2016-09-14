using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;
using System.Collections.Generic;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class CursoTest
    {
        public Curso Curso1, Curso2;       

        [TestInitialize]
        public void InicializarTeste()
        {
            Curso1 = new Curso()
            {
                ICursoId = 1,
                IVagas = 10,                
                SDescricao = "Analise de Sistemas"
                
            };
        }

        [TestMethod]
        public void Garantir_Que_2_Cursos_Sao_Iguai_Quando_Tem_Mesmo_Id()
        {
            Curso2 = new Curso()
            {
                ICursoId = 1,
                IVagas = 10,                
                SDescricao = "Analise de Sistemas"
            };

            Assert.AreEqual(Curso1.ICursoId, Curso2.ICursoId);
            Assert.AreEqual(Curso1, Curso2);
        }

        [TestMethod]
        public void Garantir_Que_2_Cursos_Sao_Iguai_Quando_Tem_Mesmo_Id_Descricao()
        {
            Curso2 = new Curso()
            {
                ICursoId = 1,
                IVagas = 1000,
                SDescricao = "Analise de Sistemas"
            };

            Assert.AreEqual(Curso1.ICursoId, Curso2.ICursoId);
            Assert.AreEqual(Curso1.SDescricao, Curso2.SDescricao);
            Assert.AreEqual(Curso1, Curso2);
        }
    }
}
