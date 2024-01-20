using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyLineControl
{
    internal class OrderDetails
    {
        public string amount { get; set; }
        public string customerID { get; set; }
        public string date { get; set; }
        public string delAddress { get; set; }
        public string dueDate { get; set; }
        public string orderID { get; set; }
        public Dictionary<string,int> orderList { get; set; }
        public string orderStatus { get; set; }
        public string paymentType { get; set; }
        public string payment_Status { get; set; }
    }
}
