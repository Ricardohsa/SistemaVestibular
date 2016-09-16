using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Syntax;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.WebUI.Infraestrutura.Provider.Abstract;
using SisVest.WebUI.Infraestrutura.Provider.Concrete;

namespace SisVest.WebUI.Infraestrutura
{
    public class NinjectDependecyResolver :IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependecyResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }
        
        public IKernel Kernel { get { return _kernel; } }

        private IBindingToSyntax<T> Bind<T>()
        {
            return _kernel.Bind<T>();
        }

        private void AddBindings()
        {
            Bind<ICursoRepository>().To<EfCursoRepository>();
            Bind<IAdimRepository>().To<EfAdminRepository>();
            Bind<IVestibularRepository>().To<EfVestibularRepository>();
            Bind<ICandidatoRepository>().To<EfCandidatoRepository>();
            Bind<IAutenticacaoProvider>().To<CustomAutentication>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}