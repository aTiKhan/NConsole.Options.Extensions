using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NConsole.Options
{
    // TODO: TBD: this one is borderline a class "extension" that should probably be supported by OptionSet itself...
    // TODO: TBD: i.e. that is reporting any variables that went "missing" during the Parse.
    // TODO: TBD: i.e. especially given the fact that Options can be specified optional/required to begin with...
    /// <summary>
    /// Derives from OptionSet, and adds capability for variables that are required.
    /// </summary>
    /// <remarks>http://www.ndesk.org/doc/ndesk-options/NDesk.Options/OptionSet.html</remarks>
    [Obsolete("I think this deserves promotion into NConsole.Options itself, especially given the Optional/Required capability.")]
    public class RequiredValuesOptionSet : OptionSet
    {
        public IEnumerable<Option> GetMissingVariables()
        {
            // get items in dictionary where there is no entry
            var q = from t in _requiredVariableValues
                    join o in this as KeyedCollection<string, Option> on t.Key equals o.Prototype
                    where t.Value == false
                    select o;

            return q;
        }

        /// <summary>
        /// Dictionary that holds whether or not prototype variables have been set
        /// </summary>
        private readonly Dictionary<string, bool> _requiredVariableValues = new Dictionary<string, bool>();

        /// <summary>
        /// Adds the Required Variable to the OptionSet.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Variable<T> AddRequiredVariable<T>(string prototype, string description = null)
        {
            _requiredVariableValues.Add(prototype + "=", false);
            return this.AddVariable<T>(prototype, variablePrototype => { this._requiredVariableValues[variablePrototype] = true; }, description);
        }

        /// <summary>
        /// Adds the Required VariableList to the OptionSet.
        /// </summary>
        /// <typeparam name="TVariable"></typeparam>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public VariableList<TVariable> AddRequiredVariableList<TVariable>(string prototype, string description = null)
        {
            _requiredVariableValues.Add(prototype + "=", false);
            return this.AddVariableList<TVariable>(prototype, variablePrototype => { this._requiredVariableValues[variablePrototype] = true; }, description);
        }
    }
}
