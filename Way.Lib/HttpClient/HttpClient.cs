using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Way.Lib
{
    public class HttpClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="timeout">超时时间，单位:毫秒</param>
        /// <returns></returns>
        public static string PostQueryString(string url, IDictionary<string, object> query, int timeout)
        {
            if (query == null)
            {
                return PostQueryString(url, "", timeout);
            }
            StringBuilder str = new StringBuilder();
            foreach (var item in query)
            {
                if (str.Length > 0)
                    str.Append('&');
                str.Append(item.Key);
                str.Append("=");
                str.Append(item.Value);
            }
            return PostQueryString(url, str.ToString(), timeout);
        }

        public static string PostQueryString(string url, IDictionary<string, string> headers, IDictionary<string, object> query, int timeout)
        {
            if (query == null)
            {
                return PostQueryString(url, "", timeout);
            }
            StringBuilder str = new StringBuilder();
            foreach (var item in query)
            {
                if (str.Length > 0)
                    str.Append('&');
                str.Append(item.Key);
                str.Append("=");
                str.Append(item.Value);
            }
            return PostQueryString(url, headers,str.ToString(), timeout);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="timeout">超时时间，单位:毫秒</param>
        /// <returns></returns>
        public static string PostQueryString(string url, IDictionary<string, string> query, int timeout)
        {
            StringBuilder str = new StringBuilder();
            foreach (var item in query)
            {
                if (str.Length > 0)
                    str.Append('&');
                str.Append(item.Key);
                str.Append("=");
                str.Append(System.Web.HttpUtility.UrlEncode(item.Value, System.Text.Encoding.UTF8));
            }
            return PostQueryString(url, str.ToString(), timeout);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="timeout">超时时间，单位:毫秒</param>
        /// <returns></returns>
        public static string PostQueryString(string url, string query, int timeout)
        {
            return PostQueryString(url, null, query, timeout);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonObj"></param>
        /// <param name="timeout">超时时间，单位:毫秒</param>
        /// <returns></returns>
        public static string PostJson(string url, object jsonObj, int timeout)
        {
            //IDictionary<string, string> headers, 
            return PostJson(url,null, Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj), timeout);
        }

        public static string PostJson(string url, IDictionary<string, string> headers, object jsonObj, int timeout)
        {
            //IDictionary<string, string> headers, 
            return PostJson(url, headers, Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj), timeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="json"></param>
        /// <param name="timeout">超时时间，单位:毫秒</param>
        /// <returns></returns>
        public static string PostJson(string url, IDictionary<string, string> headers, string json, int timeout)
        {
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "POST";
                request.Timeout = timeout;
                request.ContentType = "application/json; charset=utf-8";
                if(headers != null)
                {
                    foreach( var item in headers )
                    {
                        request.Headers[item.Key] = item.Value;
                    }
                }
                byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;

                var task = request.GetRequestStreamAsync();
                task.Wait();
                var requestStream = task.Result;
                requestStream.Write(data, 0, data.Length);
                requestStream.Flush();

                var taskResponse = request.GetResponseAsync();
                taskResponse.Wait();
                var responseStream = taskResponse.Result.GetResponseStream();

                var contentType = taskResponse.Result.ContentType;//Content-Type: text/html; charset=GBK
                var match = System.Text.RegularExpressions.Regex.Match(contentType, @"charset\=([\w|\-]+)");
                var charsetCode = Encoding.UTF8;
                if (match != null && !string.IsNullOrEmpty(match.Value))
                {
                    string charset = match.Groups[1].Value;
                    try
                    {
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        charsetCode = Encoding.GetEncoding(charset);
                    }
                    catch
                    {

                    }
                }

                StreamReader sr = new StreamReader(responseStream, charsetCode);
                var result = sr.ReadToEnd().Trim();
                responseStream.Dispose();
                requestStream.Dispose();

                return result;
            }
            catch (Exception ex)
            {
                handleWebException(ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="query"></param>
        /// <param name="timeout">超时时间，单位:毫秒</param>
        /// <returns></returns>
        public static string PostQueryString(string url, IDictionary<string, string> headers, string query, int timeout)
        {
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "POST";
                request.Timeout = timeout;
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.Headers[item.Key] = item.Value;
                    }
                }
                byte[] data = System.Text.Encoding.UTF8.GetBytes(query);
                request.ContentLength = data.Length;

                var task = request.GetRequestStreamAsync();
                task.Wait();
                var requestStream = task.Result;
                requestStream.Write(data, 0, data.Length);
                requestStream.Flush();

                var taskResponse = request.GetResponseAsync();
                taskResponse.Wait();
                var responseStream = taskResponse.Result.GetResponseStream();

                var contentType = taskResponse.Result.ContentType;//Content-Type: text/html; charset=GBK
                var match = System.Text.RegularExpressions.Regex.Match(contentType, @"charset\=([\w|\-]+)");
                var charsetCode = Encoding.UTF8;
                if (match != null && !string.IsNullOrEmpty(match.Value))
                {
                    string charset = match.Groups[1].Value;
                    try
                    {
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        charsetCode = Encoding.GetEncoding(charset);
                    }
                    catch
                    {

                    }
                }

                StreamReader sr = new StreamReader(responseStream, charsetCode);
                var result = sr.ReadToEnd().Trim();
                responseStream.Dispose();
                requestStream.Dispose();

                return result;
            }
            catch (Exception ex)
            {
                handleWebException(ex);
                return null;
            }
        }

        static void handleWebException(Exception ex)
        {
            var err = ex;
            while (err.InnerException != null && !(err is WebException))
            {
                err = err.InnerException;
            }
            if (!(err is WebException) || (err as WebException).Response == null)
                throw err;

            var res = (HttpWebResponse)(err as WebException).Response;
            var contentType = res.ContentType;//Content-Type: text/html; charset=GBK
            var match = System.Text.RegularExpressions.Regex.Match(contentType, @"charset\=([\w|\-]+)");
            var charsetCode = Encoding.UTF8;
            if (match != null && !string.IsNullOrEmpty(match.Value))
            {
                string charset = match.Groups[1].Value;
                try
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    charsetCode = Encoding.GetEncoding(charset);
                }
                catch
                {

                }
            }
            StreamReader sr = new StreamReader(res.GetResponseStream(), charsetCode);
            var strResult = sr.ReadToEnd().Trim();
            if (strResult.Length == 0)
                throw err;
            throw new HttpException(err.Message , res.StatusCode , strResult);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout">超时时间，单位:毫秒</param>
        /// <returns></returns>
        public static string GetContent(string url, int timeout)
        {
            return GetContent(url, null, timeout);
        }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="url"></param>
            /// <param name="headers"></param>
            /// <param name="timeout">超时时间，单位:毫秒</param>
            /// <returns></returns>
            public static string GetContent(string url, IDictionary<string, string> headers, int timeout)
        {
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "GET";
                request.Timeout = timeout;
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.Headers[item.Key] = item.Value;
                    }
                }
                var taskResponse = request.GetResponseAsync();
                taskResponse.Wait();
                var responseStream = taskResponse.Result.GetResponseStream();

                var contentType = taskResponse.Result.ContentType;//Content-Type: text/html; charset=GBK
                var match = System.Text.RegularExpressions.Regex.Match(contentType, @"charset\=([\w|\-]+)");
                var charsetCode = Encoding.UTF8;
                if (match != null && !string.IsNullOrEmpty(match.Value))
                {
                    string charset = match.Groups[1].Value;
                    try
                    {
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        charsetCode = Encoding.GetEncoding(charset);
                    }
                    catch
                    {

                    }
                }

                StreamReader sr = new StreamReader(responseStream, charsetCode);
                var result = sr.ReadToEnd().Trim();
                responseStream.Dispose();

                return result;
            }
            catch (Exception ex)
            {
                handleWebException(ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileContent"></param>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType">如：image/jpeg</param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static string UploadFile(string url, byte[] fileContent, string name, string fileName, string contentType, Dictionary<string, string> others)
        {
            try
            {
                string result = string.Empty;
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in others.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, others[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, name, fileName, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                rs.Write(fileContent, 0, fileContent.Length);

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                WebResponse wresp = null;
                try
                {
                    wresp = wr.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);

                    result = reader2.ReadToEnd();
                }
                catch (Exception ex)
                {
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                }
                finally
                {
                    wr = null;
                }

                return result;
            }
            catch (Exception ex)
            {
                handleWebException(ex);
                return null;
            }
        }
    }
}
