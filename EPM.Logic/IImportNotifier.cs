namespace DBSoft.EPM.Logic
{
    public interface IImportNotifier
    {
        void Start(string token, string name);
        void Stop(string token, string name);
    }
}