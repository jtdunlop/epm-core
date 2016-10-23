namespace DBSoft.EPM.DAL.Services.MaterialCosts
{
    using Enums;

    public interface IFreightCalculator
    {
        decimal GetFreightCost(string token, decimal price, decimal volume);
    }
}