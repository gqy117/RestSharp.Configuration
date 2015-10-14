namespace RestSharp.Configuration.IntegrationTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ConfigureHelperNZVisaTest : ConfigureHelperTestBase
    {
        #region Properties
        protected override string CurrentFolder
        {
            get { return "ConfigureHelperIntegrationTest\\NZVisa\\"; }
        }

        protected override string BaseUrl
        {
            get { return "https://www.visaservices.co.in"; }
        }

        private string MockHarFile
        {
            get { return this.CurrentFolder + "NZVisa.har"; }
        }

        private string NZVisaHtml
        {
            get { return this.CurrentFolder + "NZVisa.html"; }
        }

        private ConfigureHelper ConfigureHelper { get; set; }

        #endregion

        [TestFixtureSetUp]
        public void Init()
        {
            this.ConfigureHelper = new ConfigureHelper();
        }

        [Test]
        public void SetConfigure_ShouldReturnRestRequestWhichCanGetResponseFromTheInternet_WhenTheInputedRestRequestIsNotNull()
        {
            // Arrange
            var restClient = this.ConfigureHelper.CreateDefaultRestRequest(this.BaseUrl);

            // Act
            RestRequest request = this.ConfigureHelper.SetConfigureByHar(this.MockHarFile);
            var actual = restClient.Execute(request);

            // Assert
            using (StreamReader sr = new StreamReader(this.NZVisaHtml, this.DefaultEncoding))
            {
                var expectedHtmlString = sr.ReadToEnd();

                actual.Content.ShouldBeEquivalentTo(expectedHtmlString);
            }
        }
    }
}
