﻿using System;
using System.Collections.Generic;
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
        /// <param name="optionSet">An OptionSet.</param>
        /// <param name="helpPrototype">The Help prototype. Informs a Switch with its Prototype.</param>
        /// <param name="helpDescription">The help description.</param>
        /// <inheritdoc />
        public ConsoleManager(string consoleName, RequiredValuesOptionSet optionSet
            , string helpPrototype = "h|help", string helpDescription = "Show the help")
            : this(optionSet, consoleName, new HelpInfo(optionSet, helpPrototype, helpDescription))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class.
        /// </summary>
        /// <param name="optionSet">An OptionSet.</param>
        /// <param name="consoleName">Name of the console.</param>
        /// <param name="helpInfo">The help info.</param>
        private ConsoleManager(RequiredValuesOptionSet optionSet, string consoleName, HelpInfo helpInfo)
        {
            ConsoleName = consoleName;
            _optionSet = optionSet;
            _helpInfo = helpInfo;
        }

        /// <summary>
        /// Requirements backin field.
        /// </summary>
        private readonly List<Requirement> _requirements = new List<Requirement>();

        //TODO: TBD: Could potentially be an observable collection.
        /// <summary>
        /// Gets the Requirements.
        /// </summary>
        public List<Requirement> Requirements
        {
            get { return _requirements; }
        }

        /// <summary>
        /// HelpInfo backing field.
        /// </summary>
        private readonly HelpInfo _helpInfo;

        /// <summary>
        /// Gets the ConsoleName.
        /// </summary>
        internal string ConsoleName { get; private set; }

        /// <summary>
        /// OptionSet backing field.
        /// </summary>
        private readonly RequiredValuesOptionSet _optionSet;

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
            var remaining = _optionSet.Parse(args);

            //Not-parsed determined here.
            var parsed = !(remaining.Any()
                           || _optionSet.GetMissingVariables().Any()
                           || _helpInfo.Help.Enabled);

            //Show-error when any-remaining or missing-variables.
            if (remaining.Any() || _optionSet.GetMissingVariables().Any())
                writer.WriteLine("{0}: error parsing arguments:", ConsoleName);
            else if (!parsed)
                writer.WriteLine("{0} options:", ConsoleName);

            //Show-help when not-parsed.
            if (!parsed)
                _optionSet.WriteOptionDescriptions(writer);

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
