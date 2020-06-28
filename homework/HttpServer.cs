using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.Runtime.InteropServices.ComTypes;

namespace homework
{
    class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static string path = "storage.json";

        public static async Task HandleIncomingConnections()
        {

            // run server
            while (true)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse res = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("GOT CONNECTION FROM: "+req.UserHostAddress);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                // Write the response info
                byte[] buffer;
                res.ContentType = "application/json";
                res.ContentEncoding = Encoding.UTF8;
                List<Response> response = new List<Response>();
                solver solver = new solver(path);

                if (req.HttpMethod.Equals("POST"))
                {
                    StreamReader sr = new StreamReader(req.InputStream, req.ContentEncoding);
                    List<int[]> data = new List<int[]>();
                    try
                    {
                        data = JsonConvert.DeserializeObject<List<int[]>>(sr.ReadToEnd());
                    }
                    catch (Exception exc)
                    {
                        buffer = Encoding.ASCII.GetBytes("Wrong input format. Should be: [ [1, 2, 3], [4, 5, 6], ... ]");
                        res.ContentLength64 = buffer.LongLength;
                        await res.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                        res.Close();
                        continue;
                    }

                    solver.populate(data);
                    solver.Solve();
                    response = solver.Output;
                }
                else if (req.HttpMethod.Equals("GET"))
                {
                    string from = req.QueryString["from"];
                    string count = req.QueryString["count"];
                    List<Response> storage = solver.Storage;

                    if (from != null && count != null)
                    {
                        response = storage.GetRange(Int32.Parse(from), Int32.Parse(count));
                    }
                    else
                    {
                        response = storage;
                    }
                }

                string json = JsonConvert.SerializeObject(response, Formatting.Indented);
                buffer = Encoding.ASCII.GetBytes(json);
                res.ContentLength64 = buffer.LongLength;
                await res.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                res.Close();
            }
        }


        public static void Main(string[] args)
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();
            // Close the listener
            listener.Close();
        }
    }
}