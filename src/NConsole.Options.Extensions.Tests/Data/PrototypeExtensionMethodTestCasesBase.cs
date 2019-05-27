namespace NConsole.Options
{
    using static Domain;

    internal abstract class PrototypeExtensionMethodTestCasesBase : TestCasesBase
    {
        /// <summary>
        /// Literally, Null.
        /// </summary>
        protected const string NullCase = null;

        /// <summary>
        /// &quot;&quot;
        /// </summary>
        protected const string EmptyCase = "";

        /// <summary>
        /// &quot;a|alpha&quot;
        /// </summary>
        protected const string BaseCase = "a|alpha";

        /// <summary>
        /// &quot;a|alpha=&quot;
        /// </summary>
        /// <see cref="Equal"/>
        protected string BaseCaseRequired { get; } = $"{BaseCase}{Equal}";

        /// <summary>
        /// &quot;a|alpha:&quot;
        /// </summary>
        /// <see cref="Colon"/>
        protected string BaseCaseOptional { get; } = $"{BaseCase}{Colon}";
    }
}
