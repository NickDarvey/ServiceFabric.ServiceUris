using System;

namespace NickDarvey.ServiceFabric.ServiceUris
{
    public static class ServiceRemoting
    {
        /// <summary>
        /// Takes a Service Fabric reverse proxy or relative URI and returns a Service Fabric Service Remoting URI.
        /// </summary>
        /// <param name="serviceUri">Reverse proxy or relative URI. e.g. 'fabric:/Cheese/Bloop'</param>
        /// <returns>Service Fabric reverse proxy URI. e.g. 'http://localhost:19080/Cheese/Bloop'</returns>
        /// <exception cref="ArgumentException">Unable to validate the <paramref name="serviceUri"/></exception>
        /// <exception cref="UriFormatException">Unable to build a valid URI using the <paramref name="serviceUri"/></exception>
        public static Uri ToServiceRemotingServiceUri(this Uri serviceUri) =>
            serviceUri.IsAbsoluteUri == false ? AddRelativeAddress(serviceUri)
            : ReverseProxy.IsReverseProxyServiceUri(serviceUri) ? AddPathAndQuery(serviceUri)
            : ServiceRemoting.IsServiceRemotingUri(serviceUri) ? serviceUri
            : throw new ArgumentException($"Cannot convert '{serviceUri}' to a reverse proxy service URI");

        internal static bool IsServiceRemotingUri(Uri serviceUri) =>
            serviceUri.Scheme == "fabric";

        /// <summary>
        /// http://localhost:19080/X/Y --> fabric:/X/Y
        /// </summary>
        private static Uri AddPathAndQuery(Uri serviceUri) =>
            new Uri("fabric:/" + serviceUri.PathAndQuery.TrimStart('/'));

        /// <summary>
        /// /X/Y --> fabric:/X/Y
        /// </summary>
        private static Uri AddRelativeAddress(Uri serviceUri) =>
            new Uri("fabric:/" + serviceUri.OriginalString.TrimStart('/'));
    }
}
