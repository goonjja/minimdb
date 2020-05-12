namespace MiniMdb.Backend.Shared
{
    /// <summary>
    /// Api error envelope
    /// </summary>
    public class ApiError
    {
        // TODO introduce contants for error codes
        /// <summary>
        /// Unique error code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error details
        /// </summary>
        public string Message { get; set; }
    }
}
