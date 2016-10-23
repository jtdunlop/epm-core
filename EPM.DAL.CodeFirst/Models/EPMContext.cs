namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Reflection;
    using Mapping;

	public class EPMContext : DbContext
	{
		static EPMContext()
		{
			Database.SetInitializer<EPMContext>(null);
		}

		public EPMContext()
			: base("Name=EPMContext")
		{
		}

		public EPMContext(string connectionString) : base(connectionString)
		{
		}

		public DbSet<Account> Accounts { get; set; }
		public DbSet<AccountBalance> AccountBalances { get; set; }
		public DbSet<ApplicationLog> ApplicationLogs { get; set; }
		public DbSet<Asset> Assets { get; set; }
		public DbSet<Blueprint> Blueprints { get; set; }
		public DbSet<BlueprintInstance> BlueprintInstances { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Character> Characters { get; set; }
		public DbSet<Configuration> Configurations { get; set; }
		public DbSet<Corporation> Corporations { get; set; }
		public DbSet<EveApiStatus> EveApiStatus { get; set; }
		public DbSet<Group> Groups { get; set; }
        public DbSet<HiredTeam> HiredTeams { get; set; }
		public DbSet<IpAddressBlacklist> IpAddressBlacklists { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<ItemExtension> ItemExtensions { get; set; }
		public DbSet<Manifest> Manifests { get; set; }
		public DbSet<MarketImport2> MarketImports { get; set; }
		public DbSet<MarketOrder> MarketOrders { get; set; }
        public DbSet<MarketPrice> MarketPrices { get; set; }
		public DbSet<ProductionJob> ProductionJobs { get; set; }
		public DbSet<Region> Regions { get; set; }
		public DbSet<SchemaVersion> SchemaVersions { get; set; }
		public DbSet<SolarSystem> SolarSystems { get; set; }
		public DbSet<Station> Stations { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserWhitelist> UserWhitelists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var configurationInstance in typesToRegister.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add((dynamic) configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
	}
}
