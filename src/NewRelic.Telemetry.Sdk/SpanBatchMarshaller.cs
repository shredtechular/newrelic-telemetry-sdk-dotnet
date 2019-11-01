﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;


namespace NewRelic.Telemetry.Sdk
{
    public class SpanBatchMarshaller
    {
        public SpanBatchMarshaller()
        {
        }

        public virtual string ToJson(SpanBatch batch) 
        {
            var options = new JsonWriterOptions
            {
                Indented = false
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    writer.WriteStartArray();
                    writer.WriteStartObject();

                    if (batch != null)
                    {
                        if (!string.IsNullOrEmpty(batch.TraceId) || batch.Attributes?.Count > 0)
                        {
                            BuildCommonBlock(writer, batch);
                        }

                        BuildSpansBlock(writer, batch);
                    }

                    writer.WriteEndObject();
                    writer.WriteEndArray();
                }
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private void BuildCommonBlock(Utf8JsonWriter writer, SpanBatch batch)
        {
            writer.WritePropertyName("common");
            writer.WriteStartObject();
            if (!string.IsNullOrEmpty(batch.TraceId))
            {
                writer.WriteString("trace.id", batch.TraceId);
            }

            var customAttributes = batch.Attributes;
            if (customAttributes?.Count > 0)
            {
                BuidAttributesBlock(writer, customAttributes);
            }

            writer.WriteEndObject();
        }

        private void BuidAttributesBlock(Utf8JsonWriter writer, IDictionary<string, object> attributes)
        {

            writer.WritePropertyName("attributes");
            writer.WriteStartObject();

            foreach (var attribute in attributes)
            {
                var t = attribute.Value.GetType();
                if (t == typeof(string))
                {
                    writer.WriteString(attribute.Key, (string)attribute.Value);
                }
                else if (t == typeof(int))
                {
                    writer.WriteNumber(attribute.Key, (int)attribute.Value);
                }
                else if (t == typeof(double))
                {
                    writer.WriteNumber(attribute.Key, (double)attribute.Value);
                }
                else if (t == typeof(long))
                {
                    writer.WriteNumber(attribute.Key, (long)attribute.Value);
                }
                else if (t == typeof(float))
                {
                    writer.WriteNumber(attribute.Key, (float)attribute.Value);
                }
                else if (t == typeof(uint))
                {
                    writer.WriteNumber(attribute.Key, (uint)attribute.Value);
                }
                else if (t == typeof(ulong))
                {
                    writer.WriteNumber(attribute.Key, (ulong)attribute.Value);
                }
                else if (t == typeof(decimal))
                {
                    writer.WriteNumber(attribute.Key, (decimal)attribute.Value);
                }
                else if (t == typeof(bool))
                {
                    writer.WriteBoolean(attribute.Key, (bool)attribute.Value);
                }
                else
                {
                    writer.WriteString(attribute.Key, attribute.Value.ToString());
                }
            }

            writer.WriteEndObject();

        }

        private void BuildSpansBlock(Utf8JsonWriter writer, SpanBatch batch)
        {
            if(batch.Spans == null || batch.Spans.Count == 0) 
            {
                return;
            }

            bool didCreateSpansProperty = false;

            foreach (var span in batch.Spans)
            {
                if (span != null)
                {
                    if (!didCreateSpansProperty)
                    {
                        writer.WritePropertyName("spans");
                        writer.WriteStartArray();
                        didCreateSpansProperty = true;
                    }

                    writer.WriteStartObject();

                    writer.WriteString("id", span.Id);

                    if (!string.IsNullOrEmpty(span.TraceId))
                    {
                        writer.WriteString("trace.id", span.TraceId);
                    }

                    if (span.Timestamp != default)
                    {
                        writer.WriteNumber("timestamp", span.Timestamp);
                    }

                    if (span.Error)
                    {
                        writer.WriteBoolean("error", span.Error);
                    }

                    var attributes = span.Attributes;

                    if(attributes == null) 
                    {
                        attributes = new Dictionary<string, object>();
                    }

                    if (span.DurationMs != default)
                    {
                        attributes.Add("duration.ms", span.DurationMs);
                    }

                    if (!string.IsNullOrEmpty(span.Name))
                    {
                        attributes.Add("name", span.Name);
                    }

                    if (!string.IsNullOrEmpty(span.ServiceName))
                    {
                        attributes.Add("service.name", span.ServiceName);
                    }

                    if (!string.IsNullOrEmpty(span.ParentId))
                    {
                        attributes.Add("parent.id", span.ParentId);
                    }


                    if (attributes.Count > 0)
                    {
                        BuidAttributesBlock(writer, attributes);
                    }

                    writer.WriteEndObject();
                }
            }

            if (didCreateSpansProperty)
            {
                writer.WriteEndArray();
            }
        }
    }
}
