using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;
using System.Collections.Generic;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class CursoTest
    {
        public Curso curso1, curso2;       

        [TestInitialize]
        public void InicializarTeste()
        {
            curso1 = new Curso()
            {
                iCursoId = 1,
                iVagas = 10,                
                sDescricao = "Analise de Sistemas"
                
            };
        }

        [TestMethod]
        public void Garantir_Que_2_Cursos_Sao_Iguai_Quando_Tem_Mesmo_Id()
        {
            curso2 = new Curso()
            {
                iCursoId = 1,
                iVagas = 10,                
                sDescricao = "Analise de Sistemas"
            };

            Assert.AreEqual(curso1.iCursoId, curso2.iCursoId);
            Assert.AreEqual(curso1, curso2);
        }

        [TestMethod]
        public void Garantir_Que_2_Cursos_Sao_Iguai_Quando_Tem_Mesmo_Id_Descricao()
        {
            curso2 = new Curso()
            {
                iCursoId = 1,
                iVagas = 1000,
                sDescricao = "Analise de Sistemas"
            };

            Assert.AreEqual(curso1.iCursoId, curso2.iCursoId);
            Assert.AreEqual(curso1.sDescricao, curso2.sDescricao);
            Assert.AreEqual(curso1, curso2);
        }
    }
}
