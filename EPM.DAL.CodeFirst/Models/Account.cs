namespace DBSoft.EPM.DAL.CodeFirst.Models
{
	using System.Collections.Generic;

	public enum AccountType
	{
		Character,
		Corporation
	}

    public class Account
    {
        public Account()
        {
            AccountBalances = new List<AccountBalance>();
            Assets = new List<Asset>();
            Characters = new List<Character>();
			Corporations = new List<Corporation>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int ApiKeyID { get; set; }
        public string ApiVerificationCode { get; set; }
        public AccountType ApiKeyType { get; set; }
        public int ApiAccessMask { get; set; }
        public bool DeletedFlag { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<AccountBalance> AccountBalances { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
		public virtual ICollection<Corporation> Corporations { get; set; }
    }
}
