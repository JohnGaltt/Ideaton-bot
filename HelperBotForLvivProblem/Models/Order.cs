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

        public string PohneNumber { get; set; }

        public string Adress { get; set; }

        public string Email { get; set; }

        public string DescriptionOfOrder { get; set; }

        public string _TypeOrder { get; set; }

        public string LvivRegion { get; set; }

        //public int Fib(int i)
        //{
        //    if (i < 1)
        //        return 1;
        //    return Fib(i - 1) + Fib(i - 2);
        //}
    }
    [Serializable]
    public enum TypeOrder
    {
        HomeTruble,
        DTP,
        Traffic,
        SomethingElse
    }
}