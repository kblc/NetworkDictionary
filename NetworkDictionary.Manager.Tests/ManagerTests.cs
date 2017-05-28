using System;
using System.Threading;
using NetworkDictionary.Manager.Interfaces;
using Xunit;

namespace NetworkDictionary.Manager.Tests
{
    /// <summary>
    /// Unit tests for <see cref="Manager"/>
    /// </summary>
    public class ManagerTests : IClassFixture<ManagerTestFixture>
    {
        private readonly IManager _manager;

        public ManagerTests(ManagerTestFixture fixture) { _manager = fixture.Manager; }

        [Fact]
        public async void SetValueShouldNotRaiseException()
        {
            //Assign
            var key = Guid.NewGuid().ToString("N");

            //Act
            await _manager.SetValue(key, "testValue", Timeout.InfiniteTimeSpan);
        }

        [Fact]
        public async void SetExistetKeyValueShouldNotRaiseException()
        {   
            //Assign
            var key = Guid.NewGuid().ToString("N");

            //Act
            await _manager.SetValue(key, "testValue");
            await _manager.SetValue(key, "testValue2");
        }

        [Fact]
        public async void GetUnexistedValueShouldReturnNull()
        {
            //Assign
            var key = Guid.NewGuid().ToString("N");

            //Act
            var value = await _manager.GetValue(key);

            //Assert
            Assert.Null(value);
        }

        [Fact]
        public async void GetExistedValueShouldReturnExpectedValue()
        {
            //Assign
            var key = Guid.NewGuid().ToString("N");
            const string expectedValue = "testValue";

            //Act
            await _manager.SetValue(key, "testValue");
            var value = await _manager.GetValue(key);

            //Assert
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public async void DeleteExistedKeyValueShouldReturnTrue()
        {
            //Assign
            var key = Guid.NewGuid().ToString("N");
            const bool expectedValue = true;

            //Act
            await _manager.SetValue(key, "testValue");
            bool value = await _manager.DeleteValue(key);

            //Assert
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public async void DeleteUnexistedKeyValueShouldReturnFalse()
        {
            //Assign
            var key = Guid.NewGuid().ToString("N");
            const bool expectedValue = false;

            //Act
            var value = await _manager.DeleteValue(key);

            //Assert
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public async void GetKeysShouldReturnExpectedValue()
        {
            //Assign
            var expectedValue = new [] { "expectedKey1", "expectedKey2" };

            //Act
            await _manager.SetValue("expectedKey1", "testValue");
            await _manager.SetValue("expectedKey2", "testValue");
            var value = await _manager.GetKeys();

            //Assert
            Assert.Equal(expectedValue, value);
        }
    }

    /// <summary>
    /// Initialize fixture for every test in <see cref="ManagerTests"/>
    /// </summary>
    public class ManagerTestFixture
    {
        /// <summary>
        /// Manager
        /// </summary>
        public IManager Manager { get; }

        /// <summary>
        /// Create instance
        /// </summary>
        public ManagerTestFixture()
        {
            Manager = new Manager(new ManagerOptions
            {
                MaxKeyCount = 100000,
                DefaultTtl = Timeout.InfiniteTimeSpan
            });
        }
    }
}
