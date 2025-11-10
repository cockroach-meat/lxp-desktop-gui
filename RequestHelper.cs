using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace LxpAPI {
    class LxpException : Exception {
        public string ErrorText { get; set; }
        public LxpException(string errorText) : base() {
            ErrorText = errorText;
        }
    }

    class RequestHelper {
        static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        static readonly string url = "https://api.newlxp.ru/graphql";

        public static T MakeAPIRequest<T>(string token, string body){
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + token);
            
            using(var stream = request.GetRequestStream()){
                var bytes = Encoding.UTF8.GetBytes(body);
                stream.Write(bytes, 0, bytes.Length);
            }

            string content = string.Empty;

            try{
                using(var response = request.GetResponse()){
                    using(var stream = response.GetResponseStream()){
                        using(var reader = new StreamReader(stream)){
                            content = reader.ReadToEnd();
                        }
                    }
                }
            }catch(WebException ex){
                using(var response = ex.Response){
                    using(var stream = response.GetResponseStream()){
                        using(var reader = new StreamReader(stream)){
                            content = reader.ReadToEnd();
                        }
                    }
                }
            }

            if(content.StartsWith("{\"errors\":")){
                throw new LxpException(content);
            }

            return serializer.Deserialize<T>(content);
        }
    }
}