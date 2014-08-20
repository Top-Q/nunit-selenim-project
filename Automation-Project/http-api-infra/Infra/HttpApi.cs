using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mysite.http_api_infra.Infra
{
    public class HttpApi
    {
        private bool saveCookies;
        private CookieCollection cookieCollection;
        private bool allowAutoRedirect = false;
        private string domain;
        private string userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.48 Safari/537.36";


        public HttpApi(string domain)
            : this(domain, true)
        {
        }

        public HttpApi(string domain, bool saveCookies)
        {
            this.domain = domain;
            this.saveCookies = saveCookies;
        }

        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }
        public string Domain
        {
            get { return domain; }
        }
        public bool AllowAutoRedirect
        {
            get { return allowAutoRedirect; }
            set { allowAutoRedirect = value; }
        }

        public Dictionary<string, string> CreateDefaultHeaders()
        {
            var headers = new Dictionary<string, string>() { 
                                                                                             { "Accept-Encoding", "gzip,deflate,sdch" }, 
                                                                                             { "Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6,he;q=0.4,ru;q=0.2"}
                                                                                    };
            return headers;
        }
        public void AddCookie(string name, string value)
        {
            if (cookieCollection == null)
            {
                cookieCollection = new CookieCollection();
            }
            Cookie cookie = new Cookie(name, value);
            cookie.Domain = domain.Replace("http://", "");
            cookieCollection.Add(cookie);
        }              

        // REQUESTS
        public HTTPResponse Get(string uri, Dictionary<string, string> headers, string contentType, string accept)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/" + uri);
            request.Timeout = 20000;

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (saveCookies && cookieCollection != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookieCollection);
            }
            request.Method = "GET";
            request.AllowAutoRedirect = allowAutoRedirect;

            request.UserAgent = userAgent;

            if (!String.IsNullOrEmpty(accept))
            {
                request.Accept = accept;
            }

            if (headers != null)
            {
                WebHeaderCollection webHeaders = new WebHeaderCollection();
                foreach (string header in headers.Keys)
                {
                    webHeaders.Add(header, headers[header]);
                }
                request.Headers = webHeaders;
            }

            if (!String.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }

            return HandleResponse((HttpWebResponse)request.GetResponse());


        }
        public HTTPResponse Post(string uri, Dictionary<string, string> headers, string contentType, string accept,string body)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + "/" + uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = 20000;

            if (saveCookies && cookieCollection != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookieCollection);
            }
            request.Method = "POST";
            request.AllowAutoRedirect = allowAutoRedirect;
 

            if (headers != null)
            {
                WebHeaderCollection webHeaders = new WebHeaderCollection();
                foreach (string header in headers.Keys)
                {
                    webHeaders.Add(header, headers[header]);
                }
                request.Headers = webHeaders;
            }

           if (!String.IsNullOrEmpty(accept))
            {
                request.Accept = accept;
            }
            if (!String.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }

            request.UserAgent = userAgent;

            byte[] bodyInBytes = System.Text.Encoding.UTF8.GetBytes(body);
            request.ContentLength = bodyInBytes.Length;

            try
            {
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(bodyInBytes, 0, bodyInBytes.Length);
                dataStream.Close();
                return HandleResponse((HttpWebResponse)request.GetResponse());
            }
            catch
            {
                throw new Exception(" There's no response stream..");
            }
        }

        private string GetResponseBody(HttpWebResponse response, bool closeResponse)
        {

            // Gets the stream associated with the response.
            Stream receiveStream = response.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");


            // FIRST ONE:
            //string s = string.Empty;
            //using ( StreamReader reader = new StreamReader( receiveStream, encode )) { s = reader.ReadToEnd(); }
            //return s;

            // SECOND OPTION:
            string s = string.Empty;
            StreamReader reader = new StreamReader(receiveStream, encode);
            try
            {
                s = reader.ReadToEnd();
            }
            catch
            {
                throw new Exception("Could not read response body");
            }
            finally
            {
                if (closeResponse)
                {
                    response.Close();
                }
                reader.Close();
            }
            return s;



            //const int BUFFER_MAX_SIZE = 65536;

            ////Char[] read = new Char[BUFFER_MAX_SIZE];
            //            // Reads 256 characters at a time.     
            
            //char[] read = null;
            //string strResult = "";
            //StringBuilder str = new StringBuilder();
            //while (readStream.Peek() > -1)
            //{
            //    read = new char[BUFFER_MAX_SIZE];
            //    readStream.Read(read, 0, read.Length);
            //    strResult = strResult + new string(read);;
            //    str.Append(read);
            //    var peek = readStream.Peek();
            //}

            //// Releases the resources of the response.
            //if (closeResponse)
            //{
            //    response.Close();
            //}

            //// Releases the resources of the Stream.
            //readStream.Close();
            //string st = str.ToString();
            //return str.ToString();
        }
        private HTTPResponse HandleResponse(HttpWebResponse response)
        {
            if (saveCookies)
            {
                if (cookieCollection == null)
                {
                    cookieCollection = new CookieCollection();
                }
                if (response.Cookies != null && response.Cookies.Count != 0)
                {
                    foreach (Cookie cookie in response.Cookies)
                    {
                        cookieCollection.Add(cookie);
                    }
                }
            }

            // HeaderCollection To Dictionary
            Dictionary<string, string> rHeaders = new Dictionary<string, string>();
            for (int i = 0; i < response.Headers.Count; ++i)
            {
                string header = response.Headers.GetKey(i);
                foreach (string value in response.Headers.GetValues(i))
                {
                    rHeaders.Add(header, value);
                }
            }

            return new HTTPResponse(rHeaders, response.ContentType, response.Cookies, (int)response.StatusCode, GetResponseBody(response, true));
        }                
    }
    
    
    public class HTTPResponse
    {
        private Dictionary<string, string> headers;
        private string contentType;
        private CookieCollection cookies;
        private int statusCode;
        private string body;

        public HTTPResponse(Dictionary<string, string> headers, string contentType, CookieCollection cookies, int statusCode, string body = "")
        {
            this.headers = headers;
            this.contentType = contentType;
            this.cookies = cookies;
            this.statusCode = statusCode;
            this.body = body;
        }

        public Dictionary<string, string> Headers
        {
            get { return headers; }
        }
        public string ContentType
        {
            get { return contentType; }
        }
        public CookieCollection Cookies
        {
            get { return cookies; }
        }
        public int StatusCode
        {
            get { return statusCode; }
        }
        public string Body
        {
            get { return body; }
        }
    }
}



