namespace RestSharp.Configuration.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ConfigureHelperTest
    {
        private const string MockHarFile = "NZVisa.har";

        [Test]
        public void SetConfigure_ShouldReturnRestRequestWithAddedParameters()
        {
            // Arrange

            // Act
            IList<RestRequest> actual = ConfigureHelper.SetConfigure(MockHarFile);

            // Assert
            RestRequest expected = new RestRequest();

            expected.Method = Method.POST;

            expected.Resource = "NewZealand-China-Tracking/TrackingParam.aspx";

            expected.AddHeader("Origin", "https://www.visaservices.co.in");
            expected.AddHeader("Accept-Encoding", "gzip, deflate");
            expected.AddHeader("Host", "www.visaservices.co.in");
            expected.AddHeader("Accept-Language", "en-US,en;q=0.8,en-GB;q=0.6");
            expected.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.65 Safari/537.36");
            expected.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            expected.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            expected.AddHeader("Cache-Control", "max-age=0");
            expected.AddHeader("Referer", "https://www.visaservices.co.in/NewZealand-China-Tracking/TrackingParam.aspx?P=97xk/KIMCwXwingXE7Nh9JSeVyehyM4reTUxQKqWABw=");
            expected.AddHeader("Content-Length", "884");

            expected.AddQueryParameter("P", "97xk/KIMCwXwingXE7Nh9JSeVyehyM4reTUxQKqWABw=");

            expected.AddParameter("__LASTFOCUS", string.Empty);
            expected.AddParameter("__EVENTTARGET", string.Empty);
            expected.AddParameter("__EVENTARGUMENT", string.Empty);
            expected.AddParameter("__VIEWSTATE", "xVIyLw/14SOabyBGZ8+qSSYvUzLiG3uEoyqHekHBuXwPMkG0tQe7WggG+fZ5VXmoHZCNZranOKtVyfu9QEpAyXQg0Vl/r1frQpyviyEYdiysQu3kGkQWNBswphuik9ikc3GWUSv0SNwzKLGbJ+5Tcqc9oWITtR6Wi7xTPdDQQtsF+eUDoz2gkWL6tQGXE/LmBcnCkuJv3Es633iZt2GCp+U3vhexwiBJdOWnwGSARoXVXuADnJMlchRcsGjThVnh82RmFOjMLvPIfyPbFL2yF/ebanBiWMBV1uZoq026xyKVmUQOOHjye4g8ryvR1l2YHo8JQejTSAaXm/hz43QH7I+KcLemHU7EbkzofngfyCaCa1sQh1Xh1Ko+Uk77L6kh+crarzeXXJ2JMBy7JG89hpKDGohq0ZXe9YlDnUjpMIC3lZHKLe/HNwFx1GYnLAjnjTKA1K4Ju/fVE9bPtj+ZT19NxrquXQ9uUDnybSGM8mg=");
            expected.AddParameter("ctl00$CPH$txtR2Part1", "BEAC");
            expected.AddParameter("ctl00$CPH$txtR2Part2", "260515");
            expected.AddParameter("ctl00$CPH$txtR2Part3", "0134");
            expected.AddParameter("ctl00$CPH$txtDOB$txtDate", "07/08/1986");
            expected.AddParameter("ctl00$CPH$btnDOB", "提交");
            expected.AddParameter("__EVENTVALIDATION", "ZwWQzZhIU5bnUvHoAA6eGo22rvmMQLtFslZ435qF8gTk64gjpYH9a/L6/HZB24CV1c5qOVE/+pPNntYYrHLzgalrFmDOrkyQ");

            actual.FirstOrDefault().ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void SetConfigure_ShouldReturnRestRequestWhichCanGetResponseFromTheInternet_WhenTheInputedRestRequestIsNotNull()
        {
            // Arrange
            RestClient restClient = new RestClient("https://www.visaservices.co.in");

            // Act
            IList<RestRequest> request = ConfigureHelper.SetConfigure(MockHarFile);
            var actual = restClient.Execute(request.First());

            // Assert
            using (StreamReader sr = new StreamReader("NZVisa.html", Encoding.UTF8))
            {
                var expectedHtmlString = sr.ReadToEnd();

                actual.Content.ShouldBeEquivalentTo(expectedHtmlString);
            }
        }
    }
}
