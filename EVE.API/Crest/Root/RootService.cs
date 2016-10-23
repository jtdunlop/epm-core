namespace DBSoft.EveAPI.Crest.Root
{
    public class Dust
    {
        public string Href { get; set; }
    }

    public class Eve
    {
        public string Href { get; set; }
    }

    public class Server
    {
        public string Href { get; set; }
    }

    public class Motd
    {
        public Dust Dust { get; set; }
        public Eve Eve { get; set; }
        public Server Server { get; set; }
    }

    public class CrestEndpoint
    {
        public string Href { get; set; }
    }

    public class CorporationRoles
    {
        public string Href { get; set; }
    }

    public class ItemGroups
    {
        public string Href { get; set; }
    }

    public class Channels
    {
        public string Href { get; set; }
    }

    public class Corporations
    {
        public string Href { get; set; }
    }

    public class Alliances
    {
        public string Href { get; set; }
    }

    public class ItemTypes
    {
        public string Href { get; set; }
    }

    public class Decode
    {
        public string Href { get; set; }
    }

    public class BattleTheatres
    {
        public string Href { get; set; }
    }

    public class MarketPrices
    {
        public string Href { get; set; }
    }

    public class ItemCategories
    {
        public string Href { get; set; }
    }

    public class Regions
    {
        public string Href { get; set; }
    }

    public class Bloodlines
    {
        public string Href { get; set; }
    }

    public class MarketGroups
    {
        public string Href { get; set; }
    }

    public class Tournaments
    {
        public string Href { get; set; }
    }

    public class Map
    {
        public string Href { get; set; }
    }

    public class VirtualGoodStore
    {
        public string Href { get; set; }
    }

    public class Wars
    {
        public string Href { get; set; }
    }

    public class Incursions
    {
        public string Href { get; set; }
    }

    public class Races
    {
        public string Href { get; set; }
    }

    public class AuthEndpoint
    {
        public string Href { get; set; }
    }

    public class ServiceStatus
    {
        public string Dust { get; set; }
        public string Eve { get; set; }
        public string Server { get; set; }
    }

    public class UserCounts
    {
        public int Dust { get; set; }
        public string DustStr { get; set; }
        public int Eve { get; set; }
        public string EveStr { get; set; }
    }

    public class Facilities
    {
        public string Href { get; set; }
    }

    public class Systems
    {
        public string Href { get; set; }
    }

    public class Industry
    {
        public Facilities Facilities { get; set; }
        public Systems Systems { get; set; }
    }

    public class Clients
    {
        public Dust Dust { get; set; }
        public Eve Eve { get; set; }
    }

    public class Time
    {
        public string Href { get; set; }
    }

    public class MarketTypes
    {
        public string Href { get; set; }
    }

    public class EndpointRoot
    {
        public Constellations Constellations { get; set; }
        public ItemGroups ItemGroups { get; set; }
        public Corporations Corporations { get; set; }
        public Alliances Alliances { get; set; }
        public ItemTypes ItemTypes { get; set; }
        public int UserCount { get; set; }
        public Decode Decode { get; set; }
        public MarketPrices MarketPrices { get; set; }
        public Opportunities Opportunities { get; set; }
        public ItemCategories ItemCategories { get; set; }
        public Regions Regions { get; set; }
        public Bloodlines Bloodlines { get; set; }
        public MarketGroups MarketGroups { get; set; }
        public Systems Systems { get; set; }
        public Sovereignty Sovereignty { get; set; }
        public Tournaments Tournaments { get; set; }
        public VirtualGoodStore VirtualGoodStore { get; set; }
        public string ServerVersion { get; set; }
        public string UserCountStr { get; set; }
        public Wars Wars { get; set; }
        public Incursions Incursions { get; set; }
        public Dogma Dogma { get; set; }
        public Races Races { get; set; }
        public InsurancePrices InsurancePrices { get; set; }
        public AuthEndpoint AuthEndpoint { get; set; }
        public string ServiceStatus { get; set; }
        public Industry Industry { get; set; }
        public NpcCorporations NpcCorporations { get; set; }
        public Time Time { get; set; }
        public MarketTypes MarketTypes { get; set; }
        public string ServerName { get; set; }
    }

    public class Constellations
    {
        public string Href { get; set; }
    }

    public class Opportunities
    {
        public Tasks Tasks { get; set; }
        public Groups Groups { get; set; }
    }


    public class Sovereignty
    {
        public Campaigns Campaigns { get; set; }
        public Structures Structures { get; set; }
    }

    public class Dogma
    {
        public Attributes Attributes { get; set; }
        public Effects Effects { get; set; }
    }

    public class InsurancePrices
    {
        public string Href { get; set; }
    }

    public class NpcCorporations
    {
        public string Href { get; set; }
    }

    public class Tasks
    {
        public string Href { get; set; }
    }

    public class Groups
    {
        public string Href { get; set; }
    }

    public class Campaigns
    {
        public string Href { get; set; }
    }

    public class Structures
    {
        public string Href { get; set; }
    }

    public class Attributes
    {
        public string Href { get; set; }
    }

    public class Effects
    {
        public string Href { get; set; }
    }
}
