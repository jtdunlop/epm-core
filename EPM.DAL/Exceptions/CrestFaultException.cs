namespace DBSoft.EPM.DAL.Exceptions
{
    using System;

    public class CrestFaultException : Exception
    {
        public CrestFaultException(string msg, Exception e)
            : base(msg, e)
        {
        }
    }
}