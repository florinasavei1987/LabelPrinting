using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LabelPrinting.Webservice.Models;
using Newtonsoft.Json;

namespace LabelPrinting.Webservice.Controllers
{
    public class LabelPrintingController : ApiController
    {
        /// <summary>
        /// http://localhost:50742/api/labelprinting/getprinters
        /// Read printers from config file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("getPrinters")]
        // GET api/<controller>
        public IEnumerable<string> GetPrinters()
        {
            return new string[] { "Printer1", "Printer2" };
        }

        /// <summary>
        /// http://localhost:50742/api/labelprinting/getprinters
        /// Read printers from config file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("getPrinter")]
        // GET api/<controller>
        public string GetPrinterLabel(string id)
        {
            return JsonConvert.SerializeObject(new PrinterLabel
            {
                PrinterId = "testid",
                FontSize = ""
            });
        }


        /// <summary>
        /// Add printer, just post to api url
        /// </summary>
        /// <param name="value"></param>
        public void Post([FromBody]string value)
        {
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}