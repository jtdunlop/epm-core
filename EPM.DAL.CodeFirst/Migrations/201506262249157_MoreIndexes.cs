namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class MoreIndexes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MarketPrice",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        AdjustedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.ItemID);
            
            AlterColumn("dbo.EveAccount", "EveCharacterName", c => c.String(maxLength: 50));
            CreateIndex("dbo.ProductionJob", "Status");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MarketPrice", "ItemID", "dbo.Item");
            DropIndex("dbo.MarketPrice", new[] { "ItemID" });
            DropIndex("dbo.ProductionJob", new[] { "Status" });
            AlterColumn("dbo.EveAccount", "EveCharacterName", c => c.String());
            DropTable("dbo.MarketPrice");
        }
    }
}
