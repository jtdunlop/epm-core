namespace DBSoft.EPM.DAL.Services
{
	using System.Collections.Generic;
	using DTOs;

	public interface ICharacterAccountService
	{
		IEnumerable<CharacterDTO> List(string token);
	}
}