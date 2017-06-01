using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabelPrinting.Webservice.Models
{
    public class PrinterLabel
    {
        public string PrinterId { get; set; }
        public string FontSize { get; set; }

        public string TextToPrint { get; set; }

    }
}