using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NConsole.Options
{
    /// <summary>
    /// We now implement <see cref="IReadOnlyDictionary{TKey,TValue}"/> which was a better fit
    /// for the Matrix application all along. No need to worry about throwing anything, and can
    /// fairly simply extend the internal dictionary with the latest language features.
    /// </summary>
    /// <inheritdoc cref="OptionItemBase{T}"/>
    public class VariableMatrix<T> : OptionItemBase<T>, IReadOnlyDictionary<string, T>
    {
        /// <inheritdoc />
        internal VariableMatrix(string prototype)
            : base(prototype)
        {
        }

        /// <summary>
        /// Gets the Matrix for Internal use.
        /// </summary>
        internal IDictionary<string, T> InternalMatrix { get; } = new Dictionary<string, T>();

        /// <summary>
        /// <see cref="Matrix"/> backing field.
        /// </summary>
        private IReadOnlyDictionary<string, T> _matrix;

        /// <summary>
        /// Gets the Matrix. Which really means Getting Itself.
        /// </summary>
        public IReadOnlyDictionary<string, T> Matrix =>
            _matrix ?? (_matrix
                = new ReadOnlyDictionary<string, T>(InternalMatrix)
            );

        /// <summary>
        /// Returns the <typeparamref name="TResult"/> given <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private TResult DictionaryFunc<TResult>(Func<IReadOnlyDictionary<string, T>, TResult> func) => func(Matrix);

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => DictionaryFunc(x => x.GetEnumerator());

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public int Count => DictionaryFunc(x => x.Count);

        /// <inheritdoc />
        public bool ContainsKey(string key) => DictionaryFunc(x => x.ContainsKey(key));

        /// <inheritdoc />
        public bool TryGetValue(string key, out T value) => Matrix.TryGetValue(key, out value);

        /// <inheritdoc />
        public T this[string key] => DictionaryFunc(x => x[key]);

        /// <inheritdoc />
        public IEnumerable<string> Keys => DictionaryFunc(x => x.Keys);

        /// <inheritdoc />
        public IEnumerable<T> Values => DictionaryFunc(x => x.Values);
    }
}
