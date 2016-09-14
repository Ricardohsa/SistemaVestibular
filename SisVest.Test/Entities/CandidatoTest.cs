using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class CandidatoTest
    {
        public Candidato Candidato1, Candidato2;

        [TestInitialize]
        public void InicializarTeste()
        {
            Candidato1 = new Candidato()
            {
                ICandidatoId = 1,
                SCpf = "295.895.638.56",
                SEmail = "jsilva@vestibular.com.br"
            };
        }


        [TestMethod]
        public void Garantir_que_2_Candidatos_Sao_Iguais_Qaundo_Tem_Mesmo_ID()
        {
            Candidato2 = new Candidato()
            {
                ICandidatoId = 1,
                SCpf = "295.895.638.56"
            };

            Assert.AreEqual(Candidato1.ICandidatoId, Candidato2.ICandidatoId);
            Assert.AreEqual(Candidato1, Candidato2);
        }

        [TestMethod]
        public void Garantir_que_2_Candidatos_Sao_Iguais_Qaundo_Tem_Mesmo_ID_Cpf()
        {
            Candidato2 = new Candidato()
            {
                ICandidatoId = 1,
                SCpf = "295.895.638.56"
            };

            Assert.AreEqual(Candidato1.ICandidatoId, Candidato2.ICandidatoId);
            Assert.AreEqual(Candidato1.SCpf, Candidato2.SCpf);
            Assert.AreEqual(Candidato1, Candidato2);
        }

        [TestMethod]
        public void Garantir_que_2_Candidatos_Sao_Iguais_Qaundo_Tem_Mesmo_ID_Cpf_Email()
        {
            Candidato2 = new Candidato()
            {
                ICandidatoId = 1,
                SCpf = "295.895.638.56"
                
            };

            Assert.AreEqual(Candidato1.ICandidatoId, Candidato2.ICandidatoId);                
            Assert.AreEqual(Candidato1, Candidato2);
        }
    }
}
