using Microsoft.AspNetCore.Mvc;

namespace Dbsoft.Epm.Web.Controllers.api
{
    using DBSoft.EPM.DAL.Interfaces;
    using DBSoft.EPM.DAL.Requests;
    using DBSoft.EPM.DAL.Services;
    using Infrastructure;

    [Route("api/[controller]")]
    public class ItemController : EpmController
    {
        private readonly IItemService _items;

        public ItemController(IItemService items, IUserService users) : base(users)
        {
            _items = items;
        }

        [HttpPut]
        [Route("{id}/multiplier")]
        public void UpdateMultiplier(int id, [FromBody] UpdateMultiplierRequest multiplier)
        {
            var request = new UpdateMaterialRequest
            {
                Token = Token,
                BounceFactor = multiplier.BounceFactor,
                ItemID = id
            };
            _items.UpdateMaterial(request);
        }

        [HttpPut]
        [Route("{id}/batchsize")]
        public void UpdateBatchSize(int id, [FromBody] UpdateBatchSizeRequest request)
        {
            _items.UpdateItem(new UpdateItemRequest
            {
                Token = Token,
                MinimumStock = request.BatchSize,
                ItemID = id
            });
        }

    }

    public class UpdateBatchSizeRequest
    {
        public int BatchSize { get; set; }
    }

    public class UpdateMultiplierRequest
    {
        public decimal BounceFactor { get; set; }
    }
}
