namespace DBSoft.EPM.DAL.CodeFirst.Operations
{
    using System.Data.Entity.Migrations.Model;
    using Migrations;
    using Models;

    public class StaticGateOperation : MigrationOperation
    {
        public MigrationStep Step { get; set; }
        public Gate Gate { get; set; }

        public StaticGateOperation(Gate gate, MigrationStep step)
            : base(null)
        {
            Gate = gate;
            Step = step;
        }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}