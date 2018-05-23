using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace LdoClient
{
    public class BbkItem
    {
        public string Index { get; set; }
        public string Text { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("head")]
        public Head Head { get; set; }

        [JsonProperty("results")]
        public Results Results { get; set; }
    }

    public class Head
    {
        [JsonProperty("vars")]
        public string[] Variables { get; set; }
    }

    public class Results
    {
        [JsonProperty("bindings")]
        public Binding[] Bindings { get; set; }
    }

    public class Binding
    {
        [JsonProperty("concept")]
        public Concept Concept { get; set; }

        [JsonProperty("notation")]
        public Notation Notation { get; set; }

        [JsonProperty("hiddenLabel")]
        public Hiddenlabel HiddenLabel { get; set; }
    }

    public class Concept
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Notation
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Hiddenlabel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

}
