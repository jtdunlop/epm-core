namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class SolarSystem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Station", "Tax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SolarSystem", "ManufacturingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SolarSystem", "InstallationTax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Station", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Station", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.SolarSystem", "InstallationTax");
            DropColumn("dbo.SolarSystem", "ManufacturingCost");
            DropColumn("dbo.Station", "Tax");
        }
    }
}
