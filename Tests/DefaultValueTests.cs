using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SourceMock.Tests.TestInterfaces;
using SourceMock.Tests.TestInterfaces.Mocks;
using Xunit;

namespace SourceMock.Tests {
    public class DefaultValueTests {
        [Theory]
        [MemberData(nameof(AllCollectionMethods))]
        public void Collection(MethodInfo method) {
            var mock = new MockINeedsCollectionDefaults();

            var result = method.Invoke(mock, null);

            Assert.NotNull(result);
            Assert.Empty((IEnumerable)result!);
        }

        [Fact]
        public void Task_NonGeneric() {
            var mock = new MockINeedsOtherDefaults();

            var task = mock.ExecuteAsync();

            Assert.Same(Task.CompletedTask, task);
        }

        [Fact]
        public async Task Task_Generic() {
            var mock = new MockINeedsOtherDefaults();

            var task = mock.GetStringAsync();

            Assert.NotNull(task);
            Assert.Null(await task);
        }

        [Fact]
        public async Task Task_Collection() {
            var mock = new MockINeedsOtherDefaults();

            var task = mock.GetListAsync();

            Assert.NotNull(task);
            Assert.Empty(await task);
        }

        public static IEnumerable<object[]> AllCollectionMethods { get; } = typeof(INeedsCollectionDefaults)
            .GetMethods()
            .Select(m => new object[] { m })
            .ToList();
    }
}
