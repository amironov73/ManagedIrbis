using System;

using Newtonsoft.Json;

namespace IrbisBot
{
    class Announcement
    {
        [JsonProperty("NAME")]
        public string Name { get; set; }

        [JsonProperty("PREVIEW_TEXT")]
        public string Text { get; set; }

        [JsonProperty("DETAIL_PICTURE")]
        public string Picture { get; set; }

        [JsonProperty("PROPERTY_DATE_S1_VALUE")]
        public DateTime? Date { get; set; }

        [JsonProperty("DETAIL_PAGE_URL")]
        public string Url { get; set; }
    }
}
