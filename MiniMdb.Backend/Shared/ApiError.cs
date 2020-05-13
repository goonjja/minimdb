using Microsoft.Extensions.Localization;

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

        public ApiError Localized(IStringLocalizer localizer)
        {
            return new ApiError { Code = Code, Message = localizer[Message] };
        }

        #region Application error codes

        public static ApiError SystemError { get; } = new ApiError { Code = 1, Message = "Internal error" };

        public static ApiError InvalidCredentials { get; } = new ApiError { Code = 2, Message = "Invalid credentials" };

        public static ApiError NotFound { get; } = new ApiError { Code = 3, Message = "Not found" };

        #endregion
    }
}
