namespace DBSoft.EPM.DAL.CodeFirst.Operations
{
	using System.Data.Entity.Migrations.Model;
	using Models;

	public class StaticGroupOperation : MigrationOperation
	{
		public Group Group { get; set; }
		public MigrationStep Step { get; set; }

		public StaticGroupOperation(Group @group, MigrationStep step) : base(null)
		{
			Group = @group;
			Step = step;
		}

		public override bool IsDestructiveChange
		{
			get { return false; }
		}
	}
}
