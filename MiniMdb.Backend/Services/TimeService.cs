using System;

namespace MiniMdb.Backend.Services
{
    public interface ITimeService
    {
        /// <summary>
        /// Returns current UTC timestamp in seconds
        /// </summary>
        long Now();

        /// <summary>
        /// Converts UTC timestamp to DateTimeOffset
        /// </summary>
        DateTimeOffset ToDateTimeOffset(long timestamp);
    }

    public class TimeService : ITimeService
    {
        public long Now() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public DateTimeOffset ToDateTimeOffset(long timestamp) => DateTimeOffset.FromUnixTimeSeconds(timestamp);
    }
}
