﻿using System;
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
                str.Append(System.Web.HttpUtility.UrlEncode(item.Value.ToString(), System.Text.Encoding.UTF8));
            }
            return PostQueryString(url, headers, str.ToString(), timeout);
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
            return PostJson(url, null, Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj), timeout);
        }

        public static string PostJson(string url, IDictionary<string, string> headers, object jsonObj, int timeout)
        {
            //IDictionary<string, string> headers, 
            return PostJson(url, headers, Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj), timeout);
        }

        static void checkHeaders(ref IDictionary<string, string> headers)
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            if (headers != null && headers.ContainsKey("Connection") == false)
            {
                try
                {
                    headers["Connection"] = "close";
                }
                catch
                {
                }
            }

            if (headers != null && headers.ContainsKey("User-Agent") == false)
            {
                try
                {
                    headers["User-Agent"] = "Mozilla/5.0 AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.102 Safari/537.36";
                }
                catch
                {
                }
            }
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
            checkHeaders(ref headers);

            HttpWebRequest request = WebRequest.CreateHttp(url);
            try
            {

                request.Method = "POST";
                request.Timeout = timeout;
                request.ContentType = "application/json; charset=utf-8";

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.Headers[item.Key] = item.Value;
                    }
                }
                byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;

                var task = request.GetRequestStreamAsync();
                task.Wait();
                using (var requestStream = task.Result)
                {
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Flush();

                    var taskResponse = request.GetResponseAsync();
                    taskResponse.Wait();
                    using (var responseStream = taskResponse.Result.GetResponseStream())
                    {
                        var contentType = taskResponse.Result.ContentType;//Content-Type: text/html; charset=GBK
                        var match = System.Text.RegularExpressions.Regex.Match(contentType, @"charset\=([\w|\-]+)");
                        var charsetCode = Encoding.UTF8;
                        if (match != null && !string.IsNullOrEmpty(match.Value))
                        {
                            string charset = match.Groups[1].Value;
                            try
                            {
#if NETSTANDARD2_0
                                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
                                charsetCode = Encoding.GetEncoding(charset);
                            }
                            catch
                            {

                            }
                        }

                        StreamReader sr = new StreamReader(responseStream, charsetCode);
                        var result = sr.ReadToEnd().Trim();
                        sr.Dispose();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                handleWebException(ex);
                return null;
            }
            finally
            {
                request.Abort();
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
            checkHeaders(ref headers);
            HttpWebRequest request = WebRequest.CreateHttp(url);
            try
            {

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
                using (var requestStream = task.Result)
                {
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Flush();

                    var taskResponse = request.GetResponseAsync();
                    taskResponse.Wait();
                    using (var responseStream = taskResponse.Result.GetResponseStream())
                    {

                        var contentType = taskResponse.Result.ContentType;//Content-Type: text/html; charset=GBK
                        var match = System.Text.RegularExpressions.Regex.Match(contentType, @"charset\=([\w|\-]+)");
                        var charsetCode = Encoding.UTF8;
                        if (match != null && !string.IsNullOrEmpty(match.Value))
                        {
                            string charset = match.Groups[1].Value;
                            try
                            {
#if NETSTANDARD2_0
                                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
                                charsetCode = Encoding.GetEncoding(charset);
                            }
                            catch
                            {

                            }
                        }

                        StreamReader sr = new StreamReader(responseStream, charsetCode);
                        var result = sr.ReadToEnd().Trim();
                        sr.Dispose();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                handleWebException(ex);
                return null;
            }
            finally
            {
                request.Abort();
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
#if NETSTANDARD2_0
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
                    charsetCode = Encoding.GetEncoding(charset);
                }
                catch
                {

                }
            }
            using (StreamReader sr = new StreamReader(res.GetResponseStream(), charsetCode))
            {
                var strResult = sr.ReadToEnd().Trim();
                if (strResult.Length == 0)
                    throw err;
                throw new HttpException(err.Message, res.StatusCode, strResult);
            }
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
        /// 下载文件，如果文件已下载一部分，则断点续传
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filename"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="onProgress"></param>
        public static void DownloadFile(string url,string filename, IDictionary<string, string> headers, int timeout, Action<int,long,long> onProgress)
        {
            if (headers == null)
                headers = new Dictionary<string, string>();

            FileStream fs = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write);
            headers["Range"] = "bytes=" + fs.Length + "-";
            var data = new byte[409600];
            fs.Position = fs.Length;

            int lastpercent = -1;
            HttpClient.GetStream(url, headers, 8000, (sr, length) => {
                if (length == 0)
                    return;

                long received = fs.Length;
                length += fs.Length;

                while (true)
                {
                    var readed = sr.Read(data, 0, data.Length);
                    received += readed;
                    if(onProgress != null)
                    {
                        var p = (int)(received * 100 / length);
                        if (lastpercent != p)
                        {
                            lastpercent = p;
                            onProgress(lastpercent, received, length);
                        }
                    }
                    if (readed <= 0)
                    {
                        break;
                    }
                    else
                    {
                        fs.Write(data, 0, readed);
                        if (received >= length)
                            break;
                    }
                }
                fs.Close();
            });
        }

        /// <summary>
        /// 接收二进制流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="doAction"> 二进制流 , 总长度</param>
        public static void GetStream(string url, IDictionary<string, string> headers, int timeout,Action<Stream,long> doAction)
        {
            checkHeaders(ref headers);
            HttpWebRequest request = WebRequest.CreateHttp(url);
            try
            {

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
                using (var sr = taskResponse.Result.GetResponseStream())
                {
                    doAction(sr , taskResponse.Result.ContentLength);
                }
            }
            catch (Exception ex)
            {
                handleWebException(ex);
            }
            finally
            {
                request.Abort();
            }
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
            checkHeaders(ref headers);
            HttpWebRequest request = WebRequest.CreateHttp(url);
            try
            {

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
                using (var responseStream = taskResponse.Result.GetResponseStream())
                {

                    var contentType = taskResponse.Result.ContentType;//Content-Type: text/html; charset=GBK
                    var match = System.Text.RegularExpressions.Regex.Match(contentType, @"charset\=([\w|\-]+)");
                    var charsetCode = Encoding.UTF8;
                    if (match != null && !string.IsNullOrEmpty(match.Value))
                    {
                        string charset = match.Groups[1].Value;
                        try
                        {
#if NETSTANDARD2_0
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
                            charsetCode = Encoding.GetEncoding(charset);
                        }
                        catch
                        {

                        }
                    }

                    StreamReader sr = new StreamReader(responseStream, charsetCode);
                    var result = sr.ReadToEnd().Trim();
                    sr.Dispose();

                    return result;
                }
            }
            catch (Exception ex)
            {
                handleWebException(ex);
                return null;
            }
            finally
            {
                request.Abort();
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
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                string result = string.Empty;
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                try
                {
                    wr.Headers["Connection"] = "close";

                }
                catch
                {
                }

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
                    using (Stream stream2 = wresp.GetResponseStream())
                    {
                        StreamReader reader2 = new StreamReader(stream2);

                        result = reader2.ReadToEnd();
                        reader2.Dispose();

                    }

                }
                finally
                {
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                handleWebException(ex);
                return null;
            }
            finally
            {
                wr.Abort();
            }
        }
    }
}
