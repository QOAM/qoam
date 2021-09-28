using System.Collections.Generic;
using Newtonsoft.Json;

namespace QOAM.Core.Import.CrossRef
{
    public class CrossRefResult<TMessage>
    {
        public string Status { get; set; }
        
        [JsonProperty("message-type")]
        public string MessageType { get; set; }

        [JsonProperty("message-version")]
        public string MessageVersion { get; set; }

        public TMessage Message { get; set; }

    }

    public class CrossRefListResult : CrossRefResult<JournalListResponse>
    {
    }

    public class CrossRefJournalResult : CrossRefResult<Item>
    {
    }

    public class JournalListResponse
    {
        [JsonProperty("items-per-page")]
        public int ItemsPerPage { get; set; }

        [JsonProperty("next-cursor")]
        public string NextCursor { get; set; }

        [JsonProperty("total-results")]
        public int TotalResults { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string Title { get; set; }
        public Counts Counts { get; set; }
        public string Publisher { get; set; }

        public List<CrossRefSubject> Subjects { get; set; }

        [JsonProperty("issn-type")]
        public List<IssnType> IssnTypes { get; set; }
    }

    public class CrossRefSubject
    {
        public int Ascj { get; set; }
        public string Name { get; set; }
    }
    public class Counts
    {
        [JsonProperty("current-dois")]
        public int CurrentDois { get; set; }
    }

    public class Breakdown
    {
        [JsonProperty("dois-by-issued-year")]
        public string[] DoisIssuedByYear { get; set; }
    }

    public class IssnType
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}