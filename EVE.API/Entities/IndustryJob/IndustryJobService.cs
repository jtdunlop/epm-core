namespace DBSoft.EVEAPI.Entities.IndustryJob
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Xml.Linq;
	using Account;
	using Plumbing;
	using WalletTransaction;

    public class IndustryJobService : IIndustryJobService
    {
        private readonly IEveApiLoader _loader;

        private static DateTime? GetTime(XAttribute element)
		{
			var val = (DateTime)element;
			if ( val == new DateTime(1, 1, 1) )
			{
				return null;
			}
			return val;
		}

        public IndustryJobService(IEveApiLoader loader)
        {
            _loader = loader;
        }

        public async Task<ApiLoadResponse<IndustryJob>> Load(ApiKeyType keyType, int apiKeyId, string vCode, int eveApiId)
		{
            var kt = keyType == ApiKeyType.Character ? "char" : "corp";
            var url =
                $"http://api.eve-online.com/{kt}/IndustryJobsHistory.xml.aspx?keyID={apiKeyId}&vCode={vCode}&characterID={eveApiId}";
            var response = await _loader.Load(url);
            if (!response.Success) throw new Exception(response.ErrorMessage);
            if (!response.Success)
			{
				throw new Exception(response.ErrorMessage);
			}
			var xElement = response.Result.Element("rowset");
            var result = new ApiLoadResponse<IndustryJob>
            {
                CachedUntil = response.CachedUntil
            };
            if (xElement == null) return result;
            foreach (var job in xElement.Elements("row").Select(f => new IndustryJob
            {
                ID = int.Parse(f.Attribute("jobID").Value),
                InstalledItemID = long.Parse(f.Attribute("blueprintID").Value),
                InstallerID = int.Parse(f.Attribute("installerID").Value),
                Runs = int.Parse(f.Attribute("runs").Value),
                InstalledItemTypeID = int.Parse(f.Attribute("productTypeID").Value),
                LicensedRuns = int.Parse(f.Attribute("licensedRuns").Value),
                OutputTypeID = int.Parse(f.Attribute("productTypeID").Value),
                Completed = GetCompleted(f),
                ActivityID = int.Parse(f.Attribute("activityID").Value),
                BeginProductionTime = GetTime(f.Attribute("startDate")),
                EndProductionTime = GetTime(f.Attribute("endDate")),
                PauseProductionTime = GetTime(f.Attribute("pauseDate"))
            }))
            {
                result.Data.Add(job);
            }
            return result;
		}

	    private static bool GetCompleted(XElement xElement)
	    {
            return GetCompletedStatus(xElement) != IndustryJobStatus.InProgress;
	    }

	    private static IndustryJobStatus GetCompletedStatus(XElement element)
	    {
            var status = int.Parse(element.Attribute("status").Value);
            switch ( status )
            {
                case 1:
                    return IndustryJobStatus.InProgress;
                default: 
                    return IndustryJobStatus.Delivered;
            }
        }
	}
}
