namespace DBSoft.EVEAPI.Entities.Account
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Annotations;
    using Plumbing;
	using System;
	using System.Collections.Generic;

	public enum ApiKeyType
	{
		Character,
		Corporation
	}

	public class ApiKeyInfo
	{
		public ApiKeyInfo()
		{
			Characters = new List<Character>();
		}
		public List<Character> Characters { get; set; }
		public int AccessMask { get; set; }
		public ApiKeyType ApiKeyType { get; set; }

	}

    [UsedImplicitly]
    public class ApiKeyInfoService : IApiKeyInfoService
	{
        private readonly IEveApiLoader _loader;

        public ApiKeyInfoService(IEveApiLoader loader)
        {
            _loader = loader;
        }

        public async Task<IEnumerable<ApiKeyInfo>> Load(int keyId, string vCode)
		{
			var result = new ApiKeyInfo();

			var response = await _loader.Load(
			    $"http://api.eve-online.com/account/APIKeyInfo.xml.aspx?keyID={keyId}&vCode={vCode}");
			if (!response.Success)
			{
				throw new Exception(response.ErrorMessage);
			}
			var key = response.Result.Element("key");
			Debug.Assert(key != null, "key != null");
			result.AccessMask = int.Parse(key.Attribute("accessMask").Value);
			result.ApiKeyType = GetKeyType(key.Attribute("type").Value);

			var rowset = key.Element("rowset");
			if (rowset == null) return new List<ApiKeyInfo>
			{
				result
			};
			var rows = rowset.Elements("row");
			foreach (var row in rows)
			{
				var id = int.Parse(row.Attribute("characterID").Value);
				var name = row.Attribute("characterName").Value;
				var corpid = int.Parse(row.Attribute("corporationID").Value);
				var corpname = row.Attribute("corporationName").Value;
				result.Characters.Add(new Character
					{
						ID = id,
						Name = name,
						CorporationID = corpid,
						CorporationName = corpname
					});
			}
			return new List<ApiKeyInfo>
			{
				result
			};
		}

		private static ApiKeyType GetKeyType(string type)
		{
			switch (type)
			{
				case "Character":
					return ApiKeyType.Character;
				case "Account": // Account is just a convenience for all characters
					return ApiKeyType.Character;
				case "Corporation":
					return ApiKeyType.Corporation;
			}
			throw new Exception("Unknown key type");
		}
	}
}