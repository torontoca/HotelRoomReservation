using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
//using System.Web.Http.Dependencies;
using System.Web.Mvc;
using Core.Common.Extensions;
using Core.Common.Utils;

namespace RoomReservation.Web.Core
{
    public class MefDependencyResolver : IDependencyResolver
    {
        private readonly CompositionContainer _container;

        public MefDependencyResolver(CompositionContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.GetExportedValueByType(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {

            return _container.GetExportedValuesByType(serviceType);
        }
    
    }
}