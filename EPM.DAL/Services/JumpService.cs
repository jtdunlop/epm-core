namespace DBSoft.EPM.DAL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using Interfaces;
    using Queries;

    public class JumpService : IJumpService
    {
        private readonly IDbContextFactory _factory;
        private readonly IUniverseService _universe;

        public JumpService(IDbContextFactory factory, IUniverseService universe)
        {
            _factory = factory;
            _universe = universe;
        }

        [Cache.Cacheable]
        public short GetJumps(int factorySystem, int solarSystemId)
        {
            if ( factorySystem == solarSystemId )
            {
                return 0;
            }
            var region = GetRegion(factorySystem);

            if (region != GetRegion(solarSystemId))
            {
                throw new Exception($"{factorySystem} is not in the same region as {solarSystemId}");
            }
            var jumps = GetAllJumps(region, factorySystem);
            var jump = jumps.Single(f => f.SolarSystemID == factorySystem && f.TargetSolarSystemID == solarSystemId);
            return jump.Jumps;
        }

        [Cache.Cacheable]
        private int GetRegion(int sys)
        {
            var systems = _universe.ListSolarSystems().ToList();
            var region = systems.Single(f => f.SolarSystemID == sys).RegionID;
            return region;
        }

        [Cache.Cacheable]
        private IEnumerable<JumpDTO> GetAllJumps(int region, int origin)
        {
            var result = new List<JumpDTO>();
            PopulateJumps(region, origin, origin, 1, result);
            return result;
        }

        private void PopulateJumps(int region, int home, int origin, short jumps, ICollection<JumpDTO> result )
        {
            using ( var context = _factory.CreateContext())
            {
                var gates = new DataQuery<Gate>(context).GetQuery().Where(f => f.SolarSystemID == origin).ToList();
                foreach (var gate in gates)
                {
                    var system = new DataQuery<SolarSystem>(context)
                        .GetQuery()
                        .SingleOrDefault(f => f.ID == gate.TargetSolarSystemID && f.RegionID == region && f.ID != home);
                    if (system == null) continue;
                    var entry = result.SingleOrDefault(f => f.TargetSolarSystemID == system.ID);
                    if (entry == null)
                    {
                        result.Add(new JumpDTO
                        {
                            SolarSystemID = home,
                            TargetSolarSystemID = system.ID,
                            Jumps = jumps
                        });
                        PopulateJumps(region, home, gate.TargetSolarSystemID, (short)(jumps + 1), result);
                    }
                    else if (jumps < entry.Jumps)
                    {
                        entry.Jumps = jumps;
                        // If the route to here is shorter then all subsequent jumps are shorter as well
                        PopulateJumps(region, home, gate.TargetSolarSystemID, (short)(jumps + 1), result);
                    }
                }
            }
        }
    }
}
