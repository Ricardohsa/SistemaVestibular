using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class CandidatoTest
    {
        public Candidato candidato1, candidato2;

        [TestInitialize]
        public void InicializarTeste()
        {
            candidato1 = new Candidato()
            {
                iCandidatoId = 1,
                sCpf = "295.895.638.56",
                sEmail = "jsilva@vestibular.com.br"
            };
        }


        [TestMethod]
        public void Garantir_que_2_Candidatos_Sao_Iguais_Qaundo_Tem_Mesmo_ID()
        {
            candidato2 = new Candidato()
            {
                iCandidatoId = 1,
                sCpf = "295.895.638.56"
            };

            Assert.AreEqual(candidato1.iCandidatoId, candidato2.iCandidatoId);
            Assert.AreEqual(candidato1, candidato2);
        }

        [TestMethod]
        public void Garantir_que_2_Candidatos_Sao_Iguais_Qaundo_Tem_Mesmo_ID_Cpf()
        {
            candidato2 = new Candidato()
            {
                iCandidatoId = 1,
                sCpf = "295.895.638.56"
            };

            Assert.AreEqual(candidato1.iCandidatoId, candidato2.iCandidatoId);
            Assert.AreEqual(candidato1.sCpf, candidato2.sCpf);
            Assert.AreEqual(candidato1, candidato2);
        }

        [TestMethod]
        public void Garantir_que_2_Candidatos_Sao_Iguais_Qaundo_Tem_Mesmo_ID_Cpf_Email()
        {
            candidato2 = new Candidato()
            {
                iCandidatoId = 1,
                sCpf = "295.895.638.56"
                
            };

            Assert.AreEqual(candidato1.iCandidatoId, candidato2.iCandidatoId);                
            Assert.AreEqual(candidato1, candidato2);
        }
    }
}
