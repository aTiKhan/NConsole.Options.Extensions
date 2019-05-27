using System.IO;

namespace NConsole.Options
{
    /// <summary>
    /// Provides an <see cref="OptionSet"/> driven <see cref="ConsoleManager{T}"/>.
    /// </summary>
    /// <inheritdoc />
    public class OptionSetConsoleManager : ConsoleManager<OptionSet>
    {
        public OptionSetConsoleManager(string consoleName, TextWriter writer
            , string helpPrototype = DefaultHelpPrototype, string helpDescription = DefaultHelpDescription
            , TextWriter errorWriter = null)
            : this(consoleName, writer, new OptionSet(), helpPrototype, helpDescription, errorWriter)
        {
        }

        public OptionSetConsoleManager(string consoleName, TextWriter writer, OptionSet options
            , string helpPrototype = DefaultHelpPrototype, string helpDescription = DefaultHelpDescription
            , TextWriter errorWriter = null)
            : base(consoleName, writer, options, helpPrototype, helpDescription, errorWriter)
        {
        }
    }
}
