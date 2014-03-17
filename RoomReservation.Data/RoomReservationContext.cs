using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomReservation.Business.Entities;
using System.Runtime.Serialization;
using Core.Common.Contracts;

namespace RoomReservation.Data
{
    public class RoomRentalContext : DbContext
    {
        public RoomRentalContext() : base("RoomRental")
        {
          //Database.SetInitializer<RoomRentalContext>(null);
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<RoomRentalContext>());
        }

        public DbSet<Account> AccountSet { get; set; }

        public DbSet<Room> RoomSet { get; set; }

        public DbSet<Rental> RentalSet { get; set; }

        public DbSet<Reservation> ReservationSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Ignore<ExtensionDataObject>();
            // modelBuilder.Ignore<IExtensibleDataObject>();
            modelBuilder.Ignore<IIdentifiableEntity>();

            modelBuilder.Entity<Account>().HasKey(e => e.AccountId).Ignore(e => e.EntityId);
            modelBuilder.Entity<Room>().HasKey(e => e.RoomId).Ignore(e => e.EntityId).Ignore(e => e.CurrentlyRented);
            modelBuilder.Entity<Room>().Property(e => e.Description).IsOptional();
         //   modelBuilder.Entity<Room>().Property( )
            modelBuilder.Entity<Rental>().HasKey(e => e.RentalId).Ignore(e => e.EntityId);
            modelBuilder.Entity<Rental>().Property(e => e.DateReturned).IsOptional();
            modelBuilder.Entity<Reservation>().HasKey(e => e.ReservationId).Ignore(e => e.EntityId);


  //          base.OnModelCreating(modelBuilder);
        }
    }

}
