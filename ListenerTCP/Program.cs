using ListenerTCP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Configuration;
using ListenerTCP.Enum;
using ListenerTCP.Helpers;
using System.Reflection;
using System.Diagnostics;

namespace ListenerTCP
{
    class Program
    {
        static void Main()
        {
            IniciarListener();
        }

        static void IniciarListener()
        {
            IPAddress ipAddress;
            IPAddress.TryParse(ConfigurationManager.AppSettings["IP"], out ipAddress);
            TcpListener listener = new TcpListener(ipAddress, Convert.ToInt32(ConfigurationManager.AppSettings["Port"]));

            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream ns = client.GetStream();

            var stopWatch = Stopwatch.StartNew();

            try
            {
                while (true)
                {
                    var data = new byte[6144];
                    int recv = ns.Read(data, 0, data.Length);
                    var msgs = Encoding.Unicode.GetString(data, 0, recv);

                    new ProcesarMensajeHelper().Procesar(msgs);

                    if (stopWatch.Elapsed >= new TimeSpan(3, 0, 0))
                    {
                        ns.Close();
                        client.Close();
                        listener.Stop();
                        Environment.Exit(-1);
                    }

                    ns.Flush();
                }
            }
            catch (Exception)
            {
                ns.Close();
                client.Close();
                listener.Stop();
                Environment.Exit(-1);
            }
        }        
    }
}
