using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace WcfMobile
{
    public class RestAuthorizationManager : ServiceAuthorizationManager
    {
        public override bool CheckAccess(OperationContext operationContext)
        {
            //Extract the Authorization header, and parse out the credentials converting the Base64 string:  
            var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
            if ((authHeader != null) && (authHeader != string.Empty))
            {
                var svcCredentials = System.Text.ASCIIEncoding.ASCII
                    .GetString(Convert.FromBase64String(authHeader.Substring(6)))
                    .Split(':');
                var user = new
                {
                    Name = svcCredentials[0],
                    Password = svcCredentials[1]
                };

                string result = CheckLoginAsync(user.Name, user.Password).Result;
                if(string.IsNullOrEmpty(result))
                {
                    //User is authrized and originating call will proceed  
                    return true;
                }
                else
                {
                    throw new Exception("Authentication error. Reason: "+result);
                }
            }
            else
            {
                //No authorization header was provided, so challenge the client to provide before proceeding:  
                WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"MyWCFService\"");
                //Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }

        public async System.Threading.Tasks.Task<string> CheckLoginAsync(string uName, string uPassword)
        {
            string message=string.Empty;
            bool isSuccess = false;

            var client = new HttpClient();


            string url = ConfigurationManager.AppSettings["authURL"].ToString(); //"http://productapi.arabian-ceramics.com/api/Account/Login";
            var item = new Credent
            {
                email = uName,
                password = uPassword
            };
            var jSonData = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var needToPost = new StringContent(jSonData, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get the response.
            HttpResponseMessage response = await client.PostAsync(url, needToPost);

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.                
                string result = await reader.ReadToEndAsync();

                var objects = JObject.Parse(result);

                foreach (JProperty p in objects.Properties())
                {
                    string name = p.Name;
                    string value = p.Value.ToString();
                    //Console.Write(name + ": " + value);

                    if (name.Equals("message"))
                        message = p.Value.ToString();
                    else if (name.Equals("isSuccess"))
                        isSuccess = bool.Parse(p.Value.ToString());
                }

            }

            return message;
        }

        public class Credent
        {
            public string email { get; set; }
            public string password { get; set; }
        }
    }
}