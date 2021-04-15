using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSFSLayoutGenerator
{
    public class Manifest
    {
        [JsonExtensionDataAttribute]
        public IDictionary<string, object> Data { get; set; }

        [JsonPropertyName("total_package_size")]
        public string TotalPackageSize { get; set; }
    }
}