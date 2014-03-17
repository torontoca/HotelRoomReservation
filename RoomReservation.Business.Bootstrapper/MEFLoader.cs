using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Hosting;
using RoomReservation.Business.Business_Engines;
using RoomReservation.Data.Data_Repositories;

namespace RoomReservation.Business.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(RoomRentalEngine).Assembly));

            var container = new CompositionContainer(catalog);

            return container;
        }
    }
}
