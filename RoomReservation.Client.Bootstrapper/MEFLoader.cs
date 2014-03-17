using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Client.Proxies;

namespace RoomReservation.Client.Bootstrapper
{
    public static  class MEFLoader
    {
        public static CompositionContainer Init()
        {
            return Init(null);
        }


        public static CompositionContainer Init(ICollection<ComposablePartCatalog> catalogParts)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(InventoryClient).Assembly));
            if (catalogParts != null)
            {
                foreach (var catalogPart in catalogParts)
                {
                    catalog.Catalogs.Add(catalogPart);
                }
            }

            CompositionContainer container = new CompositionContainer(catalog);

            return container;
        }
    }
}
