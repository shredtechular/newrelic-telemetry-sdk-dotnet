﻿using System.Net;
using System.Threading.Tasks;
using NewRelic.Telemetry.Transport;

namespace NewRelic.Telemetry.Spans
{
    public class SpanBatchSender
    {
        private IBatchDataSender _sender;

        internal SpanBatchSender(IBatchDataSender sender) 
        {
            _sender = sender;
        }

        public async Task<Response> SendDataAsync(SpanBatch spanBatch)
        {
            if (spanBatch == null || spanBatch.Spans == null || spanBatch.Spans.Count == 0)
            {
                return new Response(false, (HttpStatusCode)0);
            }

            var serializedPayload = spanBatch.ToJson();

            var response = await _sender.SendBatchAsync(serializedPayload);

            return new Response(true, response.StatusCode);
        }
    }
}
