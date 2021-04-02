using System.Collections.Generic;
using System.Linq;

namespace SourceMock.Internal {
    public class MockHandler {
        private readonly IList<IMockMethodSetup> _setups = new List<IMockMethodSetup>();

        public MockMethodSetup<TReturn> Setup<TReturn>(params IMockArgument[] arguments) {
            var setup = new MockMethodSetup<TReturn>(arguments);
            _setups.Add(setup);
            return setup;
        }

        public TReturn Call<TReturn>(params object?[] arguments) {
            var setup = _setups.FirstOrDefault(
                s => s.Arguments.Zip(arguments, (expected, actual) => expected.Matches(actual)).All(m => m)
            );
            if (setup == null || !setup.HasReturnValue)
                return default!;

            return (TReturn)setup.ReturnValue!;
        }
    }
}
