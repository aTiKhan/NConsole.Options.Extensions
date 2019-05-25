using System;
using System.IO;
using System.Linq;

namespace NConsole.Options
{
    /// <summary>
    /// ConsoleManager class.
    /// </summary>
    /// <inheritdoc />
    public class ConsoleManager : IDisposable
    {
        /// <summary>
        /// &quot;h|help&quot;
        /// </summary>
        public const string DefaultHelpPrototype = "h|help";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class using
        /// a specified value for HelpInfo prototype and description.
        /// </summary>
        /// <param name="consoleName">Name of the console.</param>
        /// <param name="options">An <see cref="OptionSet"/>.</param>
        /// <param name="helpPrototype">The Help prototype. Informs a Switch with its Prototype.</param>
        /// <param name="helpDescription">The help description.</param>
        /// <inheritdoc />
        public ConsoleManager(string consoleName, RequiredValuesOptionSet options
            , string helpPrototype = "h|help", string helpDescription = "Show the help")
            : this(options, consoleName, new HelpInfo(options, helpPrototype, helpDescription))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class.
        /// </summary>
        /// <param name="options">An <see cref="OptionSet"/>.</param>
        /// <param name="consoleName">Name of the console.</param>
        /// <param name="helpInfo">The help info.</param>
        private ConsoleManager(RequiredValuesOptionSet options, string consoleName, HelpInfo helpInfo)
        {
            ConsoleName = consoleName;
            _options = options;
            _helpInfo = helpInfo;
        }

        /// <summary>
        /// HelpInfo backing field.
        /// </summary>
        private readonly HelpInfo _helpInfo;

        /// <summary>
        /// Gets the ConsoleName.
        /// </summary>
        internal string ConsoleName { get; }

        /// <summary>
        /// Options backing field.
        /// </summary>
        private readonly RequiredValuesOptionSet _options;

        /// <summary>
        /// Parses the Command-Line Args or Shows the Help, whichever is appropriate.
        /// Appropriateness is determined by whether there are Remaining Args, or
        /// whether the Help option was specified.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="args"></param>
        /// <returns>true when parsing was successful and no help was requested.</returns>
        /// <remarks>Which, by simplifying the model in SOLID-, DRY-style, the need
        /// for many in the way of helpers vanishes altogether.</remarks>
        public bool TryParseOrShowHelp(TextWriter writer, params string[] args)
        {
            if (!_options.SilentUnprocessedOptions)
            {
                _options.SilentUnprocessedOptions = true;
            }

            var remaining = _options.Parse(args).ToArray();

            // TODO: TBD: we may even want to connect with the Missing Options exception.
            // Not-parsed determined here.
            var parsed = !(remaining.Any() || _options.MissingOptions.Any() || _helpInfo.Help.Enabled);

            // Show-error when any-remaining or missing-variables.
            if (remaining.Any() || _options.MissingOptions.Any())
            {
                writer.WriteLine("{0}: error parsing arguments:", ConsoleName);
            }
            else if (!parsed)
            {
                writer.WriteLine("{0} options:", ConsoleName);
            }

            // Show-help when not-parsed.
            if (!parsed)
            {
                _options.WriteOptionDescriptions(writer);
            }

            return parsed;
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Gets whether IsDisposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
        }
    }
}
