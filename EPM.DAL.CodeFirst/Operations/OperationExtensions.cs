namespace DBSoft.EPM.DAL.CodeFirst.Operations
{
	using System.Data.Entity.Migrations;
	using System.Data.Entity.Migrations.Infrastructure;
	using EVE.SDE;
	using Models;

	public static class OperationExtensions
	{
		public static void StaticBlueprint(this DbMigration migration, BlueprintDto blueprint, MigrationStep step)
		{
			((IDbMigration)migration).AddOperation(new StaticBlueprintOperation(blueprint, step));
		}

        public static void StaticItem(this DbMigration migration, Item item, MigrationStep step)
		{
			((IDbMigration)migration).AddOperation(new StaticItemOperation(item, step));
		}

        
        public static void StaticGroup(this DbMigration migration, Group group, MigrationStep step)
		{
			((IDbMigration)migration).AddOperation(new StaticGroupOperation(group, step));
		}

        public static void StaticCategory(this DbMigration migration, Category category, MigrationStep step)
        {
            ((IDbMigration)migration).AddOperation(new StaticCategoryOperation(category, step));
        }

        public static void StaticManifest(this DbMigration migration, ManifestDTO manifest, MigrationStep step)
        {
            ((IDbMigration)migration).AddOperation(new StaticManifestOperation(manifest, step));
        }
	}
}