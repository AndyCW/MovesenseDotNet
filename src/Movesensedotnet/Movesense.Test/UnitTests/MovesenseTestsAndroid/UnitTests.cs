using MdsLibrary.Model;
using Plugin.Movesense;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace MovesensedotNetTests
{

    public class UnitTests : IClassFixture<MdsConnectionFixture>
    {
        IMovesenseDevice movesenseDevice;

        public UnitTests(MdsConnectionFixture fixture)
        {
            movesenseDevice = fixture.MovesenseDevice;
        }

        [Fact]
        public async void GetDeviceInfoTest()
        {
            // Talk to the device
            var info = await movesenseDevice.GetDeviceInfoAsync();
            // Just do a version check - could be spmething like "1.7.2" so normal double.Parse is no good
            var parts = info.DeviceInfo.Sw.Split(new char[] { '.' });
            var base_version = Int32.Parse(parts[0]) * 10 + Int32.Parse(parts[1]);
            Assert.True(base_version > 15, "Device software version > 1.5");
        }

        [Fact]
        public async void GetBatteryTest()
        {
            // Talk to the device
            var batt = await movesenseDevice.GetBatteryLevelAsync();
            // Check that we're > 20%
            Assert.True(batt.ChargePercent > 20, "Device battery charge > 20%");
        }

        [Fact]
        public async void   GetDetailedTimeTest()
        {
            // Talk to the device
            var detailedTime = await movesenseDevice.GetDetailedTimeAsync();
            // Check that the response looks reasonable
            Assert.True(detailedTime.Properties.UtcTime > 1550000000000, "UtcTime > Feb 12 2019");
            Assert.True(detailedTime.Properties.RelativeTime > 1000, "RelativeTime has value");
            Assert.True(detailedTime.Properties.TickRate > 1000, "Tick rate has value");
            Assert.True(detailedTime.Properties.Accuracy > 5, "Nominal accuracy has value");
        }

        [Fact]
        public async void GetLogEntriesTest()
        {
            // delete any existing log entries
            await movesenseDevice.DeleteLogEntriesAsync();
            // Start logging
            await movesenseDevice.CreateLogEntryAsync();
            // Configure logger
            DataLoggerConfig.DataEntry[] entries = { new DataLoggerConfig.DataEntry("/Meas/IMU9/52") };
            DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
            await movesenseDevice.SetLoggerConfigAsync(config);
            // Start logging
            await movesenseDevice.SetLoggerStatusAsync(true);
            Assert.True((await movesenseDevice.GetLoggerStatusAsync()).LogStatus == 3, "Verify data logger is running");
            // Pause this test for 5 seconds
            await Task.Delay(5000);
            // Stop the logging
            await movesenseDevice.SetLoggerStatusAsync(false);
            // Check it's stopped
            Assert.True((await movesenseDevice.GetLoggerStatusAsync()).LogStatus == 2, "Verify data logger not running");
            // Get the log entries
            var log = await movesenseDevice.GetLogEntriesJsonAsync();
            // Check that we've got some logbook entries
            Assert.True(log.Elements.Length > 0, "Log Entries contains results");
            // Check data integrity
            Assert.True(log.Elements[0].ModificationTimestamp > 0, "Log entry modification timestamp valid");
        }

        [Fact]
        public async void SetGetTimeTest()
        {
            var currTime = await movesenseDevice.GetTimeAsync();
            Assert.True(currTime.Time > 0, "Current device time > 0");

            // Set the time to an arbitrary value
            long timems = 1500000000000;
            string requestBody = $"{{\"value\":{timems * 1000}}}";
            System.Diagnostics.Debug.WriteLine($"INFO SetTimeAsync {requestBody}");
            await Plugin.Movesense.CrossMovesense.Current.ApiCallAsync(movesenseDevice, Plugin.Movesense.Api.MdsOp.PUT, "/Time", requestBody);

            // Check it took
            currTime = await movesenseDevice.GetTimeAsync();
            Assert.True(currTime.Time - (timems * 1000) < 200000, "Current device time set to 1500000000000ms");
        }

        [Fact]
        public async void SetTimeTest()
        {
            // Set the time using the API method
            await movesenseDevice.SetTimeAsync();

            long phonetimemicros = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds * 1000;
            // Check that that took
            var currTime = await movesenseDevice.GetTimeAsync();
            Assert.True(currTime.Time - phonetimemicros < 20000, "Current device time set to phone time");
        }

        [Fact]
        public async void GetLogEntriesTimestampBug()
        {
            // delete any existing log entries
            await movesenseDevice.DeleteLogEntriesAsync();
            // Start logging
            await movesenseDevice.CreateLogEntryAsync();
            // Configure logger
            DataLoggerConfig.DataEntry[] entries = { new DataLoggerConfig.DataEntry("/Meas/IMU9/52") };
            DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
            await movesenseDevice.SetLoggerConfigAsync(config);

            // Set the time to Int32.MaxValue seconds since Epoch
            long timemicros = (long)Int32.MaxValue * 1000000;
            string requestBody = $"{{\"value\":{timemicros}}}";
            Debug.WriteLine($"TEST SetTime TIME {requestBody}");
            await Plugin.Movesense.CrossMovesense.Current.ApiCallAsync(movesenseDevice, Plugin.Movesense.Api.MdsOp.PUT, "/Time", requestBody);

            // Check it took
            var currTime = await movesenseDevice.GetTimeAsync();
            Assert.True(currTime.Time - timemicros < 200000, $"Current device time set to {Int32.MaxValue} seconds since Epoch");

            // Start logging
            await movesenseDevice.SetLoggerStatusAsync(true);
            Assert.True((await movesenseDevice.GetLoggerStatusAsync()).LogStatus == 3, "Verify data logger is running");
            // Pause this test for 2 seconds
            await Task.Delay(2000);
            // Stop the logging
            await movesenseDevice.SetLoggerStatusAsync(false);
            // Check it's stopped
            Assert.True((await movesenseDevice.GetLoggerStatusAsync()).LogStatus == 2, "Verify data logger not running");
            // Get the log entries - should assert without the deserialization fix in it
            var log = await movesenseDevice.GetLogEntriesJsonAsync();
            // Check that we've got some logbook entries
            Assert.True(log.Elements.Length > 0, "Log Entries contains results");
            // Check data integrity
            Assert.True(log.Elements[0].ModificationTimestamp > 0, "Log entry modification timestamp valid");
        }


        //[Fact]
        //public async Task ThisShouldFail()
        //{
        //    await Task.Run(() => { throw new Exception("boom"); });
        //}
    }

}
