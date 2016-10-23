namespace DBSoft.EPM.DAL.Services
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Aspects;
    using AutoMapper;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DbSoft.Cache.Aspect.Supporting;
    using DTOs;
    using EVEAPI.Crest.MarketOrder;
    using Interfaces;
    using Queries;
    using SolarSystemDTO = DTOs.SolarSystemDTO;
    using Region = CodeFirst.Models.Region;

    public class UniverseService : IUniverseService
    {
        private readonly IDbContextFactory _factory;

        public UniverseService(IDbContextFactory factory)
		{
            _factory = factory;
		}

        [Trace,Cache.Cacheable]
        public async Task<IEnumerable<RegionDTO>> ListRegions()
        {
            using ( var context = _factory.CreateContext())
            {
                return await new DataQuery<Region>(context).GetQuery().Select(f => new RegionDTO
                {
                    RegionID = f.ID,
                    RegionName = f.Name
                }).ToListAsync();
            }
        }

        [Trace,Cache.Cacheable]
        public IEnumerable<StationDTO> ListStations()
        {
            using (var context = _factory.CreateContext())
            {
                return context.Set<Station>().Select(Mapper.Map<StationDTO>).ToList();
            }
        }

        public void AddCitadel(Citadel citadel)
        {
            using (var context = _factory.CreateContext())
            {
                var existing = context.Set<Station>().SingleOrDefault(f => f.ID == citadel.Id);
                if ( existing == null )
                {
                    var split = citadel.Name.Split("-".ToCharArray());
                    if ( split[0].StartsWith("Niarja"))
                    {
                        split[0] = "Niarja";
                    }
                    var system = GetSolarSystem(split[0].Trim());
                    var add = new Station
                    {
                        Name = split[1].Trim(),
                        ID = citadel.Id,
                        SolarSystemID = system.SolarSystemID
                    };
                    context.Stations.Add(add);
                    context.SaveChanges();
                }
            }
        }

        [Trace,Cache.Cacheable(ttl: 3600 * 23)]
        public List<SolarSystemDTO> ListSolarSystems()
        {
            using ( var context = _factory.CreateContext() )
            {
                return context.Set<SolarSystem>().Select(Mapper.Map<SolarSystemDTO>).ToList();
            }
        }

        [Trace, Cache.Cacheable(ttl: 3600 * 23)]
        public SolarSystemDTO GetSolarSystem(long id)
        {
            using (var context = _factory.CreateContext())
            {
                return Mapper.Map<SolarSystemDTO>(context.Set<SolarSystem>().Single(f => f.ID == id));
            }
        }

        [Trace, Cache.Cacheable(ttl: 3600 * 23)]
        public SolarSystemDTO GetSolarSystem(string name)
        {
            using (var context = _factory.CreateContext())
            {
                return Mapper.Map<SolarSystemDTO>(context.Set<SolarSystem>().Single(f => f.Name == name));
            }
        }

        [Trace, Cache.Cacheable]
        public StationDTO GetStation(long stationId)
        {
            using (var context = _factory.CreateContext())
            {
                return Mapper.Map<StationDTO>(context.Set<Station>().Single(f => f.ID == stationId));
            }
        }

        [Trace,Cache.TriggerInvalidation(DeleteSettings.All)]
        private static void DeleteCache()
        { }

        [Trace,Cache.Cacheable]
        public SolarSystemDTO GetStationSolarSystem(long stationId)
        {
            var systemID = GetStation(stationId).SolarSystemID;
            var result = GetSolarSystem(systemID);
            return result;
        }

        [Trace,Cache.Cacheable(ttl: 3600 * 23)]
        public List<MarketAdjustedPriceDTO> GetAdjustedMarketPrices()
        {
            using (var context = _factory.CreateContext())
            {
                return context.Set<MarketPrice>().ToList().Select(Mapper.Map<MarketAdjustedPriceDTO>).ToList();
            }
        }
    }
}

