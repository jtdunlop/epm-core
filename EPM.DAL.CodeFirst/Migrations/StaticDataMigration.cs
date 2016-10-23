namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using EVE.SDE;
    using Models;
    using Operations;

    public class StaticDataMigration : DbMigration
    {
        public override void Up()
        {
            MigrateCategories(MigrationStep.Up);
            MigrateGroups(MigrationStep.Up);
            MigrateItems(MigrationStep.Up);
            MigrateBlueprints(MigrationStep.Up);
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

        private void MigrateItems(MigrationStep step)
        {
            var provider = new YamlProvider(Settings.Items);
            var service = new ItemService(provider);

            foreach (var item in service.GetItems())
            {
                this.StaticItem(new Item
                {
                    ID = item.ItemID,
                    Name = item.ItemName,
                    GroupID = item.GroupID,
                    QuantityMultiplier = item.PortionSize,
                    Volume = item.Volume
                }, step);
            }
        }

        private void MigrateGroups(MigrationStep step)
        {
            var service = new GroupService(new YamlProvider(Settings.Groups));
            foreach (var group in service.GetGroups())
            {
                this.StaticGroup(new Group
                {
                    ID = group.GroupID,
                    CategoryID = group.CategoryID,
                    Name = group.GroupName
                }, step);
            }
        }

        private void MigrateCategories(MigrationStep step)
        {
            var service = new CategoryService(new YamlProvider(Settings.Categories));
            foreach (var category in service.GetCategories())
            {
                this.StaticCategory(new Category
                {
                    ID = category.CategoryID,
                    Name = category.CategoryName
                }, step);
            }
        }
    }
}