namespace DBSoft.EPM.DAL.Services
{
	using CodeFirst.Models;
	using Commands;
	using DTOs;
	using Interfaces;
	using Queries;
	using System;
	using System.Collections.Generic;
	using System.Linq;

    public class EveApiStatusService : DataService, IEveApiStatusService
    {
		readonly IDbContextFactory _factory;

		public EveApiStatusService(IDbContextFactory factory, IUserService users) : base(users)
		{
			_factory = factory;
		}

		public IEnumerable<EveApiStatusDTO> List(string token)
		{
			var userID = GetUserID(token);

			using (var context = _factory.CreateContext())
			{
				var result = from status in new DataQuery<EveApiStatus>(context).GetQuery()
							 where status.UserID == userID
							 select new EveApiStatusDTO
							 {
								 ApiName = status.Name,
								 Expiry = status.CacheExpiry,
								 Result = status.Result
							 };
				return result.ToList();
			}
		}

		public void UpdateStatus(string token, string name, string result, DateTime? cachedUntil)
		{
			using ( var context = _factory.CreateContext() )
			using (var cmd = new DataCommand(context))
			{
				var userID = GetUserID(token);

				var status = cmd.Get<EveApiStatus>(f => f.Name == name && f.UserID == userID);
				status.Name = name;
				if (cachedUntil.HasValue)
				{
					status.CacheExpiry = cachedUntil.Value;
				}
				else if (status.ID == 0)
				{
					status.CacheExpiry = DateTime.Now;
				}
				status.UserID = userID;
				status.Result = result;
			}
		}
	}
}
