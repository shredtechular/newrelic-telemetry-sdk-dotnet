﻿//using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NewRelic.Telemetry.Metrics
{
    /// <summary>
    /// Properties that are common all of the Metrics that are part of the MetricBatch.
    /// </summary>
    [DataContract]
    public class MetricBatchCommonProperties
    {
        /// <summary>
        /// Optional:  
        /// TODO
        /// </summary>
        [DataMember(Name = "timestamp")]
//        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? Timestamp { get; internal set; }

        /// <summary>
        /// Optional:  
        /// TODO
        /// </summary>
        [DataMember(Name = "interval.ms")]
//        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? IntervalMs { get; internal set; }

        /// <summary>
        /// Provides additional contextual information that is common to all of the
        /// Spans being reported in this SpanBatch.
        /// </summary>
        [DataMember(Name = "attributes")]
 //       [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string,object> Attributes { get; internal set; }

        internal MetricBatchCommonProperties()
        {
        }
    }
}
