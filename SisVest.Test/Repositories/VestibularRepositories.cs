using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Repositories
{
    [TestClass]
    public class VestibularRepositories
    {
        private IVestibularRepository _vestibularRepository;
        private VestContext _vestContext = new VestContext();
        private Vestibular _vestibularInserir;

        [TestInitialize]
        public void InicializarTeste()
        {
            _vestibularRepository = new EfVestibularRepository(_vestContext);

            _vestibularInserir = (new Vestibular()
            {

                DtInicioInscricao = new DateTime(2016, 3, 25),
                DtFimInscricao = new DateTime(2016, 3, 25).AddDays(5),
                DtProva = new DateTime(2016, 3, 25).AddDays(7),
                SDescricao = "Vestibular 2017"

            });
        }

        [TestMethod]
        public void Pode_Consultar_Usando_LINQ_Repositorio_Test()
        {
            //Ambiente    
            _vestContext.Vestibulares.Add(_vestibularInserir);
            _vestContext.SaveChanges();
            //Ação

            var vestibulares = _vestibularRepository.Vestibulares;

            var retorno = (from a in vestibulares
                           where a.SDescricao.Equals(_vestibularInserir.SDescricao)
                           select a).FirstOrDefault();

            //Assertivas
            Assert.IsInstanceOfType(vestibulares, typeof(IQueryable<Vestibular>));
            Assert.AreEqual(retorno, _vestibularInserir);

        }

        [TestMethod]
        public void Pode_Inserir_Vestibular_Test()
        {
            //Ambiente


            //Ação

            _vestibularRepository.Inserir(_vestibularInserir);
            _vestContext.SaveChanges();

            var retorno = (from a in _vestibularRepository.Vestibulares
                           where a.SDescricao.Equals(_vestibularInserir.SDescricao)
                           select a).FirstOrDefault();

            //Assertivas            
            Assert.AreEqual(retorno, _vestibularInserir);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Vestibular_Com_Mesma_Descricao_Test()
        {
            //Ambiente

            var vestibularInserir2 = (new Vestibular()
            {
                DtInicioInscricao = new DateTime(2016, 3, 25),
                DtFimInscricao = new DateTime(2016, 3, 25).AddDays(5),
                DtProva = new DateTime(2016, 3, 25).AddDays(7),
                SDescricao = _vestibularInserir.SDescricao

            });

            _vestibularRepository.Inserir(_vestibularInserir);
            //Ação            
            _vestibularRepository.Inserir(vestibularInserir2);


            //Assertivas         

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Inserir_Vestibular_Sem_Informar_Datas_Descricao_Test()
        {
            //Ambiente

            var vestibularInserir2 = (new Vestibular()
            {
                SDescricao = "20122"
            });

            _vestibularRepository.Inserir(vestibularInserir2);
            //Ação            


            //Assertivas         

        }


        [TestMethod]
        public void Pode_Altera_Test()
        {
            //Ambiente    
            var descriaoEsperada = _vestibularInserir.SDescricao;

            _vestibularRepository.Inserir(_vestibularInserir);


            var vestibularAlterar = (from a in _vestibularRepository.Vestibulares
                                     where a.IVestibularId == _vestibularInserir.IVestibularId
                                     select a).FirstOrDefault();

            vestibularAlterar.SDescricao = "Vestibular 2012";

            //Ação
            _vestibularRepository.Alterar(vestibularAlterar);

            var retorno = (from a in _vestibularRepository.Vestibulares
                           where a.IVestibularId.Equals(_vestibularInserir.IVestibularId)
                           select a).FirstOrDefault();
            //Assertivas
            Assert.AreEqual(retorno.IVestibularId, vestibularAlterar.IVestibularId);
            Assert.AreNotEqual(descriaoEsperada, vestibularAlterar.SDescricao);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Vestibular_Com_Mesma_Descricao_Test()
        {
            //Ambiente
            _vestibularRepository.Inserir(_vestibularInserir);

            var vestibularAlterar = (from a in _vestibularRepository.Vestibulares
                                     where a.IVestibularId == _vestibularInserir.IVestibularId
                                     select a).FirstOrDefault();

            vestibularAlterar.SDescricao = "Vestibular 2017";

            //Ação
            _vestibularRepository.Alterar(vestibularAlterar);

            //Assertivas      

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Alterar_Curso_Com_Mesma_Descricao_Ja_Persistida_Test()
        {
            //Ambiente
            _vestibularRepository.Inserir(_vestibularInserir);

            var vestibularInserir2 = new Vestibular()
            {
                SDescricao = "2019"
            };

            _vestibularRepository.Inserir(vestibularInserir2);

            var cursoAlterar = (from c in _vestibularRepository.Vestibulares
                                where c.IVestibularId.Equals(_vestibularInserir.IVestibularId)
                                select c).FirstOrDefault();

            cursoAlterar.SDescricao = vestibularInserir2.SDescricao;

            //Ação

            _vestibularRepository.Alterar(cursoAlterar);
            //Assertivas      

        }


        [TestMethod]
        public void Pode_Excluir_Test()
        {
            //Ambiente
            _vestibularRepository.Inserir(_vestibularInserir);


            //Ação
            _vestibularRepository.Excluir(_vestibularInserir.IVestibularId);

            //Assertivas            
            var result = from c in _vestContext.Vestibulares
                         where c.IVestibularId.Equals(_vestibularInserir.IVestibularId)
                         select c;

            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Dados_Que_Nao_Existe_Test()
        {
            //Ambiente


            //Ação
            _vestibularRepository.Excluir(10050);

            //Assertivas            

        }



        [TestMethod]
        public void Pode_Retornar_Candidatos_Por_Vestibular()
        {
            //Ambiente
            _vestContext.Vestibulares.Add(_vestibularInserir);

            //Criar Curso
            var cursoInserir = new Curso()
            {
                SDescricao = "Analise de Sistemas",
                IVagas = 50
            };

            _vestContext.Cursos.Add(cursoInserir);
            _vestContext.SaveChanges();


            //Criar Candidato1
            var candidato = new Candidato()
            {
                Curso = cursoInserir,
                DtNascimento = new DateTime(1982, 4, 11),
                SCpf = "295.895.638.56",
                SEmail = "r.humberto.sa@gmail.com",
                Vestibular = _vestibularInserir,
                SNome = "Ricardo",
                SSenha = "123",
                SSexo = "M",
                STelefone = "11"
            };

            _vestContext.Candidatos.Add(candidato);
            _vestContext.SaveChanges();

            //Criar Candidato1
            var candidato1 = new Candidato()
            {
                Curso = cursoInserir,
                DtNascimento = new DateTime(1982, 4, 28),
                SCpf = "297.496.428.13",
                SEmail = "michelle.sa@gmail.com",
                Vestibular = _vestibularInserir,
                SNome = "Michelle",
                SSenha = "123",
                SSexo = "F",
                STelefone = "11"
            };

            _vestContext.Candidatos.Add(candidato1);
            _vestContext.SaveChanges();
            //Ação

            var candidatos = _vestibularRepository.RetornarCandidatosPorVesntibular(_vestibularInserir.IVestibularId);

            //Assertivas            
            Assert.AreEqual(2,candidatos.Count);
            Assert.IsTrue(candidatos.Contains(candidato));
            Assert.IsTrue(candidatos.Contains(candidato1));

        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Nao_Pode_Excluir_Candidato_Algum_Inscrito_Test()
        {
            //Ambiente
            _vestContext.Vestibulares.Add(_vestibularInserir);

            //Criar Curso
            var cursoInserir = new Curso()
            {
                SDescricao = "Analise de Sistemas",
                IVagas = 45
            };

            _vestContext.Cursos.Add(cursoInserir);
            _vestContext.SaveChanges();


            //Criar Candidato1
            var candidato = new Candidato()
            {
                Curso = cursoInserir,
                DtNascimento = new DateTime(1982, 4, 11),
                SCpf = "295.895.638.56",
                SEmail = "r.humberto.sa@gmail.com",
                Vestibular = _vestibularInserir,
                SNome = "Ricardo",
                SSenha = "123",
                SSexo = "M",
                STelefone = "11"
            };

            _vestContext.Candidatos.Add(candidato);
            _vestContext.SaveChanges();

            //Criar Candidato1
            var candidato1 = new Candidato()
            {
                Curso = cursoInserir,
                DtNascimento = new DateTime(1982, 4, 28),
                SCpf = "297.496.428.13",
                SEmail = "michelle.sa@gmail.com",
                Vestibular = _vestibularInserir,
                SNome = "Michelle",
                SSenha = "123",
                SSexo = "F",
                STelefone = "11"
            };

            _vestContext.Candidatos.Add(candidato1);
            _vestContext.SaveChanges();

            //Ação
            _vestibularRepository.Excluir(candidato1.ICandidatoId);

            //Assertivas            

        }

        [TestCleanup]
        public void LimparCenario()
        {
            var vestibuarParaRemover = from a in _vestContext.Vestibulares
                                       select a;

            foreach (var vestibular in vestibuarParaRemover)
            {
                _vestContext.Vestibulares.Remove(vestibular);

            }

            _vestContext.SaveChanges();
        }
    }
}
