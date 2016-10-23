namespace DBSoft.EPM.DAL.CodeFirst.Operations
{
    using System.Data.Entity.Migrations.Model;
    using Models;

    public class StaticPublishedItemOperation : MigrationOperation
    {
        public Item Item { get; set; }
        public MigrationStep Step { get; set; }

        public StaticPublishedItemOperation(Item item, MigrationStep step) : base(null)
        {
            Item = item;
            Step = step;
        }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}