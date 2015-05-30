namespace RestSharp.Configuration.IntegrationTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class ConfigureHelperTestBase
    {
        protected abstract string CurrentFolder { get; }
        protected abstract string BaseUrl { get; }
        protected virtual Encoding DefaultEncoding { get { return Encoding.UTF8; } }
    }
}
