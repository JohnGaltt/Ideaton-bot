using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelperBotForLvivProblem.Models
{
    [Serializable]
    public class Order
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Location { get; set; }

        public string Adress { get; set; }

        public string Email { get; set; }

        public TypeOrder _TypeOrder { get; set; }
    }
    [Serializable]
    public enum TypeOrder
    {
        HomeTruble,
        DTP,
        Traffic
    }
}