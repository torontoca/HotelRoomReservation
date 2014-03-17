using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;

namespace RoomReservation.Client.Proxies
{
    [Export(typeof(IServiceFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ServiceFactory : IServiceFactory
    {
        public T CreateClient<T>() where T : class,IServiceContract
        {
            return ObjectBase.Container != null ? ObjectBase.Container.GetExportedValue<T>() : null;
        }
    }
}