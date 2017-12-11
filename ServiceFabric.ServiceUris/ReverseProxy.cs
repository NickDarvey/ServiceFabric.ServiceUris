using System;

namespace NickDarvey.ServiceFabric.ServiceUris
{
    public static class ReverseProxy
    {
        /// <summary>
        /// Takes a Service Fabric Service Remoting or relative URI and base address and returns a Service Fabric reverse proxy URI.
        /// </summary>
        /// <param name="serviceUri">Service Remoting or relative URI. e.g. 'fabric:/Cheese/Bloop'</param>
        /// <param name="baseAddress">Reverse proxy base address. e.g. 'http://localhost:19080/'</param>
        /// <returns>Service Fabric reverse proxy URI. e.g. 'http://localhost:19080/Cheese/Bloop'</returns>
        /// <exception cref="ArgumentException">Unable to validate the <paramref name="serviceUri"/> and <paramref name="baseAddress"/></exception>
        /// <exception cref="UriFormatException">Unable to build a valid URI using the <paramref name="serviceUri"/> and <paramref name="baseAddress"/></exception>
        public static Uri ToReverseProxyServiceUri(this Uri serviceUri, Uri baseAddress) =>
            serviceUri.IsAbsoluteUri == false ? AddRelativeAddress(serviceUri, baseAddress)
            : ServiceRemoting.IsServiceRemotingUri(serviceUri) ? ReplaceBaseAddress(serviceUri, baseAddress)
            : ReverseProxy.IsReverseProxyServiceUri(serviceUri) ? ReplaceBaseAddress(serviceUri, baseAddress)
            : throw new ArgumentException($"Cannot convert '{serviceUri}' to a reverse proxy service URI");

        internal static bool IsReverseProxyServiceUri(Uri serviceUri) =>
            serviceUri.HostNameType == UriHostNameType.Dns;

        /// <summary>
        /// fabric:/X/Y --> baseAddressScheme://baseAddressHostName/X/Y,
        /// oldBaseAddressScheme://oldBaseAddressHostName/X/Y --> newBaseAddressScheme://newBaseAddressHostName/X/Y
        /// </summary>
        private static Uri ReplaceBaseAddress(Uri serviceUri, Uri baseAddress) =>
            Uri.TryCreate(baseAddress, serviceUri.PathAndQuery, out var result) ? result
            : throw new UriFormatException($"Couldn't create a reverse proxy service URI using '{baseAddress}' and '{serviceUri.PathAndQuery}'");

        /// <summary>
        /// /X/Y --> baseAddressScheme://baseAddressHostName/X/Y
        /// </summary>
        private static Uri AddRelativeAddress(Uri serviceUri, Uri baseAddress) =>
            Uri.TryCreate(baseAddress, serviceUri.OriginalString, out var result) ? result
            : throw new UriFormatException($"Couldn't create a reverse proxy service URI using '{baseAddress}' and '{serviceUri.OriginalString}'");
    }
}
