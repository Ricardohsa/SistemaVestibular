using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class VestibularTest
    {
        public Vestibular Vestibular1, Vestibular2;

        [TestInitialize]
        public void Inicialize()
        {
            Vestibular1 = new Vestibular()
            {
                IVestibularId = 1,
                DtInicioInscricao = new DateTime(2016, 09, 01),
                DtFimInscricao = new DateTime(2016, 10, 31),
                DtProva = new DateTime(2016, 11, 07),
                SDescricao = "Vestibular 2017"

            };
        }

        [TestMethod]
        public void Garantir_Que_2_Vestibulares_Sao_Iguais_Quando_Tem_Id_Iguais()
        {
            Vestibular2 = new Vestibular()
            {
                IVestibularId = 1,
                DtInicioInscricao = new DateTime(2016, 09, 01),
                DtFimInscricao = new DateTime(2016, 10, 31),
                DtProva = new DateTime(2016, 11, 07),
                SDescricao = "Vestibular 2017"

            };

            Assert.AreEqual(Vestibular1.IVestibularId, Vestibular2.IVestibularId);
            Assert.AreEqual(Vestibular1, Vestibular2);
        }

        [TestMethod]
        public void Garantir_Que_2_Vestibulares_Sao_Iguais_Quando_Tem_Id_Descricao_Iguais()
        {
            Vestibular2 = new Vestibular()
            {
                IVestibularId = 1,
                DtInicioInscricao = new DateTime(2016, 09, 01),
                DtFimInscricao = new DateTime(2016, 10, 31),
                DtProva = new DateTime(2016, 11, 07),
                SDescricao = "Vestibular 2017"

            };

            Assert.AreEqual(Vestibular1.IVestibularId, Vestibular2.IVestibularId);
            Assert.AreEqual(Vestibular1.SDescricao, Vestibular2.SDescricao);
            Assert.AreEqual(Vestibular1, Vestibular2);
        }

        [TestMethod]
        public void Garantir_Que_2_Vestibulares_Sao_Iguais_Quando_Tem_Id_Descricao_DTInicio_DTFim_Iguais()
        {
            Vestibular2 = new Vestibular()
            {
                IVestibularId = 1,
                DtInicioInscricao = new DateTime(2016, 09, 01),
                DtFimInscricao = new DateTime(2016, 10, 31),
                DtProva = new DateTime(2016, 11, 07),
                SDescricao = "Vestibular 2017"

            };

            Assert.AreEqual(Vestibular1.IVestibularId, Vestibular2.IVestibularId);
            Assert.AreEqual(Vestibular1.SDescricao, Vestibular2.SDescricao);           
            Assert.AreEqual(Vestibular1, Vestibular2);
        }
    }
}

