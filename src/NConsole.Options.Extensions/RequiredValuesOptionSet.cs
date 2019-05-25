using System;
using System.Collections.Generic;

namespace NConsole.Options
{
    using static Domain;
    using static OptionValueType;

    // TODO: TBD: this one is borderline a class "extension" that should probably be supported by OptionSet itself...
    // TODO: TBD: i.e. that is reporting any variables that went "missing" during the Parse.
    // TODO: TBD: i.e. especially given the fact that Options can be specified optional/required to begin with...
    /// <summary>
    /// Derives from <see cref="OptionSet"/>, and adds capability for variables that are required.
    /// </summary>
    /// <remarks>http://www.ndesk.org/doc/ndesk-options/NDesk.Options/OptionSet.html</remarks>
    /// <inheritdoc />
    [Obsolete("I think this deserves promotion into NConsole.Options itself, especially given the Optional/Required capability.")]
    public class RequiredValuesOptionSet : OptionSet
    {
        /// <summary>
        /// Gets or Sets whether thrown <see cref="UnprocessedRequiredOptionsException"/>
        /// occurs silently.
        /// </summary>
        public bool SilentUnprocessedOptions { get; set; }

        /// <summary>
        /// Gets the MissingOptions.
        /// </summary>
        /// <see cref="Option"/>
        /// <see cref="IReadOnlyList{T}"/>
        public IReadOnlyList<IOption> MissingOptions { get; private set; } = new Option[] { };

        // TODO: TBD: might be better to say virtual/override Parse.
        // TODO: TBD: for that matter, we might even want a TryParse after all...
        /// <inheritdoc cref="OptionSet.Parse" />
        public new IEnumerable<string> Parse(params string[] args)
        {
            try
            {
                return base.Parse(args);
            }
            // ReSharper disable once IdentifierTypo
            catch (UnprocessedRequiredOptionsException oroex)
            {
                MissingOptions = oroex.UnprocessedOptions;
                if (!SilentUnprocessedOptions)
                {
                    throw;
                }

                return new string[] { };
            }
        }

        /// <summary>
        /// Adds the <see cref="Required"/> <see cref="Variable{T}"/> to the <see cref="OptionSet"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prototype"></param>
        /// <param name="callback"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Variable<T> AddRequiredVariable<T>(string prototype, OptionCallback<T> callback, string description = null)
            => this.AddVariable($"{prototype}{Equal}", callback, description);

        public Variable<T> AddRequiredVariable<T>(string prototype, string description = null)
            => this.AddVariable<T>(prototype, _ => { }, description);

        /// <summary>
        /// Adds the Required <see cref="VariableList{T}"/> to the <see cref="OptionSet"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public VariableList<T> AddRequiredVariableList<T>(string prototype, string description = null)
            => this.AddVariableList<T>(prototype, _ => { }, description);
    }
}
