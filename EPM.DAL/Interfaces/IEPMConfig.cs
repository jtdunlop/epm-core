namespace DBSoft.EPM.DAL.Interfaces
{
    public interface IEpmConfig
    {
        T GetSetting<T>(string key) where T : struct;
        string GetSetting(string key);
        string GetConnectionString(string key);
    }
}