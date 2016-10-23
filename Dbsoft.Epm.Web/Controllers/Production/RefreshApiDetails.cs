namespace DBSoft.EPMWeb.Models.Production
{
	public class RefreshApiDetails : IRefreshDetails
	{
		public string Div { get { return "apistatus"; } }
		public string Action { get { return "ApiStatus"; } }
		public string Title { get { return "Refresh API"; } }
		public string TitleDiv { get { return "api-box"; } }
	}
}