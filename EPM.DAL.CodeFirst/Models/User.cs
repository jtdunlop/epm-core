using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class User
    {
        public User()
        {
            Accounts = new List<Account>();
            Assets = new List<Asset>();
            Configurations = new List<Configuration>();
            EveApiStatus = new List<EveApiStatus>();
            ItemExtensions = new List<ItemExtension>();
            MarketImports = new List<MarketImport2>();
            MarketOrders = new List<MarketOrder>();
            Transactions = new List<Transaction>();
            HiredTeamAuctions = new List<HiredTeamAuction>();
            ProductionJobs = new List<ProductionJob>();
            EveAccounts = new List<EveAccount>();
        }

        public int ID { get; set; }
        public string Login { get; set; }
        public string EveOnlineCharacter { get; set; }
        public string Password { get; set; }
        public int? AuthenticationFailures { get; set; }
        public DateTime? LockedUntil { get; set; }
        public DateTime? LastLogin { get; set; }
        public string SsoRefreshToken { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Configuration> Configurations { get; set; }
        public virtual ICollection<EveApiStatus> EveApiStatus { get; set; }
        public virtual ICollection<ItemExtension> ItemExtensions { get; set; }
        public virtual ICollection<MarketImport2> MarketImports { get; set; }
        public virtual ICollection<MarketOrder> MarketOrders { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<HiredTeamAuction> HiredTeamAuctions { get; set; }
        public virtual ICollection<ProductionJob> ProductionJobs { get; set; }
        public virtual ICollection<EveAccount> EveAccounts { get; set; }
    }
}
