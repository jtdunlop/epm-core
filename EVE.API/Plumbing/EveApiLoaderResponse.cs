using System;
using System.Xml.Linq;

namespace DBSoft.EVEAPI.Plumbing
{
	public class EveApiLoaderResponse
	{
		public bool Success { get; set; }
		public int ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
		public DateTime CachedUntil { get; set; }
		public DateTime CurrentTime { get; set; }
		public XElement Result { get; set; }
	}
}