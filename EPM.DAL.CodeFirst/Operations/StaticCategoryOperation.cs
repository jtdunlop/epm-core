namespace DBSoft.EPM.DAL.CodeFirst.Operations
{
    using System.Data.Entity.Migrations.Model;
    using Models;

    public class StaticCategoryOperation : MigrationOperation
    {
        public Category Category { get; set; }
        public MigrationStep Step { get; set; }

        public StaticCategoryOperation(Category category, MigrationStep step)
            : base(null)
        {
            Category = category;
            Step = step;
        }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}