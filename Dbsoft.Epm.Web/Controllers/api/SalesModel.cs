namespace Dbsoft.Epm.Web.Controllers.api
{
    public class SalesModel
    {
        public decimal Sales { get; set; }
        public decimal? Profit { get; set; }
        public decimal? GpPct
        {
            get
            {
                if (Sales == 0)
                {
                    return 0;
                }
                return Profit * 100 / Sales;
            }
        }
    }
}