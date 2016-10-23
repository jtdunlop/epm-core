namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class MissingIndexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Account", "UserID");
            CreateIndex("dbo.AccountBalance", "AccountID");
            CreateIndex("dbo.Asset", "AccountID");
            CreateIndex("dbo.Asset", "ItemID");
            CreateIndex("dbo.Asset", "UserID");
            CreateIndex("dbo.Blueprint", "ItemID");
            CreateIndex("dbo.Blueprint", "BuildItemID");
            CreateIndex("dbo.BlueprintInstance", "BlueprintID");
            CreateIndex("dbo.Character", "AccountID");
            CreateIndex("dbo.Configuration", "UserID");
            CreateIndex("dbo.EveApiStatus", "UserID");
            CreateIndex("dbo.Group", "CategoryID");
            CreateIndex("dbo.Item", "GroupID");
            CreateIndex("dbo.ItemExtension", "ItemID");
            CreateIndex("dbo.ItemExtension", "UserID");
            CreateIndex("dbo.Manifest", "ItemID");
            CreateIndex("dbo.Manifest", "MaterialItemID");
            CreateIndex("dbo.MarketImport2", "ItemID");
            CreateIndex("dbo.MarketImport2", "UserID");
            CreateIndex("dbo.MarketOrder", "ItemID");
            CreateIndex("dbo.MarketOrder", "UserID");
            CreateIndex("dbo.ProductionJob", "AssetID");
            CreateIndex("dbo.ProductionJob", "ItemID");
            CreateIndex("dbo.SolarSystem", "RegionID");
            CreateIndex("dbo.Station", "SolarSystemID");
        }

        public override void Down()
        {
            DropIndex("dbo.Station", new[] { "SolarSystemID" });
            DropIndex("dbo.SolarSystem", new[] { "RegionID" });
            DropIndex("dbo.ProductionJob", new[] { "ItemID" });
            DropIndex("dbo.ProductionJob", new[] { "AssetID" });
            DropIndex("dbo.MarketOrder", new[] { "ItemID" });
            DropIndex("dbo.MarketOrder", new[] { "UserID" });
            DropIndex("dbo.MarketImport2", new[] { "UserID" });
            DropIndex("dbo.MarketImport2", new[] { "ItemID" });
            DropIndex("dbo.Manifest", new[] { "MaterialItemID" });
            DropIndex("dbo.Manifest", new[] { "ItemID" });
            DropIndex("dbo.ItemExtension", new[] { "ItemID" });
            DropIndex("dbo.ItemExtension", new[] { "UserID" });
            DropIndex("dbo.Item", new[] { "GroupID" });
            DropIndex("dbo.Group", new[] { "CategoryID" });
            DropIndex("dbo.EveApiStatus", new[] { "UserID" });
            DropIndex("dbo.Configuration", new[] { "UserID" });
            DropIndex("dbo.Character", new[] { "AccountID" });
            DropIndex("dbo.BlueprintInstance", new[] { "BlueprintID" });
            DropIndex("dbo.Blueprint", new[] { "ItemID" });
            DropIndex("dbo.Blueprint", new[] { "BuildItemID" });
            DropIndex("dbo.Asset", new[] { "UserID" });
            DropIndex("dbo.Asset", new[] { "ItemID" });
            DropIndex("dbo.Asset", new[] { "AccountID" });
            DropIndex("dbo.AccountBalance", new[] { "AccountID" });
            DropIndex("dbo.Account", new[] { "UserID" });
        }
    }
}
