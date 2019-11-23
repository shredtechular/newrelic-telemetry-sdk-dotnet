﻿using System;
using System.Collections.Generic;
using NewRelic.Telemetry.Extensions;

namespace NewRelic.Telemetry.Spans
{
    public class SpanBuilder
    {
        private const string attribName_ServiceName = "service.name";
        private const string attribName_DurationMs = "duration.ms";
        private const string attribName_Name = "name";
        private const string attribName_ParentID = "parent.id";
        private const string attribName_Error = "error";

        public static SpanBuilder Create(string spanId)
        {
            return new SpanBuilder(spanId);
        }

        private readonly Span _span = new Span();

        private Dictionary<string, object> _attributes => _span.Attributes ?? (_span.Attributes = new Dictionary<string, object>());

        private SpanBuilder(string spanId)
        {
            if (string.IsNullOrEmpty(spanId))
            {
                throw new NullReferenceException("Span id is not set.");
            }

            _span.Id = spanId;
        }

        public Span Build()
        {
            return _span;
        }

        public SpanBuilder WithTraceId(string traceId)
        {
            _span.TraceId = traceId;
            return this;
        }

        public SpanBuilder WithTimestamp(long timestamp)
        {
            if(timestamp == default)
            {
                return this;
            }

            _span.Timestamp = timestamp;
            return this;
        }

        public SpanBuilder WithTimestamp(DateTimeOffset timestamp)
        {
            if (timestamp == null)
            {
                return this;
            }
            
            return WithTimestamp(DateTimeExtensions.ToUnixTimeMilliseconds(timestamp));
        }

        public SpanBuilder HasError(bool b)
        {
            if (b)
            {
                return WithAttribute(attribName_Error, true);
            }

            if (_span.Attributes?.ContainsKey(attribName_Error) == true)
            {
                _span.Attributes.Remove(attribName_Error);
            }

            return this;
        }

        public SpanBuilder WithDurationMs(double durationMs)
        {
            WithAttribute(attribName_DurationMs, durationMs);
            return this;
        }

        public SpanBuilder WithDurationMs(DateTimeOffset startTimestamp, DateTimeOffset endTimestamp)
        {
            if(startTimestamp == null || endTimestamp == null)
            {
                return this;
            }

            return WithDurationMs(DateTimeExtensions.ToUnixTimeMilliseconds(endTimestamp)
                - DateTimeExtensions.ToUnixTimeMilliseconds(startTimestamp));
        }

        public SpanBuilder WithExecutionTimeInfo(DateTimeOffset startTimestamp, DateTimeOffset endTimestamp)
        {
            if(startTimestamp == null)
            {
                return this;
            }

            var startTimestampUnix = DateTimeExtensions.ToUnixTimeMilliseconds(startTimestamp);
            WithTimestamp(startTimestampUnix);
            
            if(endTimestamp == null)
            {
                return this;
            }

            WithDurationMs(DateTimeExtensions.ToUnixTimeMilliseconds(endTimestamp) - startTimestampUnix);

            return this;
        }

        public SpanBuilder WithName(string name)
        {
            WithAttribute(attribName_Name, name);
            return this;
        }

        public SpanBuilder WithParentId(string parentId)
        {
            WithAttribute(attribName_ParentID, parentId);
            return this;
        }

        public SpanBuilder WithServiceName(string serviceName)
        {
            WithAttribute(attribName_ServiceName, serviceName);
            return this;
        }

        public SpanBuilder WithAttributes<T>(IEnumerable<KeyValuePair<string,T>> attributes)
        {
            if (attributes == null)
            {
                return this;
            }

            foreach (var attrib in attributes)
            {
                WithAttribute(attrib.Key, attrib.Value);
            }

            return this;

        }
    
        public SpanBuilder WithAttribute<T>(string attribName, T attribVal)
        {
            if (string.IsNullOrWhiteSpace(attribName))
            {
                throw new InvalidOperationException($"{nameof(attribName)} cannot be empty.");
            }
           
            _attributes[attribName] = attribVal;
            return this;
        }

    }
}
