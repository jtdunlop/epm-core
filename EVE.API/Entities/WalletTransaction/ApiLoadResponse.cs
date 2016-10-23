namespace DBSoft.EVEAPI.Entities.WalletTransaction
{
    using System;
    using System.Collections.Generic;

    public class ApiLoadResponse<T>
    {
        public ApiLoadResponse()
        {
            Data = new List<T>();
        }

        public DateTime CachedUntil { get; set; }
        public List<T> Data { get; set; } 
    }
}