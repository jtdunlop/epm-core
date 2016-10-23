namespace Dbsoft.Epm.Web.Controllers.api
{
    public class DashboardModel
    {
        public SalesModel Sales7 { get; set; }
        public SalesModel Sales30 { get; set; }
        public decimal Capital { get; set; }
        public decimal WalletBalance { get; set; }
        public decimal? CapitalRatio { get { return Sales30.Sales == 0 ? (decimal?) null : (Capital + WalletBalance) / (Sales30.Sales); } }
    }
}