using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace InvoiceAppDemo.Infrastructure
{
    public class MEFControllerFactory : DefaultControllerFactory
    {
        private readonly CompositionContainer _container;

        public MEFControllerFactory(CompositionContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;

            try
            {
                Lazy<object, object> export = _container.GetExports(controllerType, null, null).FirstOrDefault();

                if (export == null)
                    return base.GetControllerInstance(requestContext, controllerType);
                return (IController)export.Value;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public override void ReleaseController(IController controller)
        {
            ((IDisposable)controller).Dispose();
        }
    }

}