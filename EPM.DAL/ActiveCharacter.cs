//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPM.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ActiveCharacter
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AccountID { get; set; }
        public int EveApiID { get; set; }
        public int ApiKeyID { get; set; }
        public string ApiVerificationCode { get; set; }
    }
}
