namespace DBSoft.EPM.DAL.CodeFirst.Operations
{
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.SqlServer;

    public class EPMMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(MigrationOperation migrationOperation)
        {
            dynamic operation = migrationOperation;
            DoGenerate(operation);
        }

        private void DoGenerate(StaticBlueprintOperation operation)
        {
            var blueprint = operation.BlueprintDto;
            using (var writer = Writer())
            {
                switch (operation.Step)
                {
                    case MigrationStep.Up:
                        writer.WriteLine("IF EXISTS (SELECT * FROM Item WHERE ID = {0}) AND EXISTS (SELECT * FROM Item WHERE ID = {2}) BEGIN " +
                            "IF EXISTS (SELECT * FROM Blueprint WHERE BuildItemID = {0}) " +
                            "UPDATE Blueprint SET ProductionTime = {1}, ItemID = {2}, BuildItemID = {0} WHERE BuildItemID = {0} " +
                            "ELSE INSERT INTO Blueprint (BuildItemID, ProductionTime, ItemID) VALUES ({0},{1}, {2}) END",
                            blueprint.BuildItemID, blueprint.ProductionTime, blueprint.ItemID);
                        break;
                    case MigrationStep.Down:
                        break;
                }
                Statement(writer);
            }
        }

        private void DoGenerate(StaticItemOperation operation)
        {
            var item = operation.Item;
            using (var writer = Writer())
            {
                switch (operation.Step)
                {
                    case MigrationStep.Up:
                        writer.WriteLine(@"
                            IF EXISTS (SELECT * FROM Item WHERE ID = {0}) AND EXISTS (SELECT * FROM [Group] WHERE ID = {2}) 
                                UPDATE Item SET Name = '{1}', GroupID = {2}, QuantityMultiplier = {3}, Volume = {4} WHERE ID = {0} 
                            ELSE IF EXISTS (SELECT * FROM [Group] WHERE ID = {2}) 
                                INSERT INTO Item (ID, Name, GroupID, QuantityMultiplier, Volume) VALUES ({0},'{1}', {2}, {3}, {4})",
                            item.ID, item.Name.Replace("'", "''"), item.GroupID, item.QuantityMultiplier, item.Volume);
                        break;
                }
                Statement(writer);
            }
        }

        private void DoGenerate(StaticGroupOperation operation)
        {
            var group = operation.Group;
            using (var writer = Writer())
            {
                switch (operation.Step)
                {
                    case MigrationStep.Up:
                        writer.WriteLine("IF EXISTS (SELECT * FROM [Group] WHERE ID = {0}) UPDATE [Group] SET Name = '{1}', CategoryID = {2} WHERE ID = {0} " +
                            "ELSE INSERT INTO [Group] (ID, Name, CategoryID) VALUES ({0},'{1}', {2})",
                            group.ID, group.Name.Replace("'", "''"), group.CategoryID);
                        break;
                }
                Statement(writer);
            }
        }

        private void DoGenerate(StaticCategoryOperation operation)
        {
            var category = operation.Category;
            using (var writer = Writer())
            {
                switch (operation.Step)
                {
                    case MigrationStep.Up:
                        writer.WriteLine("IF EXISTS (SELECT * FROM Category WHERE ID = {0}) UPDATE Category SET Name = '{1}' WHERE ID = {0} " +
                            "ELSE INSERT INTO Category (ID, Name) VALUES ({0},'{1}')",
                            category.ID, category.Name.Replace("'", "''"));
                        break;
                    case MigrationStep.Down:
                        break;
                }
                Statement(writer);
            }
        }

        private void DoGenerate(StaticManifestOperation operation)
        {
            using (var writer = Writer())
            {
                switch (operation.Step)
                {
                    case MigrationStep.Up:
                        writer.WriteLine("DELETE FROM Manifest WHERE ItemID = {0}", operation.Manifest.ItemID);
                        writer.WriteLine("IF NOT EXISTS (SELECT * FROM Item WHERE ID = {0})" +
                            "INSERT INTO Item (ID, Name, GroupID, QuantityMultiplier) VALUES ({0}, 'Unpublished', 0, {1})",
                            operation.Manifest.ItemID, operation.Manifest.Quantity);
                        writer.WriteLine("UPDATE ITEM SET QuantityMultiplier = {0} WHERE ID = {1} AND QuantityMultiplier <> {0}",
                            operation.Manifest.Quantity, operation.Manifest.ItemID);
                        foreach (var material in operation.Manifest.Materials)
                        {
                            writer.WriteLine("INSERT INTO Manifest (ItemID, MaterialItemID, Quantity) VALUES ({0}, {1}, {2})",
                                operation.Manifest.ItemID, material.ItemID, material.Quantity);
                        }
                        break;
                }
                Statement(writer);
            }
        }
    }
}