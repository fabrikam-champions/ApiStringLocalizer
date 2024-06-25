using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ApiStringLocalizerOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public bool ThrowOnError { get; set; } = true;
        public TimeSpan CacheTimeout { get; set; } = TimeSpan.FromHours(1);
    }
}