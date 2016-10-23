namespace DBSoft.EVEAPI.Plumbing
{
	using System;
	using System.Xml.Linq;

	public abstract class EveApiService
	{
		private const string BaseUrl = "http://api.eve-online.com";

		protected EveApiService(string service)
		{
			Service = service;
		}

	    private string Url { get; set; }
		private string _service;

		private string Service
		{
			set
			{
				_service = value;
				Url = BaseUrl + _service;
			}
		}

		private DateTime CurrentTime { get; set; }
		public DateTime CachedUntil { get; private set; }

	    protected bool Success { get; private set; }
		public int ErrorCode { get; set; }
		protected string ErrorMessage { get; private set; }
		protected XElement Result { get; private set; }

		protected void DoLoad()
		{
			var xml = XDocument.Load(Url);
			var api = xml.Element("eveapi");
			if (api == null) return;
			var xElement = api.Element("cachedUntil");
			if (xElement != null) CachedUntil = DateTime.Parse(xElement.Value);
			var element = api.Element("currentTime");
			if (element != null) CurrentTime = DateTime.Parse(element.Value);
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
	}
}