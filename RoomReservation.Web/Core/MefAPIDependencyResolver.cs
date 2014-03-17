using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Core.Common.Extensions;
using Core.Common.Utils;

namespace RoomReservation.Web.Core
{
    public class MefApiDependencyResolver : IDependencyResolver
    {

        public MefApiDependencyResolver(CompositionContainer container)
        {
            _container = container;
        }

        private readonly CompositionContainer _container;

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return _container.GetExportedValueByType(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetExportedValuesByType(serviceType);
        }

        public void Dispose()
        {
            return;
        }
    }
}