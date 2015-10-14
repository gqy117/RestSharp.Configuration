namespace RestSharp.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
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

        public RestClient CreateDefaultRestClient(string baseUrl)
        {
            RestClient restClient = new RestClient(baseUrl)
            {
                CookieContainer = new CookieContainer()
            };

            return restClient;
        }

        public RestRequest SetConfigureByHar(string harConfigFile)
        {
            RestRequest restRequest = new RestRequest();

            using (StreamReader stream = new StreamReader(harConfigFile))
            {
                JsonTextReader reader = new JsonTextReader(stream);
                JsonSerializer se = new JsonSerializer();

                Har har = se.Deserialize<Har>(reader);

                var entry = har.Log.Entries.First();

                var configuredRequest = entry.Request;
                restRequest = this.SetRequest(configuredRequest);
            }

            return restRequest;
        }

        private RestRequest SetRequest(Request configuredRequest)
        {
            var restRequest = new RestRequest();

            this.SetMethod(restRequest, configuredRequest);
            this.SetUrl(restRequest, configuredRequest);
            this.AddHeaders(restRequest, configuredRequest);
            this.AddQueryParameters(restRequest, configuredRequest);
            this.AddCookies(restRequest, configuredRequest);
            this.AddParams(restRequest, configuredRequest);

            return restRequest;
        }

        private void AddHeaders(RestRequest restRequest, Request request)
        {
            if (request.Headers != null)
            {
                foreach (var head in request.Headers)
                {
                    if (!IgnoredHeaderString.Contains(head.Name))
                    {
                        restRequest.AddHeader(this.UrlDecode(head.Name), this.UrlDecode(head.Value.ToString()));
                    }
                }
            }
        }

        private void AddQueryParameters(RestRequest restRequest, Request request)
        {
            if (request.QueryString != null)
            {
                foreach (var queryString in request.QueryString)
                {
                    restRequest.AddQueryParameter(this.UrlDecode(queryString.Name), this.UrlDecode(queryString.Value.ToString()));
                }
            }
        }

        private void AddCookies(RestRequest restRequest, Request request)
        {
            if (request.Cookies != null)
            {
                foreach (var cookie in request.Cookies)
                {
                    restRequest.AddCookie(this.UrlDecode(cookie.Name), this.UrlDecode(cookie.Value.ToString()));
                }
            }
        }

        private void AddParams(RestRequest restRequest, Request request)
        {
            if (request.PostData != null)
            {
                foreach (var parameter in request.PostData.Params)
                {
                    restRequest.AddParameter(this.UrlDecode(parameter.Name), this.UrlDecode(parameter.Value.ToString()));
                }
            }
        }

        private void SetUrl(RestRequest restRequest, Request request)
        {
            Uri uri = new Uri(request.Url);

            string removedFirstSlash = uri.AbsolutePath.Remove(0, 1);

            restRequest.Resource = removedFirstSlash;
        }

        private void SetMethod(RestRequest restRequest, Request request)
        {
            restRequest.Method = (Method)Enum.Parse(typeof(Method), request.Method, true);
        }

        private string UrlDecode(string value)
        {
            return HttpUtility.UrlDecode(value);
        }
    }
}