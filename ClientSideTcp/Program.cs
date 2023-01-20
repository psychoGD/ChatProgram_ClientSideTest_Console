using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSideTcp
{
    internal class Program
    {
        public static string GetHostName()
        {
            string hostName = Dns.GetHostName();
            return hostName;
        }

        [Obsolete]
        public static string GetIpAdress(string HostName)
        {
            string IP = Dns.GetHostByName(HostName).AddressList[0].ToString();
            return IP;
        }

        [Obsolete]
        public static void StartFunc()
        {
            var client = new TcpClient();
            var ip = IPAddress.Parse(GetIpAdress(GetHostName()));
            var port = 27001;
            var ep = new IPEndPoint(ip, port);
            Console.WriteLine($"{ip} --- {port}");
            try
            {
                client.Connect(ep);
                if (client.Connected)
                {
                    var writer = Task.Run(() =>
                    {
                        while (true)
                        {
                            var text = Console.ReadLine();
                            var stream = client.GetStream();
                            var bw = new BinaryWriter(stream);
                            bw.Write(text);
                        }

                    });
                    var reader = Task.Run(() =>
                    {
                        while (true)
                        {
                            var stream = client.GetStream();
                            var br = new BinaryReader(stream);
                            Console.WriteLine($"From Server {br.ReadString()}");
                        }

                    });
                    Task.WaitAll(writer, reader);
                }
            }
            catch (Exception ex)
            {

            }
        }
        [Obsolete]
        static void Main(string[] args)
        {
            //
            StartFunc();

            

        }
    }
}
