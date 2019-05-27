namespace NConsole.Options
{
    /// <summary>
    /// Provides an <see cref="OptionSet"/> driven <see cref="ConsoleManager{T}"/>.
    /// </summary>
    /// <inheritdoc />
    public class OptionSetConsoleManager : ConsoleManager<OptionSet>
    {
        public OptionSetConsoleManager(string consoleName
            , string helpPrototype = DefaultHelpPrototype
            , string helpDescription = DefaultHelpDescription)
            : this(consoleName, new OptionSet(), helpPrototype, helpDescription)
        {
        }

        public OptionSetConsoleManager(string consoleName
            , OptionSet options, string helpPrototype = DefaultHelpPrototype
            , string helpDescription = DefaultHelpDescription)
            : base(consoleName, options, helpPrototype, helpDescription)
        {
        }
    }
}
