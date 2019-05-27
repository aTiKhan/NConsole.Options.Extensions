using System.Collections.Generic;
using System.Linq;

namespace NConsole.Options
{
    using static TestFixtureBase;

    internal class MaySpecifyPrototypeTestCases : PrototypeExtensionMethodTestCasesBase
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
                    yield return GetRange<object>(BaseCase, BaseCaseOptional).ToArray();
                    yield return GetRange<object>(BaseCaseRequired, BaseCaseOptional).ToArray();
                    yield return GetRange<object>(BaseCaseOptional, BaseCaseOptional).ToArray();
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }
    }
}
