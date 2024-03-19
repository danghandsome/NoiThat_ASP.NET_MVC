using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoiThat
{
    public class XMessage
    {
        public string TypeMes { get; set; }
        public string Message { get; set; }
        public XMessage() { }
        public XMessage(string typeMes, string mes) 
        {
            this.TypeMes = typeMes;
            this.Message = mes;
        }
    }
}