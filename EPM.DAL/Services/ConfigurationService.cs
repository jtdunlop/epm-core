namespace DBSoft.EPM.DAL.Services
{
    using System;
    using System.Collections.Generic;
	using System.Data.Entity;
	using Aspects;
	using CodeFirst.Models;
	using Commands;
	using DbSoft.Cache.Aspect.Attributes;
	using DbSoft.Cache.Aspect.Supporting;
	using DTOs;
	using Enums;
	using Extensions;
	using Interfaces;
	using Queries;
	using System.Globalization;
	using System.Linq;


	public class ConfigurationService : IConfigurationService
	{
	    private readonly IUserService _users;
	    private readonly DbContext _context;

		public ConfigurationService(IDbContextFactory factory, IUserService users)
		{
		    _users = users;
		    _context = factory.CreateContext();
		}

	    [Trace,Cache.Cacheable]
        public ConfigurationSettingsDTO List(string token)
		{
			var userID = _users.GetUserID(token);
			var query = new DataQuery<Configuration>(_context);
			var settings = query
				.Specify(f => f.UserID == userID)
				.GetQuery()
				.ToList();
			var result = new ConfigurationSettingsDTO
			{
				MinimumMarkup = GetSetting<decimal>(settings, ConfigurationType.MinimumMarkup),
				UndercutPercent = GetSetting<decimal>(settings, ConfigurationType.UndercutPercent),
				OvercutPercent = GetSetting<decimal>(settings, ConfigurationType.OvercutPercent),
				PurchaseBrokerFee = GetSetting<decimal>(settings, ConfigurationType.PurchaseBrokerFee),
				SellBrokerFee = GetSetting<decimal>(settings, ConfigurationType.SellBrokerFee),
				SalesTax = GetSetting<decimal>(settings, ConfigurationType.SalesTax),
                MarketSellLocation = GetSetting<int>(settings, ConfigurationType.MarketSellLocation),
                MarketBuyLocation = GetSetting<int>(settings, ConfigurationType.MarketBuyLocation),
				FactoryLocation = GetSetting<int>(settings, ConfigurationType.FactoryLocation),
				InboundContractThreshold = GetSetting<int>(settings, ConfigurationType.InboundContractThreshold),
				BuyOrderRange = GetSetting<int>(settings, ConfigurationType.BuyOrderRange),
                FreightJumpCost = GetSetting<decimal>(settings, ConfigurationType.FreightJumpCost),
                FreightMaxCollateral = GetSetting<decimal>(settings, ConfigurationType.FreightMaxCollateral),
                FreightMaxVolume = GetSetting<decimal>(settings, ConfigurationType.FreightMaxVolume),
                FreightPickupCost = GetSetting<decimal>(settings, ConfigurationType.FreightPickupCost),
                PosLocation = GetNullableInt(GetSetting<int>(settings, ConfigurationType.PosLocation))
			};
            if ( result.MarketBuyLocation == 0 )
            {
                result.MarketBuyLocation = result.FactoryLocation;
            }
			return result;
		}

	    private static int? GetNullableInt(int setting)
	    {
            return setting == 0 ? (int?) null : setting;
	    }

	    [Trace,Cache.Cacheable]
		public T GetSetting<T>(string token, ConfigurationType type) where T : struct
		{
			var userID = _users.GetUserID(token);
			var query = new DataQuery<Configuration>(_context)
				.Specify(f => f.UserID == userID);
			var name = type.ToString();
			var setting = query.Specify(f => f.Name == name).GetQuery().ToList();
			return GetSetting<T>(setting, type);
		}

		private static T GetSetting<T>(IEnumerable<Configuration> settings, ConfigurationType configurationType) where T : struct
		{
			var type = configurationType.ToString();
			var setting = settings.SingleOrDefault(f => f.Name == type) ?? new Configuration { Name = type };
			return ConfigurationExtensions.TryParse<T>(setting.Value);
		}

		[Trace,Cache.TriggerInvalidation(DeleteSettings.Token)]
		public void SaveSettings(string token, ConfigurationSettingsDTO settings)
		{
			using (var cmd = new DataCommand(_context))
			{
				SaveSetting(cmd, token, ConfigurationType.MinimumMarkup, settings.MinimumMarkup.ToString(CultureInfo.InvariantCulture));
				SaveSetting(cmd, token, ConfigurationType.UndercutPercent, settings.UndercutPercent.ToString(CultureInfo.InvariantCulture));
				SaveSetting(cmd, token, ConfigurationType.OvercutPercent, settings.OvercutPercent.ToString(CultureInfo.InvariantCulture));
				SaveSetting(cmd, token, ConfigurationType.PurchaseBrokerFee, settings.PurchaseBrokerFee.ToString(CultureInfo.InvariantCulture));
				SaveSetting(cmd, token, ConfigurationType.SellBrokerFee, settings.SellBrokerFee.ToString(CultureInfo.InvariantCulture));
				SaveSetting(cmd, token, ConfigurationType.SalesTax, settings.SalesTax.ToString(CultureInfo.InvariantCulture));
				SaveSetting(cmd, token, ConfigurationType.FactoryLocation, settings.FactoryLocation.ToString(CultureInfo.InvariantCulture));
				SaveSetting(cmd, token, ConfigurationType.MarketSellLocation, settings.MarketSellLocation.ToString(CultureInfo.InvariantCulture));
                SaveSetting(cmd, token, ConfigurationType.MarketBuyLocation, settings.MarketBuyLocation.ToString(CultureInfo.InvariantCulture));
                SaveSetting(cmd, token, ConfigurationType.InboundContractThreshold, settings.InboundContractThreshold.ToString(CultureInfo.InvariantCulture));
				SaveSetting(cmd, token, ConfigurationType.BuyOrderRange, settings.BuyOrderRange.ToString(CultureInfo.InvariantCulture));
                SaveSetting(cmd, token, ConfigurationType.FreightJumpCost, settings.FreightJumpCost.ToString(CultureInfo.InvariantCulture));
                SaveSetting(cmd, token, ConfigurationType.FreightMaxCollateral, settings.FreightMaxCollateral.ToString(CultureInfo.InvariantCulture));
                SaveSetting(cmd, token, ConfigurationType.FreightMaxVolume, settings.FreightMaxVolume.ToString(CultureInfo.InvariantCulture));
                SaveSetting(cmd, token, ConfigurationType.FreightPickupCost, settings.FreightPickupCost.ToString(CultureInfo.InvariantCulture));
                SaveSetting(cmd, token, ConfigurationType.PosLocation, settings.PosLocation.ToString());
            }
		}

	    private void SaveSetting(DataCommand cmd, string token, ConfigurationType configurationType, string value)
		{
			var userID = _users.GetUserID(token);
			var name = configurationType.ToString();
			var setting = cmd.Get<Configuration>(from => from.Name == name && from.UserID == userID);
			setting.Name = name;
			setting.Value = value;
			setting.UserID = userID;
		}

        public List<int> ListMarketSellLocations()
        {
            var start = DateTime.UtcNow.AddDays(-7);
            return new DataQuery<Configuration>(_context)
                .Specify(f => f.Name == "MarketSellLocation")
                .Specify(f => f.User.LastLogin > start)
                .GetQuery()
                .DistinctBy(f => f.Value)
                .Select(f => int.Parse(f.Value))
                .ToList();
        }
	}
}
