namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using EVE.SDE;
    using Operations;

    public partial class MosaicBlueprints : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Blueprint", "WasteFactor");
            DropColumn("dbo.Blueprint", "ProductivityModifier");
            MigrateBlueprints(MigrationStep.Up);
        }

        public override void Down()
        {
            AddColumn("dbo.Blueprint", "ProductivityModifier", c => c.Int());
            AddColumn("dbo.Blueprint", "WasteFactor", c => c.Int(nullable: false));
        }

        private void MigrateBlueprints(MigrationStep step)
        {
            var provider = new YamlProvider(Settings.Blueprints);
            var service = new BlueprintService(provider);
            foreach (var blueprint in service.GetManifests()
                .Select(manifest => new BlueprintDto
                {
                    BuildItemID = manifest.ItemID,
                    ItemID = manifest.BlueprintID,
                    ProductionTime = manifest.ProductionTime
                }))
            {
                this.StaticBlueprint(blueprint, step);
            }
        }
    }
}
