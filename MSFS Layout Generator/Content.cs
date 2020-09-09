using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSFSLayoutGenerator
{
    public class Content
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("date")]
        public long Date { get; set; }
    }
}