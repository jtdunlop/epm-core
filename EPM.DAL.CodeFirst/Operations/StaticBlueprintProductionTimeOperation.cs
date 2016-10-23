namespace DBSoft.EPM.DAL.CodeFirst.Operations
{
    using System.Data.Entity.Migrations.Model;

    public class StaticBlueprintProductionTimeOperation : MigrationOperation
    {
        public BlueprintProductionTime BlueprintProductionTime { get; set; }
        public MigrationStep Step { get; set; }

        public StaticBlueprintProductionTimeOperation(BlueprintProductionTime time, MigrationStep step)
            : base(null)
        {
            BlueprintProductionTime = time;
            Step = step;
        }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}