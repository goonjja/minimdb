namespace MiniMdb.Backend.Shared
{
    /// <summary>
    /// Unified envelope for API messages.
    /// </summary>
    public class ApiMessage
    {
        /// <summary>
        /// Not null when API call fails
        /// </summary>
        public ApiError Error { get; set; }

        /// <summary>
        /// Not null when API method supports pagination
        /// </summary>
        public ApiPagination Pagination { get; set; }

        public static ApiMessage MakeError(int code, string message)
        {
            return new ApiMessage { Error = new ApiError { Code = code, Message = message } };
        }

        public static ApiMessage<T> From<T>(ApiPagination pagination, params T[] data)
        {
            return new ApiMessage<T> { Data = data, Pagination = pagination };
        }

        public static ApiMessage<T> From<T>(T data)
        {
            return new ApiMessage<T> { Data = new[] { data } };
        }
    }

    /// <summary>
    /// Unified envelope for API result with data.
    /// </summary>
    /// <typeparam name="T">data type of successful execution</typeparam>
    public class ApiMessage<T> : ApiMessage
    {
        /// <summary>
        /// API result in case of successful execution
        /// </summary>
        public T[] Data { get; set; }
    }
}
