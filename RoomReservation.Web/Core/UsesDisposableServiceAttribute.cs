using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
//using RoomRental.Common;
using Core.Common.Contracts;

namespace RoomReservation.Web.Core
{
    public class UsesDisposableServiceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // pre-processing

            IServiceAwareController controller = actionContext.ControllerContext.Controller as IServiceAwareController;
            if (controller != null)
            {
                controller.RegisterDisposableServices(controller.DisposableServices);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //post-processing

            IServiceAwareController controller = actionExecutedContext.ActionContext.ControllerContext.Controller as IServiceAwareController;
            if (controller != null)
            {
                foreach (var disposable in controller.DisposableServices.Where(service => service != null && service is IDisposable).OfType<IDisposable>())
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
