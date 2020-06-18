using System.Collections;
using System.Collections.Generic;

namespace NConsole.Options
{
    /// <inheritdoc />
    internal abstract class TestCasesBase : IEnumerable<object[]>
    {
        protected abstract IEnumerable<object[]> Cases { get; }

        public IEnumerator<object[]> GetEnumerator() => Cases.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
