using System;
using Ninject;
using System.Web.Mvc;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using SisVest.DomainModel.Entities;


namespace SisVest.WebUI.Infraestrutura
{
    public class NinjectControllerFactory :DefaultControllerFactory
    {
        private IKernel _ninjectKernel;

        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBindins();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext,
            Type controllerType)
        {

            return controllerType == null ? null : (IController) _ninjectKernel.Get(controllerType);
        }

        private void AddBindins()
        {
            _ninjectKernel.Bind<ICursoRepository>().To<EfCursoRepository>();
            _ninjectKernel.Bind<IAdimRepository>().To<EfAdminRepository>();
            _ninjectKernel.Bind<IVestibularRepository>().To<EfVestibularRepository>();
            _ninjectKernel.Bind<ICandidatoRepository>().To<EfCandidatoRepository>();
        }

      

    }
}