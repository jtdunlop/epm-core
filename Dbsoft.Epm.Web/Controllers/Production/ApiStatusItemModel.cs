namespace DBSoft.EPMWeb.Models.Production
{
	using System;

	public class ApiStatusItemModel
	{
		public string ApiName { get; set; }
		public DateTime Expiry { get; set; }
		public string Result { get; set; }
	}
}