namespace RestSharp.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Model;
    using Newtonsoft.Json;

    public class ConfigureHelper
    {
        private static readonly IList<string> IgnoredHeaderString = new List<string>
        {
            "Connection"
        };

        public static IList<RestRequest> SetConfigure(string configFile)
        {
            IList<RestRequest> listRestRequest = new List<RestRequest>();

            using (StreamReader stream = new StreamReader(configFile))
            {
                JsonTextReader reader = new JsonTextReader(stream);
                JsonSerializer se = new JsonSerializer();

                Har har = se.Deserialize<Har>(reader);

                foreach (var entry in har.Log.Entries)
                {
                    var configuredRequest = entry.Request;
                    var restRequest = SetNewRequest(configuredRequest);

                    listRestRequest.Add(restRequest);
                }
            }

            return listRestRequest;
        }

        private static RestRequest SetNewRequest(Request configuredRequest)
        {
            var restRequest = new RestRequest();

            SetMethod(restRequest, configuredRequest);
            SetUrl(restRequest, configuredRequest);
            AddHeaders(restRequest, configuredRequest);
            AddQueryParameters(restRequest, configuredRequest);
            AddParams(restRequest, configuredRequest);

            return restRequest;
        }

        private static void AddHeaders(RestRequest restRequest, Request request)
        {
            foreach (var head in request.Headers)
            {
                if (!IgnoredHeaderString.Contains(head.Name))
                {
                    restRequest.AddHeader(ConvertValue(head.Name), ConvertValue(head.Value.ToString()));
                }
            }
        }

        private static void AddQueryParameters(RestRequest restRequest, Request request)
        {
            foreach (var queryString in request.QueryString)
            {
                restRequest.AddQueryParameter(ConvertValue(queryString.Name), ConvertValue(queryString.Value.ToString()));
            }
        }

        private static void AddParams(RestRequest restRequest, Request request)
        {
            foreach (var parameter in request.PostData.Params)
            {
                restRequest.AddParameter(ConvertValue(parameter.Name), ConvertValue(parameter.Value.ToString()));
            }
        }

        private static void SetUrl(RestRequest restRequest, Request request)
        {
            Uri uri = new Uri(request.Url);

            string removedFirstSlash = uri.AbsolutePath.Remove(0, 1);

            restRequest.Resource = removedFirstSlash;
        }

        private static void SetMethod(RestRequest restRequest, Request request)
        {
            restRequest.Method = (Method)Enum.Parse(typeof(Method), request.Method, true);
        }

        private static string ConvertValue(string value)
        {
            return HttpUtility.UrlDecode(value);
        }
    }
}
