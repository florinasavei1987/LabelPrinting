using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
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

        //// PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public static void SendFtpCommand()
        {
            var serverName = "[FTP_SERVER_NAME]";
            var port = 21;
            var userName = "";
            var password = "";
            var command = @"PUT C:\temp";

            var tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(serverName, port);
                Flush(tcpClient);

                var response = TransmitCommand(tcpClient, "user " + userName);
                if (response.IndexOf("331", StringComparison.OrdinalIgnoreCase) < 0)
                    throw new Exception(string.Format("Error \"{0}\" while sending user name \"{1}\".", response,
                        userName));

                response = TransmitCommand(tcpClient, "pass " + password);
                if (response.IndexOf("230", StringComparison.OrdinalIgnoreCase) < 0)
                    throw new Exception(string.Format("Error \"{0}\" while sending password.", response));

                response = TransmitCommand(tcpClient, command);
                if (response.IndexOf("200", StringComparison.OrdinalIgnoreCase) < 0)
                    throw new Exception(string.Format("Error \"{0}\" while sending command \"{1}\".", response, command));
            }
            finally
            {
                if (tcpClient.Connected)
                    tcpClient.Close();
            }
        }

        private static string TransmitCommand(TcpClient tcpClient, string cmd)
        {
            var networkStream = tcpClient.GetStream();
            if (!networkStream.CanWrite || !networkStream.CanRead)
                return string.Empty;

            var sendBytes = Encoding.ASCII.GetBytes(cmd + "\r\n");
            networkStream.Write(sendBytes, 0, sendBytes.Length);

            var streamReader = new StreamReader(networkStream);
            return streamReader.ReadLine();
        }

        private static string Flush(TcpClient tcpClient)
        {
            try
            {
                var networkStream = tcpClient.GetStream();
                if (!networkStream.CanWrite || !networkStream.CanRead)
                    return string.Empty;

                var receiveBytes = new byte[tcpClient.ReceiveBufferSize];
                networkStream.ReadTimeout = 10000;
                networkStream.Read(receiveBytes, 0, tcpClient.ReceiveBufferSize);

                return Encoding.ASCII.GetString(receiveBytes);
            }
            catch
            {
                // Ignore all irrelevant exceptions
            }

            return string.Empty;
        }
    }
}