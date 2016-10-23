namespace DBSoft.EPM.DAL.Services.ItemCosts
{
    using System;
    using System.Diagnostics;

    public class ListBuildableRequest
	{
		public string Token { get; set; }

		[DebuggerStepThrough]
		public ListBuildableRequest()
		{
			Date = DateTime.Today;
		}
		private DateTime? _date;

		public DateTime? Date
		{
			get 
			{ 
				return _date; 
			}
			set 
			{
				_date = value.HasValue ? value.Value.StartOfTheDay() : (DateTime?)null;
			}
		}
	}
}