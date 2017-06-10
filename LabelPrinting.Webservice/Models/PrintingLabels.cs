using System.Collections.Generic;

namespace LabelPrinting.Webservice.Models
{
    public class PrintingLabels
    {
        public static List<PrintingLabel> Labels = new List<PrintingLabel>
        {
            new PrintingLabel
            {
                Id = "LabelId1",
                Name = "Label a1",
                PrinterId = "1",
                FontSize = "",
                LabelFileName = "a1.txt"
            },
            new PrintingLabel
            {
                Id = "LabelId2",
                Name = "Label a2",
                PrinterId = "1",
                FontSize = "",
                LabelFileName = "a2.txt"
            }
        };
    }
}