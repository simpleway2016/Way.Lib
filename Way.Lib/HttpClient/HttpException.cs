using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Way.Lib
{
    public class HttpException:Exception
    {

        private HttpStatusCode _StatusCode;
        public HttpStatusCode StatusCode
        {
            get => _StatusCode;
        }


        private string _ResponseBody;
        public string ResponseBody
        {
            get => _ResponseBody;
        }

        public HttpException(string message , HttpStatusCode statusCode , string responseBody):base(message)
        {
            _ResponseBody = responseBody;
            _StatusCode = statusCode;
        }
    }
}
