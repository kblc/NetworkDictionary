using System;
using System.Threading;
using Xunit;

namespace NetworkDictionary.Manager.Tests
{
    /// <summary>
    /// Unit tests for <see cref="Manager"/>
    /// </summary>
    public class ManagerTests
    {
        private readonly ManagerOptions _managerOptions;

        public ManagerTests()
        {
            _managerOptions = new ManagerOptions(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan, 100);
        }

        [Fact]
        public async void SetValueShouldNotRaiseException()
        {
            //Assign
            const string key = "testKey";

            //Act
            using (var manager = ManagerFactory.CreateManager(_managerOptions))
            {
                await manager.SetValue(key, "testValue", Timeout.InfiniteTimeSpan);
            }
        }

        [Fact]
        public async void SetExistetKeyValueShouldNotRaiseException()
        {
            //Assign
            const string key = "testKey";

            //Act
            using (var manager = ManagerFactory.CreateManager(_managerOptions))
            {
                await manager.SetValue(key, "testValue");
                await manager.SetValue(key, "testValue2");
            }
        }

        [Fact]
        public async void GetUnexistedValueShouldReturnNull()
        {
            //Assign
            const string key = "testKey";

            //Act
            using (var manager = ManagerFactory.CreateManager(_managerOptions))
            {
                var value = await manager.GetValue(key);

                //Assert
                Assert.Null(value);
            }
        }

        [Fact]
        public async void GetExistedValueShouldReturnExpectedValue()
        {
            //Assign
            const string expectedValue = "testValue";
            const string key = "testKey";

            //Act
            using (var manager = ManagerFactory.CreateManager(_managerOptions))
            {
                await manager.SetValue(key, "testValue");
                var value = await manager.GetValue(key);

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }

        [Fact]
        public async void DeleteExistedKeyValueShouldReturnTrue()
        {
            //Assign
            const string key = "testKey";
            const bool expectedValue = true;

            //Act
            using (var manager = ManagerFactory.CreateManager(_managerOptions))
            {
                await manager.SetValue(key, "testValue");
                var value = await manager.DeleteValue(key);

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }

        [Fact]
        public async void DeleteUnexistedKeyValueShouldReturnFalse()
        {
            //Assign
            const string key = "testKey";
            const bool expectedValue = false;

            //Act
            using (var manager = ManagerFactory.CreateManager(_managerOptions))
            {
                var value = await manager.DeleteValue(key);

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }

        [Fact]
        public async void GetKeysShouldReturnExpectedValue()
        {
            //Assign
            var expectedValue = new [] { "expectedKey1", "expectedKey2" };

            //Act
            using (var manager = ManagerFactory.CreateManager(_managerOptions))
            {
                await manager.SetValue("expectedKey1", "testValue");
                await manager.SetValue("expectedKey2", "testValue");
                var value = await manager.GetKeys();

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }
    }
}
