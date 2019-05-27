using System.Collections.Generic;
using System.Linq;

namespace NConsole.Options
{
    using static TestFixtureBase;

    internal class MustSpecifyPrototypeTestCases : PrototypeExtensionMethodTestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        protected override IEnumerable<object[]> Cases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    yield return GetRange<object>(NullCase, NullCase).ToArray();
                    yield return GetRange<object>(EmptyCase, EmptyCase).ToArray();
                    yield return GetRange<object>(BaseCase, BaseCaseRequired).ToArray();
                    yield return GetRange<object>(BaseCaseOptional, BaseCaseRequired).ToArray();
                    yield return GetRange<object>(BaseCaseRequired, BaseCaseRequired).ToArray();
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }
    }
}
