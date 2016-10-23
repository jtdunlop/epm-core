namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ManufacturingCostPrecision : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SolarSystem", "ManufacturingCost", c => c.Decimal(nullable: false, precision: 16, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SolarSystem", "ManufacturingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
