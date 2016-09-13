using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class VestibularTest
    {
        public Vestibular vestibular1, vestibular2;

        [TestInitialize]
        public void Inicialize()
        {
            vestibular1 = new Vestibular()
            {
                iVestibularId = 1,
                dtInicioInscricao = new DateTime(2016, 09, 01),
                dtFimInscricao = new DateTime(2016, 10, 31),
                dtProva = new DateTime(2016, 11, 07),
                sDescricao = "Vestibular 2017"

            };
        }

        [TestMethod]
        public void Garantir_Que_2_Vestibulares_Sao_Iguais_Quando_Tem_Id_Iguais()
        {
            vestibular2 = new Vestibular()
            {
                iVestibularId = 1,
                dtInicioInscricao = new DateTime(2016, 09, 01),
                dtFimInscricao = new DateTime(2016, 10, 31),
                dtProva = new DateTime(2016, 11, 07),
                sDescricao = "Vestibular 2017"

            };

            Assert.AreEqual(vestibular1.iVestibularId, vestibular2.iVestibularId);
            Assert.AreEqual(vestibular1, vestibular2);
        }

        [TestMethod]
        public void Garantir_Que_2_Vestibulares_Sao_Iguais_Quando_Tem_Id_Descricao_Iguais()
        {
            vestibular2 = new Vestibular()
            {
                iVestibularId = 1,
                dtInicioInscricao = new DateTime(2016, 09, 01),
                dtFimInscricao = new DateTime(2016, 10, 31),
                dtProva = new DateTime(2016, 11, 07),
                sDescricao = "Vestibular 2017"

            };

            Assert.AreEqual(vestibular1.iVestibularId, vestibular2.iVestibularId);
            Assert.AreEqual(vestibular1.sDescricao, vestibular2.sDescricao);
            Assert.AreEqual(vestibular1, vestibular2);
        }

        [TestMethod]
        public void Garantir_Que_2_Vestibulares_Sao_Iguais_Quando_Tem_Id_Descricao_DTInicio_DTFim_Iguais()
        {
            vestibular2 = new Vestibular()
            {
                iVestibularId = 1,
                dtInicioInscricao = new DateTime(2016, 09, 01),
                dtFimInscricao = new DateTime(2016, 10, 31),
                dtProva = new DateTime(2016, 11, 07),
                sDescricao = "Vestibular 2017"

            };

            Assert.AreEqual(vestibular1.iVestibularId, vestibular2.iVestibularId);
            Assert.AreEqual(vestibular1.sDescricao, vestibular2.sDescricao);           
            Assert.AreEqual(vestibular1, vestibular2);
        }
    }
}

