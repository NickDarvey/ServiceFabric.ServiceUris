# ServiceFabric.ServiceUris
Sometimes you use Service Remoting, sometimes you use HTTP and sometimes you use both. This library has transformations from Service Remoting URIs to reverse proxy URIs and vice versa.

## Service Remoting to reverse proxy
```c#
var original = new Uri("fabric:/Application/Name/And/Service/Name");
var result = original.ToReverseProxyServiceUri(new Uri("https://localhost:19080"));
Assert.Equal(new Uri("https://localhost:19080/Application/Name/And/Service/Name"), result)
```

## Reverse proxy to Service Remoting
```c#
var original = new Uri("https://localhost:19080/Application/Name/And/Service/Name");
var result = original.ToServiceRemotingServiceUri();
Assert.Equal(new Uri("fabric:/Application/Name/And/Service/Name"), result);
```