namespace DBSoft.EPM.DAL.Requests
{
	public class MaterialPurchaseRequest
	{
		public string Token {get;set;}

		/// <summary>
		/// True: Only include materials with active orders
		/// False: Only include materials without active orders
		/// Null: Include all materials irrespective of active orders
		/// </summary>
		public bool? IncludeMaterialsWithActiveOrders { get; set; }
	}
}