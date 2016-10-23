namespace DBSoft.EPM.Logic
{
    public interface IConfigurationProcessor
    {
        bool ConfigurationIsValid(string token);
        bool AccountIsAvailable(string token);
    }
}