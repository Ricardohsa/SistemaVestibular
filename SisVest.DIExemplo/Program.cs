using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisVest.DomainModel;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using Ninject;

namespace SisVest.DIExemplo
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel ninjectKernel = new StandardKernel();

            ninjectKernel.Bind<ICursoRepository>().To<EfCursoRepository>();
            ninjectKernel.Bind<VestContext>().ToSelf();

            var cursoRepository = ninjectKernel.Get<ICursoRepository>();
            
            foreach (var s in cursoRepository.Cursos.ToList())
            {
                Console.WriteLine($"Curso: {s.SDescricao} - Total Vagas: {s.IVagas} ");
            }
            Console.ReadLine();
        }
    }
}
