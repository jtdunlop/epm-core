namespace DBSoft.EPM.Logic
{
	public interface IThreadCallback
	{
		void Begin();
		void End();
		void SetText(string text);
		void SetProgress(int progress);
	}
}
