namespace DBSoft.EVE.SDE
{
    public class ItemDTO
    {
        public int ItemID { get; set; }
        public int GroupID { get; set; }
        public string ItemName { get; set; }
        public bool IsPublished { get; set; }
        public int PortionSize { get; set; }
        public decimal Volume { get; set; }
    }
}