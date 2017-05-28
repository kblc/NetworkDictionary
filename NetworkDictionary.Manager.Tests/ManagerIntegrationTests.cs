using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetworkDictionary.Manager.Tests
{
    /// <summary>
    /// Integration tests for <see cref="Manager"/>
    /// </summary>
    public class ManagerIntegrationTests
    {
        private readonly ManagerOptions _managerOptions;

        public ManagerIntegrationTests()
        {
            _managerOptions = new ManagerOptions(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(3), 100000);
        }

        [Fact]
        public async void SetFor100KKeysShouldTakesALittle()
        {
            //Assign
            var items = Enumerable.Range(0, 100000)
                .Select(i => new { Key = $"key{i:000000}", Value = $"value{i:000000}" })
                .ToArray();

            var sw = new Stopwatch();

            //Act
            using (var manager = ManagerFactory.CreateManager(_managerOptions))
            {
                sw.Start();

                var tasks = items.Select(item => manager.SetValue(item.Key, item.Value)).ToArray();
                await Task.WhenAll(tasks);

                sw.Stop();

                var value = await manager.GetKeys();

                //Assert
                Assert.Equal(100000, value.Length);
            }

            Assert.True(sw.Elapsed <  TimeSpan.FromSeconds(2));
        }

        [Fact]
        public async void AllDataShouldExpiredAndLeftFromDictionatyAfterPause()
        {
            //Assign
            var items = Enumerable.Range(0, 100000)
                .Select(i => new { Key = $"key{i:000000}", Value = $"value{i:000000}" })
                .ToArray();

            //Act
            using (var manager = ManagerFactory.CreateManager(_managerOptions))
            {
                var tasks = items.Select(item => manager.SetValue(item.Key, item.Value)).ToArray();

                await Task.WhenAll(tasks);
                await Task.Delay(TimeSpan.FromSeconds(6));

                var value = await manager.GetKeys();

                //Assert
                Assert.Equal(0, value.Length);
            }

        }
    }
}
