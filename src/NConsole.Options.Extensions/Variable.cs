namespace NConsole.Options
{
    /// <inheritdoc />
    public class Variable<T> : OptionItemBase<T>
    {
        /// <summary>
        /// Implicitly converts the <see cref="Value"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static implicit operator T(Variable<T> instance) => instance.Value;

        /// <inheritdoc />
        internal Variable(string prototype)
            : base(prototype)
        {
        }

        /// <summary>
        /// Gets the Value.
        /// </summary>
        public T Value { get; internal set; } = default(T);
    }
}
