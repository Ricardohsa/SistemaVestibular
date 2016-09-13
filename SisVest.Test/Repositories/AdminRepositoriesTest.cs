using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.DomainModel.Entities;
using System.Linq;

namespace SisVest.Test.Repository
{
    [TestClass]
    public class AdminRepositoriesTest
    {
        private IAdimRepository adminRepository;
        private VestContext vestContext = new VestContext();
        public Admin adminInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            adminRepository = new EFAdminRepository(vestContext);

            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "123456"
            });
        }

        [TestMethod]
        public void Pode_Consultar_Test()
        {
            //Ambiente    
            vestContext.Admins.Add(adminInserir);

            vestContext.SaveChanges();
            //Ação

            var admins = adminRepository.admins;

            var retorno = (from a in admins
                           where a.sLogin.Equals(adminInserir.sLogin)
                           select a).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(admins, typeof(IQueryable<Admin>));
            Assert.AreEqual(retorno, adminInserir);

        }

        [TestMethod]
        public void Pode_Inserir_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            //Ação

            adminRepository.Inserir(adminInserir);
            vestContext.SaveChanges();

            var retorno = (from a in adminRepository.admins
                           where a.sLogin.Equals(adminInserir.sLogin)
                           select a).FirstOrDefault();

            //Assertivas            
            Assert.AreEqual(retorno, adminInserir);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Pode_Inserir_Admin_Com_Mesmo_Email_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            var adminInserir2 = (new Admin()
            {
                sEmail = adminInserir.sEmail,
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            adminRepository.Inserir(adminInserir);
            //Ação            
            adminRepository.Inserir(adminInserir2);


            //Assertivas            


        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Pode_Inserir_Admin_Com_Mesmo_Login_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            var adminInserir2 = (new Admin()
            {
                sEmail = "teste@gmail.com.br",
                sLogin = adminInserir.sLogin,
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            adminRepository.Inserir(adminInserir);
            //Ação

            adminRepository.Inserir(adminInserir2);

            //Assertivas            

        }




        [TestMethod]
        public void Pode_Altera_Test()
        {
            //Ambiente    
            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            adminRepository.Inserir(adminInserir);

            var emailEsperado = adminInserir.sEmail;
            var loginEsperado = adminInserir.sLogin;
            var nomeTratamentoEsperado = adminInserir.sNomeTratamento;
            var senhaEsperada = adminInserir.sSenha;


            var adminAlterar = (from a in adminRepository.admins
                                where a.iAdminId == adminInserir.iAdminId
                                select a).FirstOrDefault();

            adminAlterar.sEmail = "miguel@gmail.com";
            adminAlterar.sLogin = "Miguelsa";
            adminAlterar.sNomeTratamento = "Miguel";
            adminAlterar.sSenha = "45644";

            //Ação
            adminRepository.Alterar(adminAlterar);

            var retorno = (from a in adminRepository.admins
                          where a.iAdminId.Equals(adminInserir.iAdminId)
                          select a).FirstOrDefault();
            //Assertivas
            Assert.AreEqual(retorno.iAdminId, adminAlterar.iAdminId);
            Assert.AreNotEqual(emailEsperado, adminAlterar.sEmail);
            Assert.AreNotEqual(loginEsperado, adminAlterar.sLogin);
            Assert.AreNotEqual(nomeTratamentoEsperado, adminAlterar.sNomeTratamento);
            Assert.AreNotEqual(senhaEsperada, adminAlterar.sSenha);

        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Alterar_Admin_Com_Mesmo_Email_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            adminRepository.Inserir(adminInserir);

            var adminAlterar = (from a in adminRepository.admins
                                where a.iAdminId == adminInserir.iAdminId
                                select a).FirstOrDefault();

            adminAlterar.sEmail = adminInserir.sEmail;
            adminAlterar.sLogin = "Miguelsa";
            adminAlterar.sNomeTratamento = "Miguel";
            adminAlterar.sSenha = "45644";

            //Ação
            adminRepository.Alterar(adminAlterar);


            //Assertivas            


        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Alterar_Admin_Com_Mesmo_Login_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            adminRepository.Inserir(adminInserir);

            var adminAlterar = (from a in adminRepository.admins
                                where a.iAdminId == adminInserir.iAdminId
                                select a).FirstOrDefault();

            adminAlterar.sEmail = "miguel@gmail.com";
            adminAlterar.sLogin = adminInserir.sLogin;
            adminAlterar.sNomeTratamento = "Miguel";
            adminAlterar.sSenha = "45644";

            //Ação
            adminRepository.Alterar(adminAlterar);

            //Assertivas            

        }

        [TestMethod]
        public void Pode_Excluir_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            adminRepository.Inserir(adminInserir);
            

            //Ação
            adminRepository.Exclir(adminInserir.iAdminId);

            //Assertivas            
            var result = from a in vestContext.Admins
                         where a.iAdminId.Equals(adminInserir.iAdminId)
                                    select a;

            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Dados_Que_Nao_Existe_Test()
        {
            //Ambiente
          
            
            //Ação
            adminRepository.Exclir(10050);

            //Assertivas            
            
        }



        [TestMethod]
        public void Retornar_Dados_Por_ID_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
            });

            adminRepository.Inserir(adminInserir);

            //Ação
            
            var result = adminRepository.Retornar(adminInserir.iAdminId);

            //Assertivas            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Admin));
            Assert.AreEqual(adminInserir, result);

        }


        //[TestCleanup]
        //public void LimparCenario()
        //{
        //    var adminsParaRemover = from a in vestContext.Admins
        //                            select a;

        //    foreach (var admin in adminsParaRemover)
        //    {
        //        vestContext.Admins.Remove(admin);

        //    }

        //    vestContext.SaveChanges();
        //}


    }
}


  