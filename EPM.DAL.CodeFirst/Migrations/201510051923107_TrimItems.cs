namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class TrimItems : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Item", "MinimumStock");
            DropColumn("dbo.Item", "BounceFactor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Item", "BounceFactor", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Item", "MinimumStock", c => c.Int());
        }
    }
}
