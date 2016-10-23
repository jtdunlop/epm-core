namespace DBSoft.EPM.DAL.Services.MaterialCosts
{
    using DTOs;

    public class MaterialCostVarianceByItemDto : MaterialCostVarianceDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemCost { get; set; }
        private decimal CostWeight => (ItemCost == 0 ? 0 : MaterialQuantity * CostNow / ItemCost);
        public decimal WeightedVariance => Variance * CostWeight ?? 0;
        public long MaterialQuantity { private get; set; }
    }
}