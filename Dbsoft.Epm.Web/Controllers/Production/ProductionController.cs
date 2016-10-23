using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Dbsoft.Epm.Web.Controllers.Production
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DBSoft.EPM.DAL.Interfaces;
    using DBSoft.EPM.DAL.Requests;
    using DBSoft.EPM.DAL.Services;
    using DBSoft.EPM.DAL.Services.Contracts;
    using DBSoft.EPM.DAL.Services.Materials;
    using DBSoft.EPM.Logic;
    using InboundContract;
    using OutboundContract;
    using PostBuyOrders;
    using PostSellOrders;
    using UpdateBuyOrders;
    using UpdateSellOrders;

    public class ProductionController : EpmController
    {
        private readonly IMarketRestockService _marketRestockService;
        private readonly IProductionQueueService _productionQueueService;
        private readonly IProductionMaterialService _productionMaterialService;
        private readonly IItemService _itemService;
        private readonly IMarketRepriceService _marketRepriceService;
        private readonly IMaterialPurchaseService _materialPurchaseService;
        private readonly IMaterialItemService _materialItemService;
        private readonly IImportManager _importer;
        private readonly IContractService _contractService;

        public ProductionController(IMarketRestockService marketRestockService, IProductionQueueService productionQueueService,
            IProductionMaterialService productionMaterialService, IItemService itemService, IMarketRepriceService marketRepriceService,
            IMaterialPurchaseService materialPurchaseService, IMaterialItemService materialItemService,
            IContractService contractService, IImportManager importer, IUserService users) : base(users)
        {
            _marketRestockService = marketRestockService;
            _productionQueueService = productionQueueService;
            _productionMaterialService = productionMaterialService;
            _itemService = itemService;
            _marketRepriceService = marketRepriceService;
            _materialPurchaseService = materialPurchaseService;
            _materialItemService = materialItemService;
            _importer = importer;
            _contractService = contractService;
        }

        public ActionResult RefreshApi()
        {
            var model = new RefreshApiModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult StartApiRefresh()
        {
            Task.Run(() => _importer.StartImport(Token));
            return Json("OK");
        }

        public JsonResult GetApiStatus()
        {
            var items = _importer.List(Token).OrderBy(f => f.ApiName).ToList();
            return Json(items);
        }

        public ActionResult MarketRestock()
        {
            return View(new PostSellOrdersModel(_marketRestockService, Token));
        }

        public ActionResult MarketReprice()
        {
            var model = new UpdateSellOrderModel(_marketRepriceService, Token);
            return View(model);
        }

        public ActionResult ItemBuild()
        {
            return View(new ProductionQueueModel(_productionQueueService, Token));
        }

        [HttpPost]
        public JsonResult SaveItem(BuildableItemModel model)
        {
            try
            {
                _itemService.UpdateItem(new UpdateItemRequest
                {
                    ItemID = model.ItemId,
                    Token = Token,
                    MinimumStock = model.MinimumStock,
                    PerJobAdditionalCost = model.PerJobAdditionalCost
                });
                return Json(new
                {
                    Success = true
                });
            }
            catch (Exception e)
            {
                var result = Json(new { Success = false, e.Message });
                return result;
            }

        }

        [HttpPost]
        public JsonResult SaveMaterial(MaterialItemModel model)
        {
            try
            {
                _itemService.UpdateMaterial(new UpdateMaterialRequest
                {
                    ItemID = model.ItemID,
                    Token = Token,
                    BounceFactor = model.BounceFactor
                });
                return Json(new
                {
                    Success = true,
                    LastModified = DateTime.UtcNow.ToString("d")
                });
            }
            catch (Exception e)
            {
                var result = Json(new { Success = false, e.Message });
                return result;
            }

        }

        public ActionResult MaterialItemList()
        {
            var model = new MaterialItemListModel(_materialItemService, Token, HttpContext.Request.Path);
            return PartialView(model);
        }

        public ActionResult PurchaseRelist()
        {
            var model = new PostBuyOrderModel(_materialPurchaseService, Token, IsEveClient);
            return View(model);
        }

        public ActionResult PurchaseReprice()
        {
            var model = new UpdateBuyOrderModel(_materialPurchaseService, Token);
            return View(model);
        }

        public ActionResult InboundContract()
        {
            var model = new InboundContractModel(_contractService, Token);
            return View(model);
        }

        public ActionResult OutboundContract()
        {
            var model = new OutboundContractModel(_contractService, Token);
            return View(model);
        }

        public ActionResult ProductionMaterial()
        {
            return View(new ProductionMaterialModel(_productionMaterialService, Token,
                HttpContext.Request.Path));

        }
    }
}
