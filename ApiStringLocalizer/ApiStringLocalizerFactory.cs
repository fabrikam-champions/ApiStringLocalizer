using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net.Http;

namespace ApiStringLocalizer
{
    public class ApiStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly HttpClient _httpClient;
        private readonly ApiStringLocalizerOptions _options;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;
        public ApiStringLocalizerFactory(HttpClient httpClient, IOptions<ApiStringLocalizerOptions> options, IMemoryCache memoryCache, ILogger<ApiStringLocalizerFactory> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            return new ApiStringLocalizer(_httpClient, culture, resourceSource, _memoryCache, _logger, _options);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            return new ApiStringLocalizer(_httpClient, culture, null, _memoryCache, _logger, _options);
        }
    }
}