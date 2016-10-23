namespace DBSoft.EPM.DAL.Services.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using Aspects;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Interfaces;
    using JetBrains.Annotations;
    using Queries;

    [UsedImplicitly]
    public class ItemTransactionService : IItemTransactionService
	{
        private readonly IDbContextFactory _factory;
        private readonly IUserService _users;

		public ItemTransactionService(IDbContextFactory factory, IUserService users)
		{
		    _factory = factory;
		    _users = users;
		}

        [Trace,Cache.Cacheable]
		public IEnumerable<ItemTransactionDto> List(ItemTransactionRequest request)
		{
            using (var context = _factory.CreateContext())
            {
                var transactions = new TransactionQuery(context).Specify(f => f.VisibleFlag);
                ApplySpecifications(request, transactions);

                var result = from transaction in transactions.GetQuery()
                             orderby new { transaction.Item.Name, TransactionID = transaction.ID, transaction.DateTime }
                             select new ItemTransactionDto
                             {
                                 ItemName = transaction.Item.Name,
                                 DateTime = transaction.DateTime,
                                 Cost = transaction.Cost,
                                 Quantity = (long)transaction.Quantity,
                                 Price = transaction.Price
                             };
                return result.ToList();
            }
        }

		[Trace,Cache.Cacheable]
		public List<ItemTransactionByItemDto> ListByItem(ItemTransactionRequest request)
		{
			ValidateToken(request.Token);

		    using (var context = _factory.CreateContext())
		    {
                var transactions = new TransactionQuery(context).SpecifyVisible();
                ApplySpecifications(request, transactions);

                var result = from transaction in transactions.GetQuery()
                             orderby new { transaction.ItemID, transaction.Item.Name }
                             group transaction by new
                             {
                                 transaction.ItemID,
                                 ItemName = transaction.Item.Name
                             } into grp
                             select new ItemTransactionByItemDto
                             {
                                 ItemId = grp.Key.ItemID,
                                 ItemName = grp.Key.ItemName,
                                 Quantity = grp.Sum(f => (long)f.Quantity),
                                 Price = grp.Sum(f => f.Quantity * f.Price / grp.Sum(t => (long)t.Quantity)),
                                 Cost = grp.Sum(f => f.Quantity * f.Cost / grp.Sum(t => (long)t.Quantity))
                             };
                var list = result.ToList();
                return list;
            }
        }

		private static void ValidateToken(string token)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(token));
		}

		[Trace,Cache.Cacheable]
		public IEnumerable<ItemTransactionByDateDto> ListByDate(ItemTransactionRequest request)
		{
			ValidateToken(request.Token);

		    using (var context = _factory.CreateContext())
		    {
                var transactions = new TransactionQuery(context).Specify(f => f.VisibleFlag);
                ApplySpecifications(request, transactions);

                var result = from transaction in transactions.GetQuery()
                             orderby new { transaction.ItemID }
                             group transaction by new
                             {
                                 DateTime = DbFunctions.TruncateTime(transaction.DateTime).Value
                             } into grp
                             select new ItemTransactionByDateDto
                             {
                                 DateTime = grp.Key.DateTime,
                                 Quantity = grp.Sum(f => (long)f.Quantity),
                                 Price = grp.Sum(f => (long)f.Quantity * f.Price / grp.Sum(t => (long)t.Quantity)),
                                 Cost = grp.Sum(f => (long)f.Quantity * f.Cost / grp.Sum(t => (long)t.Quantity))
                             };
                return result.ToList();
            }
        }

		[Trace,Cache.Cacheable]
		public IEnumerable<ItemTransactionByMonthDto> ListByMonth(ItemTransactionRequest request)
		{
			ValidateToken(request.Token);

		    using (var context = _factory.CreateContext())
		    {
                var transactions = new TransactionQuery(context).SpecifyVisible();
                ApplySpecifications(request, transactions);

                var result = from transaction in transactions.GetQuery()
                             orderby new { transaction.ItemID }
                             group transaction by new
                             {
                                 transaction.DateTime.Month,
                                 transaction.DateTime.Year
                             } into grp
                             select new ItemTransactionByMonthDto
                             {
                                 Month = grp.Key.Month,
                                 Year = grp.Key.Year,
                                 Quantity = grp.Sum(f => (long)f.Quantity),
                                 Price = grp.Sum(f => f.Quantity * f.Price / grp.Sum(t => (long)t.Quantity)),
                                 Cost = grp.Sum(f => f.Quantity * f.Cost / grp.Sum(t => (long)t.Quantity))
                             };
                return result.ToList();
            }
        }

        [Cache.Cacheable]
        public List<ItemTransactionBySubscriberDto> ListBySubscriber(SubscriberTransactionRequest request)
        {
            ValidateToken(request.Token);

            using (var context = _factory.CreateContext())
            {
                var transactions = new TransactionQuery(context).Specify(f => f.VisibleFlag);
                SpecifyDateRange(transactions, request.DateRange.StartDate, request.DateRange.EndDate);
                if (request.TransactionType.HasValue)
                {
                    SpecifyTransactionType(transactions, request.TransactionType.Value);
                }
                var t = transactions.GetQuery().ToList();

                var result = from transaction in t
                             group transaction by new
                             {
                                 transaction.UserID
                             } into grp
                             select new ItemTransactionBySubscriberDto
                             {
                                 UserId = grp.Key.UserID,
                                 EveOnlineCharacter = GetSubscriber(grp.Key.UserID),
                                 Quantity = grp.Sum(f => (long)f.Quantity),
                                 Price = grp.Sum(f => (long)f.Quantity * f.Price / grp.Sum(p => (long)p.Quantity)),
                                 Cost = grp.Sum(f => (long)f.Quantity * f.Cost / grp.Sum(c => (long)c.Quantity))
                             };
                return result.ToList();
            }
        }

        [Cache.Cacheable]
        private string GetSubscriber(int userId)
        {
            using (var context = _factory.CreateContext())
            {
                var subscriber = new DataQuery<EveAccount>(context).GetQuery().FirstOrDefault(f => f.UserID == userId);
                return subscriber == null ? userId.ToString(CultureInfo.InvariantCulture) : subscriber.EveCharacterName;
            }
        }

        private void ApplySpecifications(ItemTransactionRequest request, DataQuery<Transaction> transactions )
		{
			var userId = _users.GetUserID(request.Token);
			transactions.Specify(f => f.UserID == userId);
			if (request.DateRange != null)
			{
				SpecifyDateRange(transactions, request.DateRange.StartDate, request.DateRange.EndDate);
			}
			if (request.TransactionType.HasValue)
			{
				SpecifyTransactionType(transactions, request.TransactionType.Value);
			}
			if (request.ItemID.HasValue)
			{
			    transactions.Specify(f => f.ItemID == request.ItemID);
			}
		}

		private static void SpecifyDateRange(DataQuery<Transaction> transactions, DateTime startDate, DateTime endDate)
		{
			var start = startDate.StartOfTheDay();
			var end = endDate.EndOfTheDay();
			transactions.Specify(f => f.DateTime >= start && f.DateTime <= end);
		}

		private static void SpecifyTransactionType( DataQuery<Transaction> transactions, TransactionType transactionType)
		{
			transactions.Specify(f => f.TransactionType == transactionType);
		}
	}
}
