namespace DBSoft.EPM.DAL.CodeFirst.Operations
{
	using System.Data.Entity.Migrations.Model;

	public enum MigrationStep
	{
		Up,
		Down
	}
	public class StaticBlueprintOperation : MigrationOperation
	{
		public MigrationStep Step { get; private set; }
		public BlueprintDto BlueprintDto { get; private set; }

		public StaticBlueprintOperation(BlueprintDto blueprint, MigrationStep step)
			: base(null)
		{
			BlueprintDto = blueprint;
			Step = step;
		}

		public override bool IsDestructiveChange
		{
			get { return false; }
		}
	}
}
