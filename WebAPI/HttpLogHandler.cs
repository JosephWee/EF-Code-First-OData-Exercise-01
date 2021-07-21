using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebAPI
{
    /// <summary>
    /// For more information please see:
    /// https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpmethod?view=net-5.0
    /// https://docs.microsoft.com/en-us/dotnet/api/system.uri?view=net-5.0
    /// https://docs.microsoft.com/en-us/dotnet/api/system.net.http.headers.httpheaders?view=net-5.0
    /// https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent?view=net-5.0
    /// </summary>
    [Serializable]
    public class LogEntry
    {
        protected LogEntry()
        {
            EntryId = Guid.NewGuid();
            RequestHeaders = new Dictionary<string, List<string>>();
            RequestContentHeaders = new Dictionary<string, List<string>>();
            RequestContent = string.Empty;
            ResponseHeaders = new Dictionary<string, List<string>>();
            ResponseContentHeaders = new Dictionary<string, List<string>>();
            ResponseContent = string.Empty;
        }

        public LogEntry(HttpRequestMessage httpRequestMessage)
            : this()
        {
            Method = httpRequestMessage.Method.ToString();
            URI = httpRequestMessage.RequestUri;
            AppendToHeaderDictionary(RequestHeaders, httpRequestMessage.Headers);
            AppendHttpContent(httpRequestMessage).Wait();
        }

        public void AppendHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            ResponseHeaders = new Dictionary<string, List<string>>();
            AppendToHeaderDictionary(ResponseHeaders, httpResponseMessage.Headers);
            AppendHttpContent(httpResponseMessage).Wait();
        }

        protected void AppendToHeaderDictionary(Dictionary<string, List<string>> headersDictionary, System.Net.Http.Headers.HttpHeaders httpHeaders)
        {
            foreach (var item in httpHeaders)
            {
                if (headersDictionary.ContainsKey(item.Key))
                    headersDictionary[item.Key].AddRange(item.Value.ToList());
                else
                    headersDictionary.Add(item.Key, item.Value.ToList());                
            }
        }

        protected async Task AppendHttpContent(HttpRequestMessage httpRequestMessage)
        {
            AppendToHeaderDictionary(RequestContentHeaders, httpRequestMessage.Content.Headers);
            RequestContent = await GetHttpContent(httpRequestMessage.Content);
        }

        protected async Task AppendHttpContent(HttpResponseMessage httpResponseMessage)
        {
            AppendToHeaderDictionary(ResponseContentHeaders, httpResponseMessage.Content.Headers);
            //ResponseContent = await GetHttpContent(httpResponseMessage.Content);
        }

        /// <summary>
        /// For more information please read:
        /// https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent?view=net-5.0
        /// </summary>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        protected async Task<string> GetHttpContent(System.Net.Http.HttpContent httpContent)
        {
            StringBuilder builder = new StringBuilder();

            bool appended = false;
            try
            {
                var httpContentString = await httpContent.ReadAsStringAsync();
                builder.Append(httpContentString);
                appended = true;
            }
            catch (Exception ex)
            {
            }

            if (!appended)
            {
                try
                {
                    var httpContentByteArray = await httpContent.ReadAsByteArrayAsync();
                    if (httpContentByteArray != null && httpContentByteArray.Length > 0)
                    {
                        for (int b = 0; b < httpContentByteArray.Length; b++)
                        {
                            string byteString = string.Format("{0:X2} ", httpContentByteArray[b]);
                            builder.Append(byteString);
                        }
                        appended = true;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            if (!appended)
                builder.Append("Content cannot be read as string or byte array");

            return builder.ToString();
        }

        public Guid EntryId { get; set; }
        public string Method { get; set; }
        public Uri URI { get; set; }
        public Dictionary<string, List<string>> RequestHeaders { get; set; }
        public Dictionary<string, List<string>> RequestContentHeaders { get; set; }
        public string RequestContent { get; set; }
        public Dictionary<string, List<string>> ResponseHeaders { get; set; }
        public Dictionary<string, List<string>> ResponseContentHeaders { get; set; }
        public string ResponseContent { get; set; }
        public string Exception { get; set; }
    }

    /// <summary>
    /// For more information please see:
    /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/http-message-handlers
    /// </summary>
    public class HttpLogHandler : DelegatingHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LogEntry entry = new LogEntry(request);

            HttpResponseMessage response = null;
            try
            {
                // Call the inner handler.
                response = await base.SendAsync(request, cancellationToken);
                entry.AppendHttpResponse(response);
            }
            catch (Exception ex)
            {
                entry.Exception = ex.ToString();
            }
            finally
            {
                string json = JsonConvert.SerializeObject(entry, Formatting.Indented);
                log.Debug(json);
            }

            return response;
        }
    }
}