using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
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
        public HttpResponseMessage Print([FromBody] PrintingLabel printerLabel)
        {
            MakePrint(printerLabel.TextToPrint);
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
            return "value"; //test b
        }

        //// PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
            //%MaterialInfo%
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public void MakePrint(string info)
        {
           
            // Get the path of labels
            string pathToLabelFiles = ConfigurationManager.AppSettings["labelPath"];
            string strFilePath = pathToLabelFiles + "print.bat";
            Dictionary<string, string> infos = new Dictionary<string, string>();
            infos.Add("%MaterialInfo%", info);
            string printLabelName = PutText(Path.Combine(pathToLabelFiles,"a1.txt"), infos);
            string ftpUser = ConfigurationManager.AppSettings["ftpUserName"];
            string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
            string defaultPrinter = ConfigurationManager.AppSettings["ftpServer"];
            // Create ProcessInfo object

            var psi = new ProcessStartInfo("cmd.exe");
            psi.Arguments = pathToLabelFiles + printLabelName + " " + defaultPrinter;
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WorkingDirectory = pathToLabelFiles;
            // Start the process
            Process proc = Process.Start(psi);
            // Attach the output for reading
            StreamReader sOut = proc.StandardOutput;
            // Attach the in file for writing
            StreamWriter sIn = proc.StandardInput;

            /*
             * This is the contents of the batch file
             * @echo off
                echo user anonymous>ftpcmd.dat
                echo >> ftpcmd.dat
                echo bin>> ftpcmd.dat
                rem Parameter is the file name of the .pof file
                echo put %1 printer1>> ftpcmd.dat
                echo quit>> ftpcmd.dat
                rem parameter is the IP address of the printer
                ftp -n -s:ftpcmd.dat %2
                del ftpcmd.dat
             * 
             * */
            // Write each line of the batch file to standard input
            sIn.WriteLine("@echo off");
            sIn.WriteLine("echo user " + ftpUser + ">ftpcmd.dat");
            sIn.WriteLine("echo " + ftpPassword + " >> ftpcmd.dat");
            sIn.WriteLine("echo bin>> ftpcmd.dat");
            sIn.WriteLine("echo put " + printLabelName + " printer1>> ftpcmd.dat");
            sIn.WriteLine("echo quit>> ftpcmd.dat");
            sIn.WriteLine("ftp -n -s:ftpcmd.dat " + defaultPrinter);
            sIn.WriteLine("del ftpcmd.dat");
            //strm.Close();
            // Exit CMD.EXE
            const string stEchoFmt = "#{0} run successfully. Exiting";
            sIn.WriteLine(String.Format(stEchoFmt, strFilePath));
            sIn.WriteLine("Exit");
            // Close the process
            proc.Close();
            // Read the sOut to a string
            string results = sOut.ReadToEnd().Trim();
            // Close the stream
            sIn.Close();
            sOut.Close();
        }

        public string PutText(string fileName, Dictionary<string, string> infos)
        {
            StreamReader sr = new StreamReader(fileName);
           
            string newFile = Path.Combine(Path.GetDirectoryName(fileName),
                Path.GetFileNameWithoutExtension(fileName) + GenerateRandomNo() + ".txt");
            StreamWriter sw = new StreamWriter(newFile);

            string line = sr.ReadLine();
            string lineToWrite = CreateFile(newFile, line, infos);
            if (!string.IsNullOrEmpty(lineToWrite))
            {
                sw.WriteLine(lineToWrite);
            }
            while (line != null)
            {
                line = sr.ReadLine();
                lineToWrite = CreateFile(newFile, line, infos);
                if (!string.IsNullOrEmpty(lineToWrite))
                {
                    sw.WriteLine(lineToWrite);
                }
            }
            sr.Close();
            sw.Close();
            return newFile;
        }

        private string CreateFile(string newFile, string line, Dictionary<string,string> infos)
        {
            if (line == null)
            {
                return string.Empty;
            }
            foreach (string s in infos.Keys)
            {
                if (line.Contains(s))
                {
                    line = line.Replace(s, infos[s]);
                }
            }
            return line;
        }

        public int GenerateRandomNo()
        {
            int _min = 0000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}