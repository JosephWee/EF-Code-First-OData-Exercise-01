namespace DomainModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Guid(nullable: false),
                        AddressLine1 = c.String(nullable: false, maxLength: 50),
                        AddressLine2 = c.String(maxLength: 50),
                        SuburbId = c.Guid(nullable: false),
                        PostalCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Suburbs", t => t.SuburbId, cascadeDelete: true)
                .Index(t => t.SuburbId);
            
            CreateTable(
                "dbo.Suburbs",
                c => new
                    {
                        SuburbId = c.Guid(nullable: false),
                        CityId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.SuburbId)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Guid(nullable: false),
                        StateId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateId = c.Guid(nullable: false),
                        CountryId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        AddressId = c.Guid(nullable: false),
                        ParkingCapacity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.Name)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.MotorVehicles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MotorVehicleModelId = c.Guid(nullable: false),
                        Year = c.Int(nullable: false),
                        VIN = c.String(nullable: false, maxLength: 17),
                        Registration = c.String(maxLength: 8),
                        Mileage = c.Int(nullable: false),
                        HasNavSystem = c.Boolean(nullable: false),
                        HasDashCam = c.Boolean(nullable: false),
                        HasReversingCam = c.Boolean(nullable: false),
                        HasForwardParkingSensor = c.Boolean(nullable: false),
                        HasRearParkingSensor = c.Boolean(nullable: false),
                        HasBlindspotMonitoring = c.Boolean(nullable: false),
                        HasAutomaticEmergencyBrake = c.Boolean(nullable: false),
                        PickupCargoAccessoryType = c.Int(nullable: false),
                        LocationId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.MotorVehicleModels", t => t.MotorVehicleModelId, cascadeDelete: true)
                .Index(t => t.MotorVehicleModelId)
                .Index(t => t.Year)
                .Index(t => t.VIN)
                .Index(t => t.Registration)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.MotorVehicleModels",
                c => new
                    {
                        MotorVehicleModelId = c.Guid(nullable: false),
                        VehicleMakeId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        MotorVehicleType = c.Int(nullable: false),
                        FuelCapacity = c.Double(nullable: false),
                        FuelConsumption = c.Double(nullable: false),
                        EngineType = c.Int(nullable: false),
                        MaxPower = c.Int(nullable: false),
                        TransmissionType = c.Int(nullable: false),
                        AWD = c.Boolean(nullable: false),
                        CargoVolume = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MotorVehicleModelId)
                .ForeignKey("dbo.VehicleMakes", t => t.VehicleMakeId, cascadeDelete: true)
                .Index(t => t.VehicleMakeId)
                .Index(t => t.EngineType)
                .Index(t => t.MaxPower)
                .Index(t => t.TransmissionType);
            
            CreateTable(
                "dbo.VehicleMakes",
                c => new
                    {
                        VehicleMakeId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.VehicleMakeId)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MotorVehicleModels", "VehicleMakeId", "dbo.VehicleMakes");
            DropForeignKey("dbo.MotorVehicles", "MotorVehicleModelId", "dbo.MotorVehicleModels");
            DropForeignKey("dbo.MotorVehicles", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Suburbs", "CityId", "dbo.Cities");
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Cities", "StateId", "dbo.States");
            DropForeignKey("dbo.Addresses", "SuburbId", "dbo.Suburbs");
            DropIndex("dbo.VehicleMakes", new[] { "Name" });
            DropIndex("dbo.MotorVehicleModels", new[] { "TransmissionType" });
            DropIndex("dbo.MotorVehicleModels", new[] { "MaxPower" });
            DropIndex("dbo.MotorVehicleModels", new[] { "EngineType" });
            DropIndex("dbo.MotorVehicleModels", new[] { "VehicleMakeId" });
            DropIndex("dbo.MotorVehicles", new[] { "LocationId" });
            DropIndex("dbo.MotorVehicles", new[] { "Registration" });
            DropIndex("dbo.MotorVehicles", new[] { "VIN" });
            DropIndex("dbo.MotorVehicles", new[] { "Year" });
            DropIndex("dbo.MotorVehicles", new[] { "MotorVehicleModelId" });
            DropIndex("dbo.Locations", new[] { "AddressId" });
            DropIndex("dbo.Locations", new[] { "Name" });
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.Cities", new[] { "StateId" });
            DropIndex("dbo.Suburbs", new[] { "CityId" });
            DropIndex("dbo.Addresses", new[] { "SuburbId" });
            DropTable("dbo.VehicleMakes");
            DropTable("dbo.MotorVehicleModels");
            DropTable("dbo.MotorVehicles");
            DropTable("dbo.Locations");
            DropTable("dbo.Countries");
            DropTable("dbo.States");
            DropTable("dbo.Cities");
            DropTable("dbo.Suburbs");
            DropTable("dbo.Addresses");
        }
    }
}
