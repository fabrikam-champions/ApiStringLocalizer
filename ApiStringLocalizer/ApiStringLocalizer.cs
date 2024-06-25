using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiStringLocalizer
{
    public class ApiStringLocalizer : IStringLocalizer
    {
        private readonly HttpClient _httpClient;
        private readonly string _culture;
        private readonly Type _resourceSource;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;
        private readonly ApiStringLocalizerOptions _options;

        public ApiStringLocalizer(HttpClient httpClient, string culture, Type resourceSource, IMemoryCache memoryCache, ILogger logger, ApiStringLocalizerOptions options)
        {
            _httpClient = httpClient;
            _culture = culture;
            _resourceSource = resourceSource;
            _memoryCache = memoryCache;
            _logger = logger;
            _options = options;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name).GetAwaiter().GetResult();
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = this[name].Value;
                var value = string.Format(format, arguments);
                return new LocalizedString(name, value, format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var resources = GetResources().GetAwaiter().GetResult();
            return resources.Select(res => this[res.Key]);
        }

        private async Task<string> GetString(string name)
        {
            var resources = await GetResources();
            resources.TryGetValue(name, out var value);
            return value;
        }
        private async Task<IDictionary<string, string>> GetResources()
        {
            var resources = await _memoryCache.GetOrCreateAsync($"{_resourceSource?.FullName}${_culture}", async cacheEntry => {
                cacheEntry.AbsoluteExpirationRelativeToNow = _options.CacheTimeout;
                var uri = $"{_options.BaseUrl}/{WebUtility.UrlEncode(_resourceSource?.FullName)}/{WebUtility.UrlEncode(_culture)}";
                _logger.LogInformation($"Calling localization API. uri: {uri}");
                HttpResponseMessage response = null;
                string content = null;
                try
                {
                    response = await _httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<Dictionary<string, string>>(content);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, $"Error calling localization API. uri: {uri}, statusCode: {response?.StatusCode}, content: {content}");
                    if(_options.ThrowOnError)
                        throw;
                }
                return new Dictionary<string, string>();
            });
            return resources;
        }
    }
}