namespace DBSoft.EPM.DAL.Services.ItemCosts
{
    using Enums;
    using Interfaces;

    public class ItemCostDTO
	{
		private readonly IConfigurationService _config;
		private readonly string _token;

		public ItemCostDTO(IConfigurationService config, string token = null)
		{
			_config = config;
			_token = token;
		}
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public decimal Cost
		{
			get
			{
                return (BaseCost * (1M - .01M * MaterialEfficiency)) * (1 + PurchaseBrokerPercentage) + AdditionalCost + FreightCost;
			}
		}
		public decimal BaseCost { get; set; }
		public decimal AdditionalCost { get; set; }
		public int MaterialEfficiency { get; set; }

		private decimal PurchaseBrokerPercentage
		{
			get
			{
				return _token == null ? .0075m : _config.GetSetting<decimal>(_token, ConfigurationType.PurchaseBrokerFee) / 100;
			}
		}

        public decimal FreightCost { get; set; }
	}
}
