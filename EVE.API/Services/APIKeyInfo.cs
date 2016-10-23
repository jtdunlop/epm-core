namespace DBSoft.EVEAPI
{
	using System;
	using System.Collections.Generic;
	public class ApiKeyInfo : EveApiService
	{
		public ApiKeyInfo(int keyID, string vCode)
			: base(String.Format("http://api.eve-online.com/account/APIKeyInfo.xml.aspx?keyID={0}&vCode={1}",
				keyID, vCode))
		{
			this.keyID = keyID;
			this.vCode = vCode;
			Characters = new List<Character>();
			Corporations = new List<Corporation>();
		}

		public int AccessMask { get; private set; }
		public string Type { get; private set; }
		public DateTime Expires { get; private set; }
		public List<Character> Characters { get; private set; }
		public List<Corporation> Corporations { get; private set; }

		public void Retrieve()
		{
			Load();
			if ( Success )
			{
				var key = Result.Element("key");
				AccessMask = int.Parse(key.Attribute("accessMask").Value);
				Type = key.Attribute("type").Value;

				var rowset = key.Element("rowset");
				var rows = rowset.Elements("row");
				foreach ( var row in rows )
				{
					int id = int.Parse(row.Attribute("characterID").Value);
					string name = row.Attribute("characterName").Value;
					int corpid = int.Parse(row.Attribute("corporationID").Value);
					string corpname = row.Attribute("corporationName").Value;
					Characters.Add(new Character { ID = id, Name = name, CorporationID = corpid, 
						CorporationName = corpname });
				}
			}
			else
			{
				throw new Exception(ErrorMessage);
			}
		}

		int keyID;
		string vCode;
	}
}
