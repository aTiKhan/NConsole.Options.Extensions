using System.Collections;
using System.Collections.Generic;

namespace NConsole.Options
{
    /// <inheritdoc cref="OptionItemBase{T}"/>
    public class VariableList<T> : OptionItemBase<T>, IEnumerable<T>
    {
        /// <inheritdoc />
        internal VariableList(string prototype)
            : base(prototype)
        {
        }

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        /// <summary>
        /// Gets the <see cref="List{T}"/> for Internal use.
        /// </summary>
        internal List<T> InternalValues { get; } = new List<T> { };

        /// <summary>
        /// Gets the Values.
        /// </summary>
        /// <see cref="InternalValues"/>
        public IReadOnlyList<T> Values => InternalValues;

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
