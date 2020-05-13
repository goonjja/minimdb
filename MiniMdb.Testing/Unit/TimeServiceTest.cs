using MiniMdb.Backend.Services;
using System;
using Xunit;

namespace MiniMdb.Testing.Unit
{
    public class TimeServiceTest
    {
        [Fact]
        public void TestConversion()
        {
            ITimeService timeService = new TimeService();
            var referenceTime = DateTimeOffset.UtcNow;
            var timestamp = timeService.Now();
            var converted = timeService.ToDateTimeOffset(timestamp);
            var delta = converted - referenceTime;
            Assert.True(delta.TotalSeconds < 2, "Converted timestamp is incorrect");
        }
    }
}
