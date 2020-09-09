using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSFSLayoutGenerator
{
    public class Layout
    {
        [JsonPropertyName("content")]
        public List<Content> Content { get; set; } = new List<Content>();
    }
}