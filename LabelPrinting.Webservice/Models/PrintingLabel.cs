using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabelPrinting.Webservice.Models
{
    public class PrintingLabel
    {
        public string PrinterId { get; set; }
        public string FontSize { get; set; }
        /// <summary>
        /// used to send the image
        /// </summary>
        public string TextToPrint { get; set; }
        /// <summary>
        /// Name of the label on the disk
        /// </summary>
        public string LabelName { get; set; }
        /// <summary>
        /// test to be printed on label
        /// </summary>
        public string MaterialInfo { get; set; }
    }
}