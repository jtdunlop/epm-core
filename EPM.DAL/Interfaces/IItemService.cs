namespace DBSoft.EPM.DAL.Interfaces
{
	using System.Collections.Generic;
	using DTOs;
	using Requests;

	public interface IItemService
	{
		List<ItemDTO> List(ItemRequest request);
		List<BuildableItemDTO> ListMaintainable(string token);
		List<BuildableItemDTO> ListBuildable(string token);
		void UpdateMaterial(UpdateMaterialRequest request);
		void UpdateItem(UpdateItemRequest request);
	    List<ItemDTO> ListProducible();
        List<ItemDTO> ListProducibleMaterials();
	}
}