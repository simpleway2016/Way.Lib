using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
#if NET46
#else
using System.Net.Http;
using System.Security.Authentication;
#endif
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    class Helper
    {


        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>Unix时间戳格式</returns>  
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        /// <summary>
        /// Unix_timeStamp to DateTime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixToDateTime(int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }


        /// <summary>
        /// 把内容进行MD5加密
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetMd5Hash(IDictionary<string, string> parameters, string key)
        {
            StringBuilder buff = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in parameters)
            {
                if (pair.Value == null)
                {
                    throw new Exception("字典内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    if (buff.Length > 0)
                        buff.Append('&');
                    buff.Append(pair.Key);
                    buff.Append('=');
                    buff.Append(pair.Value);
                }
            }
            buff.Append("&key=");
            buff.Append(key);

            //MD5加密
            using (MD5 md5Hash = MD5.Create())
            {
                var bs = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(buff.ToString()));
                var sb = new StringBuilder();
                foreach (byte b in bs)
                {
                    sb.Append(b.ToString("x2"));
                }
                //所有字符转为大写
                return sb.ToString().ToUpper();
            }

        }

        /// <summary>
        /// 以xml形式，post数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string PostXml(string url, string xml, int timeout, string ssl_path = null, string ssl_pwd = null)
        {
            if (ssl_path != null && System.IO.File.Exists(ssl_path) == false)
                throw new Exception($"can not find {ssl_path}");

            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            if (ssl_path != null)
            {
                request.ClientCertificates.Add(new X509Certificate2(ssl_path, ssl_pwd));
            }
            request.ContinueTimeout = timeout * 1000;
            request.ContentType = "text/xml";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
            request.ContentLength = data.Length;

            var task = request.GetRequestStreamAsync();
            task.Wait();
            var requestStream = task.Result;
            requestStream.Write(data, 0, data.Length);
            requestStream.Flush();

            var taskResponse = request.GetResponseAsync();
            taskResponse.Wait();
            var responseStream = taskResponse.Result.GetResponseStream();

            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            var result = sr.ReadToEnd().Trim();
            responseStream.Dispose();
            requestStream.Dispose();

            return result;



            //byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);

            //MemoryStream ms = new MemoryStream(data);
            //ms.Position = 0;
            //HttpContent content = new StreamContent(ms);
            //content.Headers.Add("Content-Type", "text/xml");
            //content.Headers.Add("Content-Length", data.Length.ToString());

            //var handler = new HttpClientHandler();
            //if (isUseCert)
            //{
            //    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            //    handler.SslProtocols = SslProtocols.Tls12;
            //    if (System.IO.File.Exists(ssl_path) == false)
            //        throw new Exception($"文件{ssl_path}不存在");
            //    handler.ClientCertificates.Add(new X509Certificate2(ssl_path, ssl_pwd));
            //}
            //var client = new HttpClient(handler);
            //var task = client.PostAsync(url, content);
            //task.Wait();

            //var task2 = task.Result.Content.ReadAsByteArrayAsync();
            //task2.Wait();

            //return System.Text.Encoding.UTF8.GetString(task2.Result);

        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");

                    string encodedValue = WebUtility.UrlEncode(value);

                    postData.Append(encodedValue);
                    hasParam = true;
                }
            }

            return postData.ToString();
        }

        public static string PostJsonString(string url, string json, int timeout)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            request.ContinueTimeout = timeout * 1000;
            request.ContentType = "application/json;charset=utf-8";
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
                    //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout">单位：秒</param>
        /// <returns></returns>
        public static string GetWebContent(string url, int timeout)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "GET";
            request.ContinueTimeout = timeout * 1000;


            var taskResponse = request.GetResponseAsync();
            taskResponse.Wait();
            var responseStream = taskResponse.Result.GetResponseStream();

            var contentType = taskResponse.Result.ContentType;//Content-Type: text/html; charset=GBK
            var match = System.Text.RegularExpressions.Regex.Match(contentType, @"charset\=([\w|\-]+)");
            var charsetCode = Encoding.UTF8;
            //if (match != null && !string.IsNullOrEmpty(match.Value))
            //{
            //    string charset = match.Groups[1].Value;
            //    try
            //    {
            //        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //        charsetCode = Encoding.GetEncoding(charset);
            //    }
            //    catch
            //    {

            //    }
            //}

            StreamReader sr = new StreamReader(responseStream, charsetCode);
            var result = sr.ReadToEnd().Trim();
            responseStream.Dispose();

            return result;
        }

        public static string PostQueryString(string url, string query, int timeout)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            request.ContinueTimeout = timeout * 1000;
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
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
                    //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sort">是否排序</param>
        /// <returns></returns>
        public static string GetUrlString(IDictionary<string, string> parameters, bool sort = true)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = sort ? new SortedDictionary<string, string>(parameters) : parameters;
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileContent"></param>
        /// <param name="formName"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType">如：image/jpeg</param>
        /// <param name="formDict"></param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, byte[] fileContent, string formName, string fileName, string contentType, SortedDictionary<string, object> formDict)
        {
            try
            {
                string result = string.Empty;
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = false;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in formDict.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, formDict[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, formName, fileName, contentType);
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

        static void handleWebException(Exception ex)
        {
            var err = ex;
            while (err.InnerException != null && !(err is WebException))
            {
                err = err.InnerException;
            }
            if (!(err is WebException))
                throw err;
            if ((err as WebException).Response == null)
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
                    //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
            throw new Exception($"http错误信息:{err.Message}\r\n服务器输出内容:{strResult}");
        }
    }
}
