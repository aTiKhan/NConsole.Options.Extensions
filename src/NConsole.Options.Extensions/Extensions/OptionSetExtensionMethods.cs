using System;

namespace NConsole.Options
{
    using static String;

    /// <summary>
    /// OptionSetExtensions class.
    /// </summary>
    public static class OptionSetExtensionMethods
    {
        /// <summary>
        /// Adds a Switch to the OptionSet.
        /// </summary>
        /// <typeparam name="TOptionSet">Any derived OptionSet.</typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns>The Switch associated with the OptionSet.</returns>
        public static Switch AddSwitch<TOptionSet>(this TOptionSet optionSet
            , string prototype, string description = null)
            where TOptionSet : OptionSet
        {
            /* Switch and not a flag. Switch implies on or off, enabled or disabled,
             * whereas flag implies combinations, masking. */
            var @switch = new Switch();

            if (description == null)
            {
                // Which leaves us injecting a hook into the options.
                optionSet.Add(prototype, () => @switch.Enabled = true);
            }
            else
            {
                // Which leaves us injecting a hook into the options.
                optionSet.Add(prototype, description, () => @switch.Enabled = true);
            }

            // Kinda like having a future, not quite, but real close.
            return @switch;
        }

        /// <summary>
        /// Adds a strongly typed Variable to the OptionSet.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns>The Variable associated with the OptionSet.</returns>
        public static Variable<T> AddVariable<T>(this OptionSet optionSet
            , string prototype, string description = null)
        {
            // We pass an empty method to the addedEventHandler because we don't need anything extra.
            return description == null
                ? AddVariable<T>(optionSet, prototype, _ => { })
                : AddVariable<T>(optionSet, prototype, _ => { }, description);
        }

        /// <summary>
        /// Adds a strongly typed Variable to the OptionSet with the option to execute arbitrary
        /// code when options are parsed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="onAdded">Allows execution of arbitrary code when an option is parsed.</param>
        /// <param name="description"></param>
        /// <returns>The Variable associated with the OptionSet.</returns>
        internal static Variable<T> AddVariable<T>(this OptionSet optionSet
            , string prototype, Action<string> onAdded, string description = null)
        {
            // TODO: TBD: promote Action<string> (or whatever) to first class type...
            // TODO: TBD: I think we might even be able to let the OptionSet/Option do the type conversion for us...
            // TODO: TBD: such that all that remains is really to introduce a really (I mean, REALLY) thin vernier over the OptionSet.

            var variablePrototype = prototype + "=";

            var variable = new Variable<T>(variablePrototype);

            if (description == null)
            {
                optionSet.Add(variablePrototype, x =>
                {
                    variable.Value = Variable<T>.CastString(x);
                    // Perform whatever our downstream callers need to do when an option is parsed.
                    onAdded(variablePrototype);
                });
            }
            else
            {
                optionSet.Add(variablePrototype, description, x =>
                {
                    variable.Value = Variable<T>.CastString(x);
                    // Perform whatever our downstream callers need to do when an option is parsed.
                    onAdded(variablePrototype);
                });
            }

            return variable;
        }

        /// <summary>
        /// Accumulates option values in a strongly-typed Variable list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <returns>The VariableList associated with the OptionSet.</returns>
        public static VariableList<T> AddVariableList<T>(this OptionSet optionSet
            , string prototype, string description = null)
        {
            // We pass nothing to the addedEventHandler because we don't need anything extra.
            return description == null
                ? AddVariableList<T>(optionSet, prototype, none => { })
                : AddVariableList<T>(optionSet, prototype, none => { }, description);
        }

        /// <summary>
        /// Accumulates option values in a strongly-typed Variable list with the option to execute
        /// arbitrary code when options are parsed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="onAdded">Allows execution of arbitrary code when an option is parsed.</param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <returns>The VariableList associated with the OptionSet.</returns>
        internal static VariableList<T> AddVariableList<T>(this OptionSet optionSet
            , string prototype, Action<string> onAdded, string description = null)
        {
            // TODO: TBD: may include OptionValueType comprehension here... i.e. Required/Optional (or 'blank')...
            var variablePrototype = prototype + "=";

            var variable = new VariableList<T>(variablePrototype);

            // ReSharper disable InconsistentNaming
            if (description == null)
            {
                optionSet.Add(variablePrototype, x =>
                {
                    var x_Value = Variable<T>.CastString(x);
                    variable.ValuesList.Add(x_Value);

                    // Perform whatever our downstream callers need to do when an option is parsed.
                    onAdded(variablePrototype);
                });
            }
            else
            {
                optionSet.Add(variablePrototype, description, x =>
                {
                    var x_Value = Variable<T>.CastString(x);
                    variable.ValuesList.Add(x_Value);

                    // Perform whatever our downstream callers need to do when an option is parsed.
                    onAdded(variablePrototype);
                });
            }
            // ReSharper restore InconsistentNaming

            return variable;
        }

        /// <summary>
        /// Accumulates options into a strongly-typed VariableMatrix.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns>The VariableMatrix associated with the OptionSet.</returns>
        public static VariableMatrix<T> AddVariableMatrix<T>(this OptionSet optionSet
            , string prototype, string description = null)
        {
            // TODO: TBD: potentially ditto: OptionValueType ...
            var variablePrototype = prototype + ":";

            var variable = new VariableMatrix<T>(variablePrototype);

            // Key/value pairs are indeed parsed out of the args list.
            // ReSharper disable InconsistentNaming
            if (description == null)
            {
                optionSet.Add(variablePrototype, (k, x) =>
                {
                    if (IsNullOrEmpty(k))
                    {
                        throw new OptionException("Name not specified", variablePrototype);
                    }

                    var x_Value = Variable<T>.CastString(x);

                    // Utilize the InternalMatrix for purposes of this one.
                    variable.InternalMatrix.Add(k, x_Value);
                });
            }
            else
            {
                optionSet.Add(variablePrototype, description, (k, x) =>
                {
                    if (IsNullOrEmpty(k))
                    {
                        throw new OptionException("Name not specified", variablePrototype);
                    }

                    var x_Value = Variable<T>.CastString(x);

                    // Utilize the InternalMatrix for purposes of this one.
                    variable.InternalMatrix.Add(k, x_Value);
                });
            }
            // ReSharper restore InconsistentNaming

            return variable;
        }

    }
}
