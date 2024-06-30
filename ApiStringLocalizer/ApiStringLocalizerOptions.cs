using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ApiStringLocalizerOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string RouteTemplate { get; set; } = "{resourceName}/{culture}";
        public bool ThrowOnError { get; set; } = false;
        public TimeSpan? CacheTimeout { get; set; }
    }
}