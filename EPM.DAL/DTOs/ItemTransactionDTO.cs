namespace DBSoft.EPM.DAL.DTOs
{
	using System;

	public class ItemTransactionBaseDto
	{
		public long Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal? Cost { get; set; }
		public decimal GrossAmount => Price * Quantity;

	    public decimal? GpAmt
		{
			get
			{
				if (Cost != null)
				{
					return GrossAmount - Quantity * Cost.Value;
				}
				return null;
			}

		}
		public decimal? GpPct => GrossAmount == 0 ? null : GpAmt / GrossAmount * 100;
	}

	public class ItemTransactionDto : ItemTransactionBaseDto
	{
		public DateTime DateTime { get; set; }
		public string ItemName { get; set; }
	}

	public class ItemTransactionByItemDto : ItemTransactionBaseDto
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }
	}

	public class ItemTransactionByDateDto : ItemTransactionBaseDto
	{
		public DateTime DateTime { get; set; }
	}

	public class ItemTransactionByMonthDto : ItemTransactionBaseDto
	{
		public int Month { get; set; }
		public int Year { get; set; }
		public DateTime DateTime => new DateTime(Year, Month, 1);
	}
}
