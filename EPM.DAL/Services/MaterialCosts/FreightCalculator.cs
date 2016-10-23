namespace DBSoft.EPM.DAL.Services.MaterialCosts
{
    using System;
    using Enums;
    using Interfaces;

    public class FreightCalculator : IFreightCalculator
    {
        private readonly IConfigurationService _config;
        private readonly IJumpService _jumps;
        private readonly IUniverseService _universe;

        public FreightCalculator(IConfigurationService config, IJumpService jumps, IUniverseService universe)
        {
            _config = config;
            _jumps = jumps;
            _universe = universe;
        }

        public decimal GetFreightCost(string token, decimal price, decimal volume)
        {
            var perJump = _config.GetSetting<decimal>(token, ConfigurationType.FreightJumpCost);
            var pickup = _config.GetSetting<decimal>(token, ConfigurationType.FreightPickupCost);
            var max = _config.GetSetting<decimal>(token, ConfigurationType.FreightMaxVolume);
            var hub = _universe.GetStationSolarSystem(_config.GetSetting<int>(token, ConfigurationType.MarketSellLocation)).SolarSystemID;
            var factory = _universe.GetStationSolarSystem(_config.GetSetting<int>(token, ConfigurationType.FactoryLocation)).SolarSystemID;
            var collateral = _config.GetSetting<decimal>(token, ConfigurationType.FreightMaxCollateral);
            if (max == 0 || volume == 0) return 0;
            var jumps = _jumps.GetJumps(hub, factory);
            if (price == 0)
                return (pickup + perJump * jumps) / (max / volume);
            return (pickup + perJump * jumps) / Math.Min(max / volume, collateral / price);
        }

    }
}