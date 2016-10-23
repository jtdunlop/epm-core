namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FreightCosts : StaticDataMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "Volume", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            base.Up();
        }
        
        public override void Down()
        {
            base.Down();
            DropColumn("dbo.Item", "Volume");
        }
    }
}
