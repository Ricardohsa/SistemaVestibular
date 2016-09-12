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
        private Admin adminInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            adminRepository = new EFAdminRepository(vestContext);

            var adminInserir = (new Admin()
            {
                sEmail = "r.humberto.sa@gmail.com",
                sLogin = "rhumbertosa",
                sNomeTratamento = "Ricardo",
                sSenha = "dmt8017"
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

            var retorno = (from a in adminRepository.admins
                           where a.sLogin.Equals(adminInserir.sLogin)
                           select a).FirstOrDefault();

            //Assertivas            
            Assert.AreEqual(retorno, adminInserir);

        }

        [TestCleanup]
        public void LimparCenario()
        {
            var adminExclusao = (from a in vestContext.Admins
                                 where a.sLogin.Equals(adminInserir.sLogin)
                                 select a);

            if (adminExclusao.Count() > 0)
            {
                vestContext.Admins.Remove(adminExclusao.FirstOrDefault());

                vestContext.SaveChanges();
            }

        }
    }
}


  