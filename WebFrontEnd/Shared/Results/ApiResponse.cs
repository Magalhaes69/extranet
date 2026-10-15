namespace WebFrontEnd.Shared.Results
{
    public sealed class ApiResponse <T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public IReadOnlyCollection<string>? Erros { get; set; }

        public DateTime Timestamp { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Creates a successful API response containing the specified data and an optional message.
        /// </summary>
        /// <param name="data">The data to include in the response. This value is assigned to the Data property of the returned
        /// ApiResponse<typeparamref name="T"/>.</param>
        /// <param name="message">An optional message describing the result of the operation. The default is "Operation completed
        /// successfully.".</param>
        /// <returns>An <see cref="ApiResponse{T}"/> instance representing a successful operation, with the specified data and message.</returns>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully.")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates a failed API response with the specified error message and optional error details.
        /// </summary>
        /// <param name="message">The error message describing the reason for the failure. Cannot be null.</param>
        /// <param name="errors">An optional collection of error details to include in the response. Can be null if no additional errors are
        /// provided.</param>
        /// <returns>An <see cref="ApiResponse{T}"/> instance representing a failed response, containing the specified message
        /// and error details.</returns>
        public static ApiResponse<T> FailureResponse(string message, IReadOnlyCollection<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Erros = errors
            };
        }
    }
}
