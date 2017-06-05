using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using LabelPrinting.Webservice.Models;
using Newtonsoft.Json;

namespace LabelPrinting.Webservice.Controllers
{
    [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
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
        public IHttpActionResult GetPrinters()
        {
            var printersList = new List<Printer>
            {
                new Printer
                {
                    Id = "1",
                    Name = "Printer 1"
                },
                new Printer
                {
                    Id = "2",
                    Name = "Printer 2"
                }
            };
            return Ok(printersList);
        }

        /// <summary>
        /// http://localhost:50742/api/labelprinting/getprinters
        /// Read printers from config file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("getPrinterLabel")]
        // GET api/<controller>
        public IHttpActionResult GetPrinterLabel(string id)
        {
            var printerLabel1 = new PrintingLabel
            {
                PrinterId = "1",
                FontSize = ""
            };

            var printerLabel2 = new PrintingLabel
            {
                PrinterId = "1",
                FontSize = ""
            };

            if (id == "1")
                return Ok(printerLabel1);
            return Ok(printerLabel2);

            //  return JsonConvert.SerializeObject();
        }


        /// <summary>
        /// Add printer, just post to api url
        /// </summary>
        /// <param name="printerLabel"></param>
        [HttpPost]
        [ActionName("print")]
        public HttpResponseMessage Print([FromBody]PrintingLabel printerLabel)
        {
           return Request.CreateResponse(HttpStatusCode.OK, "success");
            // return Ok(true);
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

        //// PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}