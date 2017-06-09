using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabelPrinting.Webservice.Models
{
    public class PrintingLabelsPrinter1
    {
        public List<PrintingLabel> Labels;

        public PrintingLabelsPrinter1()
        {
            Labels = new List<PrintingLabel>();
            var printerLabel1 = new PrintingLabel
            {
                PrinterId = "1",
                FontSize = "",
                LabelName = "a1.txt"
            };

            var printerLabel2 = new PrintingLabel
            {
                PrinterId = "1",
                FontSize = "",
                LabelName = "a2.txt"
            };
            Labels.Add(printerLabel1);
            Labels.Add(printerLabel2);
        }
    }
}