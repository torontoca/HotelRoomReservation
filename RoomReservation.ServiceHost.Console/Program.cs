using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using RoomReservation.Business.Bootstrapper;
using RoomReservation.Business.Contracts;
using RoomReservation.Business.Managers;
using RoomReservation.Common;
using Core.Common.Core;
using  SM = System.ServiceModel;
using System.Transactions;

namespace RoomReservation.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericPrincipal principal = new GenericPrincipal(new GenericIdentity("Larry"),new string[]{Security.RoomRentalAdminRole});
            Thread.CurrentPrincipal = principal;

            ObjectBase.Container = MEFLoader.Init();

            Console.WriteLine("Starting up services...");
            Console.WriteLine();

            SM.ServiceHost hostInventoryManager = new SM.ServiceHost(typeof (InventoryManager));
            SM.ServiceHost hostRentalManager = new SM.ServiceHost(typeof(RentalManager));
            SM.ServiceHost hostAccountManager = new SM.ServiceHost(typeof(AccountManager));

            StartService(hostInventoryManager,"InventoryManager");
            StartService(hostRentalManager,"RentalManager");
            StartService(hostAccountManager,"AccountManager");

            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
            Console.WriteLine("Reservation monitor started.");


            Console.WriteLine();
            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();

            timer.Stop();
            Console.WriteLine("Reservation monitor stopped.");
            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostRentalManager, "RentalManager");
            StopService(hostAccountManager, "AccountManager");
           
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var rentalManager = new RentalManager();
            var deadReservations = rentalManager.GetDeadReservations();
            if (deadReservations == null) return;

            foreach (var deadReservation in deadReservations)
            {
                using (var scope = new System.Transactions.TransactionScope())
                {
                    try
                    {
                        rentalManager.CancelReservation(deadReservation.ReservationId);

                        //Console.WriteLine("Recycling...");

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                      Console.WriteLine(ex.Message);
                    }
                }
            }
        }


        private static  void StartService(SM.ServiceHost host, string serviceName)
        {
            host.Open();
            Console.WriteLine("Service {0} started.",serviceName);

            foreach (var endPoint in host.Description.Endpoints)
            {
                Console.WriteLine("Listening on the endpoint:");
                Console.WriteLine("Address: {0}",endPoint.Address.Uri.AbsoluteUri);
                Console.WriteLine("Binding: {0}",endPoint.Binding.Name);
                Console.WriteLine("Contract: {0}",endPoint.Contract.ConfigurationName);
            }

            Console.WriteLine();
        }


        private static void StopService(SM.ServiceHost host, string serviceName)
        {
            host.Close();
            Console.WriteLine("Service {0} stopped.",serviceName);
        }
    }
}
