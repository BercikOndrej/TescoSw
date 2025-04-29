using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TescoSwTask.Helpers;

namespace TescoSwTask.Models
{
    public class SummaryInfo
    {
        public string ModelName { get; set; }
        public double PriceWithoutDph { get; set; }
        public double PriceWithDph { get; set; }

        public string ShowPriceWithoutDph => Helper.FormatNumber(PriceWithoutDph);
        public string ShowPriceWithDph => Helper.FormatNumber(PriceWithDph);
    }
}
