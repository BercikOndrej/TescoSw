using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TescoSwTask.Helpers;

namespace TescoSwTask.Models
{
    public class Car
    {
        public string Name { get; set; }
        public DateTime DateOfSale { get; set; }
        public double Price { get; set; }
        public double DPH   { get; set; }

        public string ShowPrice => Helper.FormatNumber(Price);
        public double PriceWithDPH => (Price / 100) * DPH + Price;
    }
}
