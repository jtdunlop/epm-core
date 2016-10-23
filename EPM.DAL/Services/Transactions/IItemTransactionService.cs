namespace DBSoft.EPM.DAL.Services.Transactions
{
    using System.Collections.Generic;
    using DTOs;

    public interface IItemTransactionService
	{
		IEnumerable<ItemTransactionDto> List(ItemTransactionRequest request);
		List<ItemTransactionByItemDto> ListByItem(ItemTransactionRequest request);
		IEnumerable<ItemTransactionByDateDto> ListByDate(ItemTransactionRequest request);
		IEnumerable<ItemTransactionByMonthDto> ListByMonth(ItemTransactionRequest request);
        List<ItemTransactionBySubscriberDto> ListBySubscriber(SubscriberTransactionRequest subscriberTransactionRequest);
	}

    public class ItemTransactionByUserDto : ItemTransactionBaseDto
    {
        public string UserName { get; set; }
        public string ItemName { get; set; }
    }
}