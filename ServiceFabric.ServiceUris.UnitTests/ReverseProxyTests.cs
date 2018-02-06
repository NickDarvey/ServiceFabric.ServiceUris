using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NickDarvey.ServiceFabric.ServiceUris.UnitTests
{
    public class ReverseProxyTests
    {
        [Fact]
        public void ToReverseProxy_Returns_ServiceUri_When_ServiceRemoting()
        {
            var original = new Uri("fabric:/Application/Name/And/Service/Name");

            var result = original.ToReverseProxyServiceUri(new Uri("https://localhost:19080"));

            Assert.Equal(new Uri("https://localhost:19080/Application/Name/And/Service/Name"), result);
        }

        [Theory]
        [InlineData("/Application/Name/And/Service/Name")]
        [InlineData("Application/Name/And/Service/Name")]
        public void ToReverseProxy_Returns_ServiceUri_When_Relative(string sourceServiceUri)
        {
            var original = new Uri(sourceServiceUri, UriKind.Relative);

            var result = original.ToReverseProxyServiceUri(new Uri("https://localhost:19080"));

            Assert.Equal(new Uri("https://localhost:19080/Application/Name/And/Service/Name"), result);
        }

        [Fact]
        public void ToReverseProxy_Returns_ServiceUri_When_ReverseProxy()
        {
            var original = new Uri("https://localhost:0000/Application/Name/And/Service/Name");

            var result = original.ToReverseProxyServiceUri(new Uri("https://localhost:19080"));

            Assert.Equal(new Uri("https://localhost:19080/Application/Name/And/Service/Name"), result);
        }
    }
}
