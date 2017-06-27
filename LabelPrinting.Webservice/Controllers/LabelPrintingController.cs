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
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using ImageMagick;
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
        [ActionName("getPrinterLabels")]
        // GET api/<controller>
        public IHttpActionResult GetPrinterLabels(string id)
        {
            return Ok(PrintingLabels.Labels.Where(item => item.PrinterId == id));
        }


        /// <summary>
        /// Add printer, just post to api url
        /// </summary>
        /// <param name="printerLabel"></param>
        [HttpPost]
        [ActionName("print")]
        public HttpResponseMessage Print([FromBody] PrintingLabel printerLabel)
        {
            MakePrint(printerLabel);
            return Request.CreateResponse(HttpStatusCode.OK, "success");
            // return Ok(true);
        }

        public void MakePrint(PrintingLabel printerLabel)
        {
           
            // Get the path of labels
            string pathToLabelFiles = ConfigurationManager.AppSettings["labelPath"];
            string strFilePath = pathToLabelFiles + "print.bat";
            Dictionary<string, string> infos = new Dictionary<string, string>();
            infos.Add("%TextToPrint%", printerLabel.TextToPrint);
            infos.Add("%MaterialInfo%", printerLabel.MaterialInfo);
            //if (!string.IsNullOrEmpty(printerLabel.TextToPrint))
            //{
            //    infos.Add("%TextToPrint%", printerLabel.TextToPrint);
            //    printerLabel.LabelFileName = "a3.txt";
            //}
            //else
            //{
            // //   infos.Add("%ImageToPrint%", printerLabel.ImageToPrint);
            //    infos.Add("%MaterialInfo%", printerLabel.MaterialInfo);
            //}
           
          
            //printerLabel.ImageToPrint = printerLabel.ImageToPrint.Substring(printerLabel.ImageToPrint.IndexOf(",", StringComparison.Ordinal) + 1);
            //byte[] ff = Convert.FromBase64String(printerLabel.ImageToPrint);
            //MagickImage img = new MagickImage(ff);
            //img.Format = MagickFormat.Pcx;
            //MemoryStream m = new MemoryStream();
            //img.Write(m);
            //using (StreamReader reader = new StreamReader(m))
            //{
            //    m.Position = 0;
            //    string newFile = Path.Combine(Path.GetDirectoryName(Path.Combine(pathToLabelFiles, printerLabel.LabelFileName)),
            //    Path.GetFileNameWithoutExtension(Path.Combine(pathToLabelFiles, printerLabel.LabelFileName)) + GenerateRandomNo() + ".txt");
            //    StreamWriter sw = new StreamWriter(newFile);
            //    sw.WriteLine(reader.ReadToEnd());
            //}

          
            string printLabelName = PutText(Path.Combine(pathToLabelFiles, printerLabel.LabelFileName), infos);
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

            File.Delete(printLabelName);
        }

        public string PutText(string fileName, Dictionary<string, string> infos)
        {
            StreamReader sr = new StreamReader(fileName, Encoding.ASCII);
           
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

        public string PutAllText(string fileName, Dictionary<string, string> infos)
        {
            string newFile = Path.Combine(Path.GetDirectoryName(fileName),
              Path.GetFileNameWithoutExtension(fileName) + GenerateRandomNo() + ".txt");

            FileInfo inn = new FileInfo(fileName);
            inn.CopyTo(newFile);


            //string initial = File.ReadAllText(newFile);
            //foreach (string s in infos.Keys)
            //{
            //    if (initial.Contains(s))
            //    {
            //        initial = initial.Replace(s, infos[s]);
            //    }
            //}

            File.WriteAllText(newFile, infos["%TextToPrint%"]);
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