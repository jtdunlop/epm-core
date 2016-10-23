namespace DBSoft.EPM.DAL.DTOs
{
    using System;

    public class MaterialItemDto
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public decimal? BounceFactor { get; set; }
        public DateTime? LastModified { get; set; }
        public decimal Volume { get; set; }
	}
}
