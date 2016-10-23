namespace DBSoft.EPM.DAL.Services.Materials
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Aspects;
    using CodeFirst.Models;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using Interfaces;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class BuildMaterialService : IBuildMaterialService
    {
        private readonly IItemService _items;
        private readonly IUserService _users;
        private readonly DbContext _context;

        public BuildMaterialService(IItemService items, IDbContextFactory factory, IUserService users)
        {
            _items = items;
            _users = users;
            _context = factory.CreateContext();
        }

        [Trace, Cache.Cacheable(name: "BuildMaterials")]
        public List<BuildMaterialDto> ListBuildable(string token)
        {
            var userId = _users.GetUserID(token);
            var buildables = _items.ListBuildable(token).Select(f => new ItemDTO
            {
                ItemID = f.ItemID,
                ItemName = f.ItemName,
                QuantityMultiplier = f.QuantityMultiplier,
                Volume = f.Volume
            });
            var result = GetBuildMaterials(buildables, userId);
            var @return = result.ToList();

            return @return;
        }

        [Trace, Cache.Cacheable]
        public List<BuildMaterialDto> ListProducible([CanBeNull] string token)
        {
            // Authenticated user doesn't come into play when getting materials for items that can be produced
            var userId = token == null ? 0 : _users.GetUserID(token);
            var items = _items.ListProducible();
            var result = GetBuildMaterials(items, userId);
            var @return = result.ToList();

            return @return;
        }

        private IEnumerable<BuildMaterialDto> GetBuildMaterials(IEnumerable<ItemDTO> buildables, int userId)
        {
            var manifests = from manifest in _context.Set<Manifest>()
                from extension in
                    _context.Set<ItemExtension>()
                        .Where(f => f.ItemID == manifest.MaterialItemID && f.UserID == userId)
                        .DefaultIfEmpty()
                join item in _context.Set<Item>() on manifest.ItemID equals item.ID
                join material in _context.Set<Item>() on manifest.MaterialItemID equals material.ID
                select new
                {
                    manifest.ItemID,
                    ItemName = item.Name,
                    manifest.MaterialItemID,
                    MaterialItemName = material.Name,
                    manifest.Quantity,
                    BounceFactor = extension == null ? 1 : extension.BounceFactor,
                    material.Volume
                };

            var result = from manifest in manifests.ToList()
                join buildable in buildables on manifest.ItemID equals buildable.ItemID
                group new {manifest} by new
                {
                    manifest.ItemID,
                    manifest.ItemName,
                    manifest.MaterialItemID,
                    MaterialName = manifest.MaterialItemName,
                    manifest.BounceFactor,
                    Multiplier = buildable.QuantityMultiplier,
                    manifest.Volume
                }
                into grp
                select new BuildMaterialDto
                {
                    ItemId = grp.Key.ItemID,
                    ItemName = grp.Key.ItemName,
                    MaterialId = grp.Key.MaterialItemID,
                    MaterialName = grp.Key.MaterialName,
                    BounceFactor = grp.Key.BounceFactor ?? 1,
                    Quantity = grp.Sum(f => f.manifest.Quantity),
                    QuantityMultiplier = grp.Key.Multiplier,
                    Volume = grp.Key.Volume
                };
            return result;
        }
    }
}
