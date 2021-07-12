namespace DomainModel.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Text;
    using Ent = DomainModel.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<DomainModel.VehicleRentalContext>
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DomainModel.VehicleRentalContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //Create FK that is set to null when parent row is deleted:
            //ChildTable, ParentTable, FK, ParentPK
            var list_FK_SET_NULL = new List<Tuple<string, string, string, string>>()
            {
                new Tuple<string, string, string, string>(
                    "dbo.MotorVehicles", "dbo.Locations", "LocationId", "LocationId")
            };

            foreach (var tup in list_FK_SET_NULL)
            {
                string FKName =
                    string.Format(
                        "FK_{0}_{1}_{2}",
                        tup.Item1,
                        tup.Item2,
                        tup.Item3
                    );

                string sqlDropFK =
                    string.Format(
                            @"IF EXISTS (SELECT 1 FROM SYS.FOREIGN_KEYS WHERE object_id = OBJECT_ID(N'[{0}]') AND parent_object_id = OBJECT_ID(N'{1}'))
                                ALTER TABLE {1} DROP CONSTRAINT [{0}];",
                            FKName,
                            tup.Item1
                        );

                string sqlCreateFK =
                    string.Format(
                            "ALTER TABLE {1} ADD CONSTRAINT [{0}] FOREIGN KEY ({3}) REFERENCES {2} ({4}) ON UPDATE NO ACTION ON DELETE SET NULL;",
                            FKName,
                            tup.Item1,
                            tup.Item2,
                            tup.Item3,
                            tup.Item4
                        );

                log.Debug("Create FK that is set to null when parent row is deleted\r\n");
                log.Debug(string.Format("--Foregin Key Name\r\n", FKName));
                log.Debug(string.Format("--Drop Existing FK:\r\n", sqlDropFK));
                log.Debug(string.Format("--Create FK:\r\n", sqlCreateFK));

                context.Database.ExecuteSqlCommand(sqlDropFK);
                context.Database.ExecuteSqlCommand(sqlCreateFK);
            }

            //Create UNIQUE CLUSTERED MULTI-COLUMN INDEX on the following:
            //TableName, Coulmn1Name, Collumn2Name
            var list_UNIQUE_CONSTRAINT_CLUSTERED = new List<Tuple<string, string, string>>()
            {
                new Tuple<string, string, string>(
                    "dbo.Addresses", "SuburbId", "AddressLine1"),
                new Tuple<string, string, string>(
                    "dbo.Suburbs", "CityId", "Name"),
                new Tuple<string, string, string>(
                    "dbo.Cities", "StateId", "Name"),
                new Tuple<string, string, string>(
                    "dbo.States", "CountryId", "Name"),
                new Tuple<string, string, string>(
                    "dbo.MotorVehicleModels", "VehicleMakeId", "Name")
            };

            foreach (var tup in list_UNIQUE_CONSTRAINT_CLUSTERED)
            {
                string ConstraintName =
                    string.Format(
                        "IX_UNIQUE_{0}_{1}_{2}",
                        tup.Item1,
                        tup.Item2,
                        tup.Item3
                    );

                string sqlDropConstraint =
                    string.Format(
                            @"IF EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = N'{0}')
                                ALTER TABLE {1} DROP CONSTRAINT [{0}];",
                            ConstraintName,
                            tup.Item1
                        );

                string sqlCreateConstraint =
                    string.Format(
                            "ALTER TABLE {1} ADD CONSTRAINT [{0}] UNIQUE ({2}, {3});",
                            ConstraintName,
                            tup.Item1,
                            tup.Item2,
                            tup.Item3
                        );

                log.Debug("Create FK that is set to null when parent row is deleted\r\n");
                log.Debug(string.Format("--Unique Constraint Name\r\n", ConstraintName));
                log.Debug(string.Format("--Drop Existing Unique Constraint:\r\n", sqlDropConstraint));
                log.Debug(string.Format("--Create Unique Constraint:\r\n", sqlCreateConstraint));

                context.Database.ExecuteSqlCommand(sqlDropConstraint);
                context.Database.ExecuteSqlCommand(sqlCreateConstraint);
            }
        }
    }
}