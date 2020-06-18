namespace NConsole.Options
{
    using Xunit;
    using Xunit.Abstractions;

    public class PrototypeTests : TestFixtureBase
    {
        public PrototypeTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private delegate string PrototypeSpecificationDelegate(string prototype);

        /// <summary>
        /// Null or <see cref="string.Empty"/> are likely not what you want to specify
        /// as a <paramref name="prototype"/>, but we will allow for it.
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="expectedPrototype"></param>
        /// <param name="callback"></param>
        /// <see cref="PrototypeExtensionMethodTestCasesBase.NullCase"/>
        /// <see cref="PrototypeExtensionMethodTestCasesBase.EmptyCase"/>
        private static void VerifyPrototypeSpecification(string prototype, string expectedPrototype
            , PrototypeSpecificationDelegate callback)
            => callback.AssertNotNull().Invoke(prototype).AssertEqual(expectedPrototype);

        // ReSharper disable once IdentifierTypo
        /// <summary>
        /// Verifies that <paramref name="prototype"/> May be Specified is rendered properly.
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="expectedPrototype"></param>
        /// <see cref="MaySpecifyPrototypeTestCases"/>
        [Theory
            , ClassData(typeof(MaySpecifyPrototypeTestCases))
            ]
        public void MaySpecify_Prototye_Correct(string prototype, string expectedPrototype)
            => VerifyPrototypeSpecification(prototype, expectedPrototype, p => p.MaySpecify());

        // ReSharper disable once IdentifierTypo
        /// <summary>
        /// Verifies that <paramref name="prototype"/> Must be Specified is rendered properly.
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="expectedPrototype"></param>
        /// <see cref="MustSpecifyPrototypeTestCases"/>
        [Theory
            , ClassData(typeof(MustSpecifyPrototypeTestCases))
            ]
        public void MustSpecify_Prototye_Correct(string prototype, string expectedPrototype)
            => VerifyPrototypeSpecification(prototype, expectedPrototype, p => p.MustSpecify());
    }
}
