namespace MiniMdb.Backend.Shared
{
    /// <summary>
    /// Api error envelope
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Unique error code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error details
        /// </summary>
        public string Message { get; set; }

        #region Application error codes

        public static ApiError SystemError { get; } = new ApiError { Code = 1, Message = "Internal error" };

        public static ApiError InvalidCredentials { get; } = new ApiError { Code = 2, Message = "Invalid credentials" };

        public static ApiError NotFound { get; } = new ApiError { Code = 3, Message = "Not found" };

        #endregion
    }
}
