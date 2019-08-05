using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class HttpClientHelper
    {
        public HttpClientHelper()
        {

        }
        public async Task<string> Get(string URL)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            var result = "";


            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(URL);

            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }
        public string Get(string URL, Dictionary<string, string> header)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            var result = "";


            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(URL);

            request.Method = "GET";
            foreach (var head in header)
            {

                request.Headers.Add(head.Key, head.Value);

            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        public static string Post(string url, string data, Dictionary<string, string> header)
        {
            var result = "";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";

            foreach (var head in header)
            {

                httpWebRequest.Headers.Add(head.Key, head.Value);

            }

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = data;

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;

        }

        public async Task<string> Post(string url, string data)
        {
            var result = "";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";

            try
            {

                httpWebRequest.Headers.Add("Content-Type", "application/json");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = data;

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                var ex2 = ex.Message.ToString();
            }
            return result;

        }

        public static string Put(string url, string data, Dictionary<string, string> header)
        {
            var result = "";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "PUT";

            foreach (var head in header)
            {

                httpWebRequest.Headers.Add(head.Key, head.Value);

            }

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = data;

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;

        }
    }
}
