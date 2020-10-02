using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Com.Unisys.CdR.Certi.Objects.Pagamenti
{
   

    public partial class ResponseJson
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }
    }
    public partial class Body
    {
        [JsonProperty("dto")]
        public string Dto { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("elements")]
        public Elements Elements { get; set; }
    }

    public partial class Header
    {
        [JsonProperty("msgUid")]
        public Guid MsgUid { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("metadata")]
        public HeaderMetadata Metadata { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("retCode")]
        public long RetCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("requestRef")]
        public RequestRef RequestRef { get; set; }
    }

    public partial class HeaderMetadata
    {
        [JsonProperty("TRACKER_BIZID_TEST4")]
        public string TrackerBizidTest4 { get; set; }

        [JsonProperty("TRACKER_BIZID_PROG_POSIZIONE")]      
        public string TrackerBizidProgPosizione { get; set; }

        [JsonProperty("TRACKER_BIZID_IUV")]
        public string TrackerBizidIuv { get; set; }
    }

    public partial class RequestRef
    {
        [JsonProperty("msgUid")]
        public string MsgUid { get; set; }

        [JsonProperty("timestamp")]
        public object Timestamp { get; set; }

        [JsonProperty("metadata")]
        public RequestRefMetadata Metadata { get; set; }

        [JsonProperty("codApplication")]
        public string CodApplication { get; set; }

        [JsonProperty("codEnte")]
        public object CodEnte { get; set; }

        [JsonProperty("invocationContext")]
        public string InvocationContext { get; set; }

        [JsonProperty("caller")]
        public string Caller { get; set; }

        [JsonProperty("callee")]
        public string Callee { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

    public partial class RequestRefMetadata
    {
        [JsonProperty("TRACKER_BIZID_TEST4")]
        public string TrackerBizidTest4 { get; set; }

        [JsonProperty("TRACKER_BIZID_PROG_POSIZIONE")]       
        public string TrackerBizidProgPosizione { get; set; }

        [JsonProperty("TRACKER_BIZID_IUV")]
        public string TrackerBizidIuv { get; set; }
    }
}


