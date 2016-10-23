namespace DBSoft.EPM.DAL.Requests
{
    using System;

    public class UpdateMaterialRequest
    {
        public string Token { get; set; }
        public int ItemID { get; set; }
        public decimal? BounceFactor { get; set; }
    }
}