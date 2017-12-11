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
            serviceUri.IsAbsoluteUri == false ? ReverseProxyFromRelative(serviceUri, baseAddress)
            : serviceUri.Scheme == "fabric" ? ReverseProxyFromServiceRemoting(serviceUri, baseAddress)
            : throw new ArgumentException($"Cannot convert '{serviceUri}' to a reverse proxy service URI");

        /// <summary>
        /// fabric:/X/Y --> baseAddressScheme://baseAddressHostName/X/Y
        /// </summary>
        private static Uri ReverseProxyFromServiceRemoting(Uri serviceUri, Uri baseAddress) =>
            Uri.TryCreate(baseAddress, serviceUri.PathAndQuery, out var result) ? result
            : throw new UriFormatException($"Couldn't create a reverse proxy service URI using '{baseAddress}' and '{serviceUri.PathAndQuery}'");

        /// <summary>
        /// /X/Y --> baseAddressScheme://baseAddressHostName/X/Y
        /// </summary>
        private static Uri ReverseProxyFromRelative(Uri serviceUri, Uri baseAddress) =>
            Uri.TryCreate(baseAddress, serviceUri.OriginalString, out var result) ? result
            : throw new UriFormatException($"Couldn't create a reverse proxy service URI using '{baseAddress}' and '{serviceUri.OriginalString}'");
    }
}
