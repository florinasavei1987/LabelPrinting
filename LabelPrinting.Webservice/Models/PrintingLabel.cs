using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabelPrinting.Webservice.Models
{
    public class PrintingLabel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PrinterId { get; set; }
        public string FontSize { get; set; }
        public string TextToPrint { get; set; }
        /// <summary>
        /// Name of the label on the disk
        /// </summary>
        public string LabelFileName { get; set; }
        /// <summary>
        /// test to be printed on label
        /// </summary>
        public string MaterialInfo { get; set; }
        /// <summary>
        /// used to send the image
        /// </summary>
        public string ImageToPrint { get; set; }
    }
}