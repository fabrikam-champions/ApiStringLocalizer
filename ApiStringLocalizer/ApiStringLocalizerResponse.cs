using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiStringLocalizer
{
    public class ApiStringLocalizerResponse
    {
        [JsonProperty("localizationData")]
        public Dictionary<string, string> LocalizationData;
    }
}
