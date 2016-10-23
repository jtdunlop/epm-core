namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StationPk : DbMigration
    {
        public override void Up()
        {
            DropIndex("MarketImport2", "IX_MarketImport2_Composite");
            DropPrimaryKey("Station");
            AlterColumn("Asset", "StationID", c => c.Long());
            AlterColumn("MarketImport2", "StationID", c => c.Long(nullable: false));
            AlterColumn("MarketOrder", "StationID", c => c.Long(nullable: false));
            AlterColumn("Station", "ID", c => c.Long(nullable: false));
            AlterColumn("MarketResearch", "StationID", c => c.Long(nullable: false));
            AddPrimaryKey("Station", "ID");
            CreateIndex("MarketImport2", new[] { "TimeStamp", "UserID", "OrderType" }, name: "IX_MarketImport2_Composite");
        }
        
        public override void Down()
        {
            DropIndex("MarketImport2", "IX_MarketImport2_Composite");
            DropPrimaryKey("Station");
            AlterColumn("MarketResearch", "StationID", c => c.Int(nullable: false));
            AlterColumn("Station", "ID", c => c.Int(nullable: false));
            AlterColumn("MarketOrder", "StationID", c => c.Int(nullable: false));
            AlterColumn("MarketImport2", "StationID", c => c.Int(nullable: false));
            AlterColumn("Asset", "StationID", c => c.Int());
            AddPrimaryKey("Station", "ID");
            CreateIndex("MarketImport2", new[] { "TimeStamp", "UserID", "OrderType", "StationID" }, name: "IX_MarketImport2_Composite");
        }
    }
}
