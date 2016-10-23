namespace DBSoft.EPM.Logic.RefreshApi
{
    using DAL.Interfaces;
    using System;

    public class EveApiMapper
	{
	    private readonly IEveApiStatusService _service;

	    protected EveApiMapper(IEveApiStatusService service)
        {
            _service = service;
        }

	    protected void UpdateStatus(string apiName, DateTime cachedUntil, string token)
		{
			_service.UpdateStatus(token, apiName, "OK", cachedUntil.ToLocalTime());
		}

		protected void SaveError(string apiName, string token, string result )
		{
			_service.UpdateStatus(token, apiName, result, null);
		}
	}
}
