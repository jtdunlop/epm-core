namespace DBSoft.EPM.DAL.Extensions
{
	using System.Collections.Generic;
	using System.Linq;
	using CodeFirst.Models;
	using DTOs;

	public static class ItemExtensions
	{
/*
		public static IQueryable<Item> Intersection(this IQueryable<Item> left, IEnumerable<Item> right)
		{
			return left.Join(right, outer => outer.ID, inner => inner.ID, (outer, inner) => inner);
		}
*/

		public static IEnumerable<BuildableItemDTO> Difference(this IEnumerable<BuildableItemDTO> left, IEnumerable<Item> right)
		{
			return left.Where(f => right.All(g => g.ID != f.ItemID));
		}
	}
}
