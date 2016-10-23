namespace DBSoft.EVEAPI
{
	using System;
	using System.Collections.Generic;
	using System.Xml.Linq;
	using System.Xml.XPath;
	public class EveApiService
	{
		protected EveApiService(string url)
		{
			URL = baseURL + url;
		}
		public string URL { get; private set; }
		public int Version { get; set; }
		public DateTime CurrentTime { get; set; }
		public DateTime CachedUntil { get; set; }
		public bool Success { get; set; }
		public int ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
		public XElement Result { get; set; }
		public void Load()
		{
			var xml = XDocument.Load(URL);
			var api = xml.Element("eveapi");
			CachedUntil = DateTime.Parse(api.Element("cachedUntil").Value);
			CurrentTime = DateTime.Parse(api.Element("currentTime").Value);
			var error = api.Element("error");
			if ( error != null )
			{
				Success = false;
				ErrorCode = int.Parse(error.Attribute("code").Value);
				ErrorMessage = error.Value;
			}
				else
			{
				Success = true;
				ErrorCode = 0;
				ErrorMessage = "";
				Result = api.Element("result");
			}
		}

		private string baseURL = "http://api.eve-online.com";
	}
}
