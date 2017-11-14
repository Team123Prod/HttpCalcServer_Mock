using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpCalcServer
{
    public class HttpServerMock
    {
        HttpListener listener;

        public HttpServerMock(string uri)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(uri);
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    Receiver(context);
                }
                catch (Exception e)
                {

                }
            }

        }

        private void Receiver(HttpListenerContext context)
        {
            string a = "";
            string b = "";
            string op = "";

            if (context.Request.HttpMethod == "POST")
            {
                string[] param = new StreamReader(context.Request.InputStream).ReadToEnd().Split('=', '&');
                a = param[1];
                b = param[3];
                op = param[5];
            }
            else
            {
                a = context.Request.QueryString["a"];
                b = context.Request.QueryString["b"];
                op = context.Request.QueryString["op"];
            }

            int n1 = Convert.ToInt32(a);
            int n2 = Convert.ToInt32(b);
            if (string.Equals(op, "p"))
                op = "+";

            int res = CalcMock(n1, n2, op);
            string data = res.ToString();

            context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.ContentLength64 = data.Length;
            context.Response.OutputStream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            context.Response.OutputStream.Close();
        }
        
        public static int CalcMock(int a, int b, string op)
        {
            return 10;
        }
    }
}
