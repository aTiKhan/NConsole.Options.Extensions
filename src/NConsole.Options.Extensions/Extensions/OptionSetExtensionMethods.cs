using System;

namespace NConsole.Options
{
    using static Domain;
    using static String;

    /// <summary>
    /// <see cref="OptionSet"/> extension methods.
    /// </summary>
    public static class OptionSetExtensionMethods
    {
        private static OptionCallback GetDefaultSimpleOptionCallback() => () => { };

        /// <summary>
        /// Adds a <see cref="Switch"/> to the <see cref="OptionSet"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="prototype"></param>
        /// <param name="callback"></param>
        /// <param name="description"></param>
        /// <returns>The <see cref="Switch"/> associated with the <paramref name="options"/>.</returns>
        public static Switch AddSwitch(this OptionSet options, string prototype
            , OptionCallback callback, string description = null)
        {
            /* Switch and not a flag. Switch implies on or off, enabled or disabled,
             * whereas flag implies combinations, masking. */
            var result = new Switch();

            options.Add(prototype, description, () =>
            {
                result.Enabled = true;
                callback?.Invoke();
            });

            // Return the near-future Switch instance.
            return result;
        }

        /// <summary>
        /// Adds a <see cref="Switch"/> to the <see cref="OptionSet"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns>The <see cref="Switch"/> associated with the <paramref name="options"/>.</returns>
        public static Switch AddSwitch(this OptionSet options, string prototype, string description = null)
            => AddSwitch(options, prototype, GetDefaultSimpleOptionCallback(), description);

        private static OptionCallback<T> GetDefaultTargetOptionCallback<T>() => _ => { };

        /// <summary>
        /// Adds a strongly typed <see cref="Variable{T}"/> to the <paramref name="options"/>
        /// with the option to execute arbitrary code when arguments are parsed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="prototype"></param>
        /// <param name="callback"></param>
        /// <param name="description"></param>
        /// <returns>The <see cref="Variable{T}"/> associated with the <paramref name="options"/>.</returns>
        internal static Variable<T> AddVariable<T>(this OptionSet options, string prototype
            , OptionCallback<T> callback, string description = null)
        {
            // TODO: TBD: but really and truly, why are we mucking around with optional/required at this level? It is such an inherent part of the prototype, just let the user handle that part
            // TODO: TBD: while this library focuses on simply wrapping the concerns in variable, variable list, variable matrix, and so on, consequences be damned...
            // TODO: TBD: should expose bits like `=' and `:' from the NConsole.Options Domain assets...
            prototype = prototype.AppendRequiredOrOptional(Equal);

            var result = new Variable<T>(prototype);

            options.Add<T>(prototype, description, x =>
            {
                result.Value = x;
                callback(x);
            });

            // Return with the near-future Variable instance.
            return result;
        }

        /// <summary>
        /// Adds a strongly-typed <see cref="Variable{T}"/> to the <paramref name="options"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns>The <see cref="Variable{T}"/> associated with the <paramref name="options"/>.</returns>
        public static Variable<T> AddVariable<T>(this OptionSet options, string prototype, string description = null)
            => AddVariable(options, prototype, GetDefaultTargetOptionCallback<T>(), description);

        /// <summary>
        /// Accumulates option values in a strongly-typed <see cref="VariableList{T}"/> with
        /// the <see cref="Option"/> to execute arbitrary code when arguments are parsed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="prototype"></param>
        /// <param name="callback">Allows execution of arbitrary code when an argument is parsed.</param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <returns>The VariableList associated with the OptionSet.</returns>
        internal static VariableList<T> AddVariableList<T>(this OptionSet options, string prototype
            , OptionCallback<T> callback, string description = null)
        {
            // TODO: TBD: may include OptionValueType comprehension here... i.e. Required/Optional (or 'blank')...
            prototype = prototype.AppendRequiredOrOptional(Equal);

            var result = new VariableList<T>(prototype);

            options.Add<T>(prototype, description, x =>
            {
                result.InternalValues.Add(x);
                callback(x);
            });

            // Return with the near-future Variable instance.
            return result;
        }

        /// <summary>
        /// Accumulates <typeparamref name="T"/> values in a strongly-typed
        /// <see cref="VariableList{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <returns>The <see cref="VariableList{T}"/> associated with the <paramref name="options"/>.</returns>
        public static VariableList<T> AddVariableList<T>(this OptionSet options, string prototype, string description = null)
            => AddVariableList(options, prototype, GetDefaultTargetOptionCallback<T>(), description);

        private static OptionCallback<TKey, TValue> GetDefaultKeyValuePairOptionCallback<TKey, TValue>() => (_, __) => { };

        /// <summary>
        /// Accumulates <typeparamref name="T"/> into a strongly-typed
        /// <see cref="VariableMatrix{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="prototype"></param>
        /// <param name="callback"></param>
        /// <param name="description"></param>
        /// <returns>The <see cref="VariableMatrix{T}"/> associated with the
        /// <paramref name="options"/>.</returns>
        public static VariableMatrix<T> AddVariableMatrix<T>(this OptionSet options, string prototype
            , OptionCallback<string, T> callback, string description = null)
        {
            // TODO: TBD: potentially ditto: OptionValueType ...
            prototype = prototype.AppendRequiredOrOptional(Colon);

            var result = new VariableMatrix<T>(prototype);

            options.Add<string, T>(prototype, description, (k, x) =>
            {
                if (IsNullOrEmpty(k))
                {
                    throw new OptionException("Name not specified", prototype, null);
                }

                result.InternalMatrix.Add(k, x);
                callback?.Invoke(k, x);
            });

            // Return with the near-future Variable instance.
            return result;
        }

        /// <summary>
        /// Accumulates <typeparamref name="T"/> into a strongly-typed
        /// <see cref="VariableMatrix{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns>The <see cref="VariableMatrix{T}"/> associated with the
        /// <paramref name="options"/>.</returns>
        public static VariableMatrix<T> AddVariableMatrix<T>(this OptionSet options, string prototype, string description = null)
            => AddVariableMatrix(options, prototype, GetDefaultKeyValuePairOptionCallback<string, T>(), description);
    }
}
