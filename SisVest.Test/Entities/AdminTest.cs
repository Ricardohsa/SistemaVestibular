using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class AdminTest
    {
        public Admin Admin1, Admin2;

        [TestInitialize]
        public void InicializarTeste()
        {
            Admin1 = new Admin()
            {
                IAdminId = 1,
                SEmail = "joao@sistema.com.br",
                SLogin = "Joao",
                SNomeTratamento = "joaozinho",
                SSenha = "123456"

            };
           
        }
        
        [TestMethod]
        public void Garantir_Que_2_Admins_Sao_Iguais_Qaundo_Tem_Mesmo_Id()
        {
            Admin2 = new Admin()
            {
                IAdminId = 1,
                SEmail = "j.silva@sistema.com.br",
                SLogin = "jsilva",
                SNomeTratamento = "joao",
                SSenha = "147852"

            };
            
            Assert.AreEqual(Admin1.IAdminId, Admin2.IAdminId);
            Assert.AreEqual(Admin1, Admin2);
        }

        [TestMethod]
        public void Garantir_Que_2_Admins_Sao_Iguais_Qaundo_Tem_Mesmo_Id_Login()
        {

            Admin2 = new Admin()
            {
                IAdminId = 1,
                SEmail = "joao@sistema.com.br",
                SLogin = "Joao",
                SNomeTratamento = "joao",
                SSenha = "147852"

            };

            Assert.AreEqual(Admin1.IAdminId, Admin2.IAdminId);
            Assert.AreEqual(Admin1.SLogin, Admin2.SLogin);
            Assert.AreEqual(Admin1, Admin2);
        }

        [TestMethod]
        public void Garantir_Que_2_Admins_Sao_Iguais_Qaundo_Tem_Mesmo_Id_Login_Email()
        {

            Admin2 = new Admin()
            {
                IAdminId = 1,
                SEmail = "joao@sistema.com.br",
                SLogin = "Joao",
                SNomeTratamento = "joao",
                SSenha = "147852"

            };

            Assert.AreEqual(Admin1.IAdminId, Admin2.IAdminId);
            Assert.AreEqual(Admin1.SLogin, Admin2.SLogin);
            Assert.AreEqual(Admin1.SEmail, Admin2.SEmail);
            Assert.AreEqual(Admin1, Admin2);
        }
    }
}
