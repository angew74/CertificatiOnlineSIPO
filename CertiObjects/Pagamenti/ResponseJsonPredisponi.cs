using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Unisys.CdR.Certi.Objects.Pagamenti
{
    public partial class ResponseJsonPredisponi
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }
    } 

    public partial class Elements
    {
        [JsonProperty("ticket")]
        public Guid Ticket { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("@dto")]
        public string Dto { get; set; }

        [JsonProperty("features")]
        public FeaturesClass Features { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public partial class FeaturesClass
    {
    }

  

   

   
}

