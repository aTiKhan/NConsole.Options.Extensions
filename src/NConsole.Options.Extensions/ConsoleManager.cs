using System;
using System.IO;
using System.Linq;

namespace NConsole.Options
{
    using static String;

    /// <summary>
    /// Provides basic ConsoleManager details.
    /// </summary>
    public abstract class ConsoleManager
    {
        /// <summary>
        /// &quot;h|help&quot;
        /// </summary>
        public const string DefaultHelpPrototype = "h|help";

        /// <summary>
        /// &quot;Show the help&quot;
        /// </summary>
        public const string DefaultHelpDescription = "Show the help";

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
        public abstract bool TryParseOrShowHelp(TextWriter writer, params string[] args);

        /// <summary>
        /// Runs the Console. Override in order to do something meaningful in response to the
        /// Console running, and ostensibly having invoked <see cref="TryParseOrShowHelp"/>.
        /// </summary>
        public virtual void Run()
        {
        }
    }

    /// <summary>
    /// ConsoleManager class.
    /// </summary>
    /// <inheritdoc cref="ConsoleManager"/>
    public class ConsoleManager<TOptions> : ConsoleManager, IDisposable
        where TOptions : OptionSet
    {
        private Switch HelpSwitch { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class using
        /// a specified value for HelpInfo prototype and description.
        /// </summary>
        /// <param name="consoleName">Name of the console.</param>
        /// <param name="options">An <see cref="OptionSet"/>.</param>
        /// <param name="helpPrototype">The Help prototype. Informs a Switch with its Prototype.</param>
        /// <param name="helpDescription">The help description.</param>
        /// <inheritdoc />
        public ConsoleManager(string consoleName, TOptions options
            , string helpPrototype = DefaultHelpPrototype
            , string helpDescription = DefaultHelpDescription)
        {
            ConsoleName = consoleName;
            HelpSwitch = (Options = options).AddSwitch(helpPrototype, helpDescription);
        }


        /// <summary>
        /// Gets the ConsoleName.
        /// </summary>
        internal string ConsoleName { get; }

        /// <summary>
        /// Gets the Options.
        /// </summary>
        protected TOptions Options { get; }

        /// <inheritdoc />
        public sealed override bool TryParseOrShowHelp(TextWriter writer, params string[] args)
        {
            var parsed = false;

            TextWriter ErrorParsingArguments()
            {
                writer.Write($"{ConsoleName}: error parsing arguments:");
                return writer;
            }

            try
            {
                var remaining = Options.Parse(args).ToArray();
                // ReSharper disable once InvertIf
                if (remaining.Any())
                {
                    string RenderRemainingArguments() => Join(", ", remaining.Select(x => $"`{x}'"));
                    ErrorParsingArguments().WriteLine($" {RenderRemainingArguments()}");
                    return false;
                }

                parsed = true;
            }
            // ReSharper disable once IdentifierTypo
            catch (UnprocessedRequiredOptionsException uroex)
            {
                ErrorParsingArguments();
                HelpSwitch.Enabled = !parsed;
            }

            // ReSharper disable once InvertIf
            if (HelpSwitch.Enabled)
            {
                if (parsed)
                {
                    writer.WriteLine($"{ConsoleName} options:");
                }

                Options.WriteOptionDescriptions(writer);
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
