using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class AdminTest
    {
        public Admin admin1, admin2;

        [TestInitialize]
        public void InicializarTeste()
        {
            admin1 = new Admin()
            {
                iAdminId = 1,
                sEmail = "joao@sistema.com.br",
                sLogin = "Joao",
                sNomeTratamento = "joaozinho",
                sSenha = "123456"

            };
           
        }
        
        [TestMethod]
        public void Garantir_Que_2_Admins_Sao_Iguais_Qaundo_Tem_Mesmo_Id()
        {
            admin2 = new Admin()
            {
                iAdminId = 1,
                sEmail = "j.silva@sistema.com.br",
                sLogin = "jsilva",
                sNomeTratamento = "joao",
                sSenha = "147852"

            };
            
            Assert.AreEqual(admin1.iAdminId, admin2.iAdminId);
            Assert.AreEqual(admin1, admin2);
        }

        [TestMethod]
        public void Garantir_Que_2_Admins_Sao_Iguais_Qaundo_Tem_Mesmo_Id_Login()
        {

            admin2 = new Admin()
            {
                iAdminId = 1,
                sEmail = "joao@sistema.com.br",
                sLogin = "Joao",
                sNomeTratamento = "joao",
                sSenha = "147852"

            };

            Assert.AreEqual(admin1.iAdminId, admin2.iAdminId);
            Assert.AreEqual(admin1.sLogin, admin2.sLogin);
            Assert.AreEqual(admin1, admin2);
        }

        [TestMethod]
        public void Garantir_Que_2_Admins_Sao_Iguais_Qaundo_Tem_Mesmo_Id_Login_Email()
        {

            admin2 = new Admin()
            {
                iAdminId = 1,
                sEmail = "joao@sistema.com.br",
                sLogin = "Joao",
                sNomeTratamento = "joao",
                sSenha = "147852"

            };

            Assert.AreEqual(admin1.iAdminId, admin2.iAdminId);
            Assert.AreEqual(admin1.sLogin, admin2.sLogin);
            Assert.AreEqual(admin1.sEmail, admin2.sEmail);
            Assert.AreEqual(admin1, admin2);
        }
    }
}
