namespace NConsole.Options
{
    // TODO: TBD: could potentially expose NConsole.Options Characters/Domain for this purpose...
    public static class Domain
    {
        /// <summary>
        /// &apos;,&apos;
        /// </summary>
        internal const char Comma = ',';

        /// <summary>
        /// &apos;=&apos;
        /// </summary>
        internal const char Equal = '=';

        /// <summary>
        /// &apos;:&apos;
        /// </summary>
        internal const char Colon = ':';

        /// <summary>
        /// &apos;-&apos;
        /// </summary>
        internal const char Dash = '-';

        /// <summary>
        /// &apos; &apos;, useful when you have a mash up of arguments in a single
        /// <see cref="string"/> that you want to split.
        /// </summary>
        public const char DefaultArgumentMashUpSeparator = ' ';

        /// <summary>
        /// &quot;--&quot;
        /// </summary>
        /// <see cref="Dash"/>
        internal static readonly string DoubleDash = $"{Dash}{Dash}";
    }
}
