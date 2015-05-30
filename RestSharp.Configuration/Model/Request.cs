namespace RestSharp.Configuration.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Request
    {
        public string Method { get; set; }

        public string Url { get; set; }

        public IList<Parameter> Headers { get; set; }

        public IList<Parameter> QueryString { get; set; }

        public IList<Parameter> Cookies { get; set; }

        public PostData PostData { get; set; }
    }
}
