namespace DBSoft.EPM.DAL.DTOs
{
	using System;

	public class EveApiStatusDTO
	{
		public string ApiName { get; set; }
		public DateTime? Expiry { get; set; }
		public string Result { get; set; }
        public bool Expired { get { return Expiry < DateTime.UtcNow; } }
	}
}