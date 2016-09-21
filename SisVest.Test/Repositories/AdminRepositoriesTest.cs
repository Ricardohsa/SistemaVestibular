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
        private IAdimRepository _adminRepository;
        private VestContext _vestContext = new VestContext();
        private Admin _adminInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            _adminRepository = new EfAdminRepository(_vestContext);

            _adminInserir = (new Admin()
            {
                SEmail = "r.humberto.sa@gmail.com",
                SLogin = "rhumbertosa",
                SNomeTratamento = "Ricardo",
                SSenha = "123456"
            });
        }

        [TestMethod]
        public void Pode_Consultar_Test()
        {
            //Ambiente    


            _vestContext.Admins.Add(_adminInserir);

            _vestContext.SaveChanges();
            //Ação

            var admins = _adminRepository.Admins;

            var retorno = (from a in admins
                           where a.SLogin.Equals(_adminInserir.SLogin)
                           select a).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(admins, typeof(IQueryable<Admin>));
            Assert.AreEqual(retorno, _adminInserir);

        }

        [TestMethod]
        public void Pode_Inserir_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                SEmail = "r.humberto.sa@gmail.com",
                SLogin = "rhumbertosa",
                SNomeTratamento = "Ricardo",
                SSenha = "dmt8017"
            });

            //Ação

            _adminRepository.Inserir(adminInserir);
            _vestContext.SaveChanges();

            var retorno = (from a in _adminRepository.Admins
                           where a.SLogin.Equals(adminInserir.SLogin)
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
                SEmail = "r.humberto.sa@gmail.com",
                SLogin = "rhumbertosa",
                SNomeTratamento = "Ricardo",
                SSenha = "dmt8017"
            });

            var adminInserir2 = (new Admin()
            {
                SEmail = adminInserir.SEmail,
                SLogin = "rhumbertosa",
                SNomeTratamento = "Ricardo",
                SSenha = "dmt8017"
            });

            _adminRepository.Inserir(adminInserir);
            //Ação            
            _adminRepository.Inserir(adminInserir2);


            //Assertivas            


        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Pode_Inserir_Admin_Com_Mesmo_Login_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                SEmail = "r.humberto.sa@gmail.com",
                SLogin = "rhumbertosa",
                SNomeTratamento = "Ricardo",
                SSenha = "dmt8017"
            });

            var adminInserir2 = (new Admin()
            {
                SEmail = "teste@gmail.com.br",
                SLogin = adminInserir.SLogin,
                SNomeTratamento = "Ricardo",
                SSenha = "dmt8017"
            });

            _adminRepository.Inserir(adminInserir);
            //Ação

            _adminRepository.Inserir(adminInserir2);

            //Assertivas            

        }




        [TestMethod]
        public void Pode_Altera_Test()
        {
            //Ambiente    
            var admin1 = (new Admin()
            {
                SEmail = "julio.sa@gmail.com",
                SLogin = "juliosa",
                SNomeTratamento = "Ricardo",
                SSenha = "dmt8017"
            });

            _adminRepository.Inserir(admin1);
            _vestContext.SaveChanges();

            var emailEsperado = admin1.SEmail;
            var loginEsperado = admin1.SLogin;
            var nomeTratamentoEsperado = admin1.SNomeTratamento;
            var senhaEsperada = admin1.SSenha;


            var adminAlterar = (from a in _adminRepository.Admins
                                where a.IAdminId == admin1.IAdminId
                                select a).FirstOrDefault();

            adminAlterar.SEmail = "teste@gmail.com";
            adminAlterar.SLogin = "teste123";
            adminAlterar.SNomeTratamento = "Miguel";
            adminAlterar.SSenha = "45644";

            //Ação
            _adminRepository.Alterar(adminAlterar);

            var retorno = (from a in _adminRepository.Admins
                          where a.IAdminId.Equals(admin1.IAdminId)
                          select a).FirstOrDefault();
            //Assertivas
            Assert.AreEqual(retorno.IAdminId, adminAlterar.IAdminId);
            Assert.AreNotEqual(emailEsperado, adminAlterar.SEmail);
            Assert.AreNotEqual(loginEsperado, adminAlterar.SLogin);
            Assert.AreNotEqual(nomeTratamentoEsperado, adminAlterar.SNomeTratamento);
            Assert.AreNotEqual(senhaEsperada, adminAlterar.SSenha);

        }


        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void Nao_Alterar_Admin_Com_Mesmo_Email_Test()
        //{
        //    //Ambiente
        //    var adminInserir = (new Admin()
        //    {
        //        SEmail = "r.humberto.sa@gmail.com",
        //        SLogin = "rhumbertosa",
        //        SNomeTratamento = "Ricardo",
        //        SSenha = "dmt8017"
        //    });

        //    _adminRepository.Inserir(adminInserir);

        //    var adminAlterar = (from a in _adminRepository.Admins
        //                        where a.IAdminId == adminInserir.IAdminId
        //                        select a).FirstOrDefault();

        //    adminAlterar.SEmail = adminInserir.SEmail;
        //    adminAlterar.SLogin = "rhumbertosa";
        //    adminAlterar.SNomeTratamento = "Miguel";
        //    adminAlterar.SSenha = "45644";

        //    //Ação
        //    _adminRepository.Alterar(adminAlterar);


        //    //Assertivas            


        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void Nao_Alterar_Admin_Com_Mesmo_Login_Test()
        //{
        //    //Ambiente
        //    var adminInserir = (new Admin()
        //    {
        //        SEmail = "r.humberto.sa@gmail.com",
        //        SLogin = "rhumbertosa",
        //        SNomeTratamento = "Ricardo",
        //        SSenha = "dmt8017"
        //    });

        //    _adminRepository.Inserir(adminInserir);

        //    var adminAlterar = (from a in _adminRepository.Admins
        //                        where a.IAdminId == adminInserir.IAdminId
        //                        select a).FirstOrDefault();

        //    adminAlterar.SEmail = "miguel@gmail.com";
        //    adminAlterar.SLogin = adminInserir.SLogin;
        //    adminAlterar.SNomeTratamento = "Miguel";
        //    adminAlterar.SSenha = "45644";

        //    //Ação
        //    _adminRepository.Alterar(adminAlterar);

        //    //Assertivas            

        //}

        [TestMethod]
        public void Pode_Excluir_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                SEmail = "r.humberto.sa@gmail.com",
                SLogin = "rhumbertosa",
                SNomeTratamento = "Ricardo",
                SSenha = "dmt8017"
            });

            _adminRepository.Inserir(adminInserir);
            

            //Ação
            _adminRepository.Excluir(adminInserir.IAdminId);

            //Assertivas            
            var result = from a in _vestContext.Admins
                         where a.IAdminId.Equals(adminInserir.IAdminId)
                                    select a;

            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Dados_Que_Nao_Existe_Test()
        {
            //Ambiente
          
            
            //Ação
            _adminRepository.Excluir(10050);

            //Assertivas            
            
        }



        [TestMethod]
        public void Retornar_Dados_Por_ID_Test()
        {
            //Ambiente
            var adminInserir = (new Admin()
            {
                SEmail = "r.humberto.sa@gmail.com",
                SLogin = "rhumbertosa",
                SNomeTratamento = "Ricardo",
                SSenha = "dmt8017"
            });

            _adminRepository.Inserir(adminInserir);

            //Ação
            
            var result = _adminRepository.Retornar(adminInserir.IAdminId);

            //Assertivas            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Admin));
            Assert.AreEqual(adminInserir, result);

        }


        [TestCleanup]
        public void LimparCenario()
        {
            var adminsParaRemover = from a in _vestContext.Admins
                                    select a;

            foreach (var admin in adminsParaRemover)
            {
                _vestContext.Admins.Remove(admin);

            }

            _vestContext.SaveChanges();
        }


    }
}


  