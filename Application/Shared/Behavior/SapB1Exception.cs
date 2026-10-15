

namespace Application.Shared.Behavior
{
    public class SapB1Exception : Exception
    {
        public int Code { get; }
        public int StatusCode { get; }

        public SapB1Exception(int code, string message, int statusCode)
            : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }

        public sealed class SapErrorResponse
        {
            public SapError? Error { get; set; }
        }

        public sealed class SapError
        {
            public int Code { get; set; }
            public SapErrorMessage? Message { get; set; }
        }

        public sealed class SapErrorMessage
        {
            public string? Lang { get; set; }
            public string? Value { get; set; }
        }
    }
}
