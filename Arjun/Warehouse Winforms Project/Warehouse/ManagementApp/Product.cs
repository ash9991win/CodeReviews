using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementApp
{
    public class Product
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Availability { get; set; }
        public string Location { get; set; }
        public string Dpci { get; set; }
        public double Cost { get; set; }
        public double TotalCost { get; set; }
        public int Count { get; set; }
        public string FullDetails { get; set; }

        public List<string> Summary = new List<string>();

    }
}
