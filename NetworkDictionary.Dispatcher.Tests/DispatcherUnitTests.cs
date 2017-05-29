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
        private readonly ManagerOptions _managerOptions;

        public DispatcherUnitTests()
        {
            _managerOptions = new ManagerOptions(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan, 100);
        }

        private IManager GetNewManager()
        {
            return ManagerFactory.CreateManager(_managerOptions);
        }

        [Fact]
        public async void SetValueShouldNotRaiseException()
        {
            //Act
            using (var dispatcher = new Dispatcher(GetNewManager(), true))
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
            using (var dispatcher = new Dispatcher(GetNewManager(), true))
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

        [Fact]
        public async void GetUnexistedValueShouldReturnNull()
        {
            //Assign
            var expectedValue = new GetValueResponseDto { Value = null };
            const string key = "testKey";


            //Act
            using (var dispatcher = new Dispatcher(GetNewManager(), true))
            {
                var value = await dispatcher.GetValue(new GetValueRequestDto{ Key = key });

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }

        [Fact]
        public async void GetExistedValueShouldReturnExpectedValue()
        {
            //Assign
            var expectedValue = new GetValueResponseDto { Value = "testValue" };
            const string key = "testKey";
            
            //Act
            using (var manager = new Dispatcher(GetNewManager(), true))
            {
                await manager.SetValue(new SetValueRequestDto{ Key = key, Value = "testValue" });
                var value = await manager.GetValue(new GetValueRequestDto { Key = key });

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }

        [Fact]
        public async void DeleteExistedKeyValueShouldReturnTrue()
        {
            //Assign
            const string key = "testKey";
            var expectedValue = new DeleteKeyResponseDto{ Deleted = true };

            //Act
            using (var dispatcher = new Dispatcher(GetNewManager(), true))
            {
                await dispatcher.SetValue(new SetValueRequestDto{ Key = key, Value = "testValue" });
                var value = await dispatcher.DeleteKey(new DeleteKeyRequestDto { Key = key });

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }

        [Fact]
        public async void DeleteUnexistedKeyValueShouldReturnFalse()
        {
            //Assign
            const string key = "testKey";
            var expectedValue = new DeleteKeyResponseDto { Deleted = false };

            //Act
            using (var dispatcher = new Dispatcher(GetNewManager(), true))
            {
                var value = await dispatcher.DeleteKey(new DeleteKeyRequestDto { Key = key });

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }

        [Fact]
        public async void GetKeysShouldReturnExpectedValue()
        {
            //Assign
            var expectedValue = new GetKeysResponseDto {Keys = new[] {"expectedKey1", "expectedKey2"}};

            //Act
            using (var dispatcher = new Dispatcher(GetNewManager(), true))
            {
                await dispatcher.SetValue(new[] {
                    new SetValueRequestDto
                    {
                        Key = "expectedKey1",
                        Value = "testValue"
                    },
                    new SetValueRequestDto
                    {
                        Key = "expectedKey2",
                        Value = "testValue"
                    }
                });
                var value = await dispatcher.GetKeys(new GetKeysRequestDto());

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }

        [Fact]
        public async void PacketExecutionShouldReturnExpectedValue()
        {
            //Assign
            var request = new PacketRequestDto
            {
                Actions = new []
                {
                    new PacketRequestItemDto
                    {
                        SetValueRequests = new []
                        {
                            new SetValueRequestDto
                            {
                                Key = "testKey",
                                Value = "testValue"
                            }
                        }
                    },
                    new PacketRequestItemDto
                    {
                        GetValueRequests = new []
                        {
                            new GetValueRequestDto
                            {
                                Key = "testKey"
                            }
                        }
                    },
                }
            };
            var expectedValue = new PacketResponseDto
            {
                Results = new[]
                {
                    new PacketResponseItemDto { },
                    new PacketResponseItemDto
                    {
                        GetValueResponses = new []
                        {
                            new GetValueResponseDto
                            {
                                Value = "testValue"
                            }
                        }
                    },
                }
            };

            //Act
            using (var dispatcher = new Dispatcher(GetNewManager(), true))
            {
                var value = await dispatcher.GetPacketExecutionResult(request);

                //Assert
                Assert.Equal(expectedValue, value);
            }
        }
    }
}
