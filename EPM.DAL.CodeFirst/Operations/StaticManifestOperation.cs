namespace DBSoft.EPM.DAL.CodeFirst.Operations
{
    using System.Data.Entity.Migrations.Model;
    using EVE.SDE;
    using Migrations;

    public class StaticManifestOperation : MigrationOperation
    {
        public MigrationStep Step { get; set; }
        public ManifestDTO Manifest { get; set; }

        public StaticManifestOperation(ManifestDTO manifest, MigrationStep step)
            : base(null)
        {
            Manifest = manifest;
            Step = step;
        }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}