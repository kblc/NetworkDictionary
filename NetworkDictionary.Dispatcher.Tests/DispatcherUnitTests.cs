using System;
using System.Threading;
using NetworkDictionary.Domain.Dto;
using NetworkDictionary.Manager;
using NetworkDictionary.Manager.Interfaces;
using Xunit;

namespace NetworkDictionary.Dispatcher.Tests
{
    /// <summary>
    /// Unit tests for <see cref="Dispatcher"/>
    /// </summary>
    public class DispatcherUnitTests
    {
        private readonly IManager _manager;

        public DispatcherUnitTests()
        {
            var managerOptions = new ManagerOptions(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan, 100);
            _manager = ManagerFactory.CreateManager(managerOptions);
        }

        [Fact]
        public async void SetValueShouldNotRaiseException()
        {
            //Act
            using (var dispatcher = new Dispatcher(_manager))
            {
                await dispatcher.SetValue(new SetValueRequestDto
                {
                    Value = "testValue",
                    Key = "testKey"
                });
            }
        }

        [Fact]
        public async void SetExistetKeyValueShouldNotRaiseException()
        {
            //Act
            using (var dispatcher = new Dispatcher(_manager))
            {
                await dispatcher.SetValue(new SetValueRequestDto
                {
                    Value = "testValue",
                    Key = "testKey"
                });
                await dispatcher.SetValue(new SetValueRequestDto
                {
                    Value = "testValue",
                    Key = "testKey2"
                });
            }
        }
    }
}
