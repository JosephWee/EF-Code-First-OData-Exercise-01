namespace DomainModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImplementConcurrencyChecks : DbMigration
    {
        /// <summary>
        /// For more information on EF6 Concurrency see:
        /// https://www.entityframeworktutorial.net/code-first/TimeStamp-dataannotations-attribute-in-code-first.aspx
        /// https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations
        /// https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.timestampattribute?view=net-5.0
        /// https://docs.microsoft.com/en-us/ef/ef6/saving/concurrency
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.Addresses", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Suburbs", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Cities", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.States", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Countries", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Locations", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.MotorVehicles", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.MotorVehicleModels", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.VehicleMakes", "TimeStamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VehicleMakes", "TimeStamp");
            DropColumn("dbo.MotorVehicleModels", "TimeStamp");
            DropColumn("dbo.MotorVehicles", "TimeStamp");
            DropColumn("dbo.Locations", "TimeStamp");
            DropColumn("dbo.Countries", "TimeStamp");
            DropColumn("dbo.States", "TimeStamp");
            DropColumn("dbo.Cities", "TimeStamp");
            DropColumn("dbo.Suburbs", "TimeStamp");
            DropColumn("dbo.Addresses", "TimeStamp");
        }
    }
}
