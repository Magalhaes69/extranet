
using Application.Shared.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Helps
{
    internal static class SapErrors
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Turns a failed Service Layer response body into a SapB1Exception
        /// carrying SAP's own error code and message.
        /// </summary>
        public static SapB1Exception Create(string body, int statusCode)
        {
            try
            {
                var error = JsonSerializer.Deserialize<SapErrorResponse>(body, JsonOptions)?.Error;
                if (error is not null)
                    return new SapB1Exception(
                        error.Code,
                        error.Message?.Value ?? "Unknown SAP B1 error",
                        statusCode);
            }
            catch (JsonException)
            {
                // Body isn't SAP JSON (e.g. proxy or HTML error page)
            }

            return new SapB1Exception(
                0,
                string.IsNullOrWhiteSpace(body) ? $"HTTP {statusCode}" : body,
                statusCode);
        }

        private sealed class SapErrorResponse
        {
            public SapError? Error { get; set; }
        }

        private sealed class SapError
        {
            public int Code { get; set; }
            public SapErrorMessage? Message { get; set; }
        }

        private sealed class SapErrorMessage
        {
            public string? Lang { get; set; }
            public string? Value { get; set; }
        }
    }
}
