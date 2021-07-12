using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ent = DomainModel.Entities;

namespace DomainModel
{
    public class VehicleRentalContext : DbContext
    {
        public VehicleRentalContext() : base("name=VehicleRentalConnectionString")
        {
            Database.SetInitializer<VehicleRentalContext>(new MigrateDatabaseToLatestVersion<VehicleRentalContext, DomainModel.Migrations.Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ent.MotorVehicle>()
                .Property(mv => mv.LocationId)
                .IsOptional();

            //modelBuilder.Entity<Ent.MotorVehicle>()
            //    .HasRequired<Ent.MotorVehicleModel>(mv => mv.MotorVehicleModel)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Ent.MotorVehicleModel>()
            //    .HasRequired<Ent.VehicleMake>(mvm => mvm.VehicleMake)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);
        }

        public DbSet<Ent.VehicleMake> VehicleMakes { get; set; }
        public DbSet<Ent.MotorVehicleModel> MotorVehicleModels { get; set; }
        public DbSet<Ent.MotorVehicle> MotorVehicles { get; set; }
        public DbSet<Ent.Country> Countries { get; set; }
        public DbSet<Ent.State> States { get; set; }
        public DbSet<Ent.City> Cities { get; set; }
        public DbSet<Ent.Suburb> Suburbs { get; set; }
        public DbSet<Ent.Address> Addresses { get; set; }
        public DbSet<Ent.Location> Locations { get; set; }
    }
}
