namespace NConsole.Options
{
    /// <summary>
    /// Variable base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class OptionItemBase<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="prototype"></param>
        protected OptionItemBase(string prototype)
        {
            Prototype = prototype;
        }

        /// <summary>
        /// Gets the Prototype.
        /// </summary>
        protected string Prototype { get; private set; }

        /// <summary>
        /// Returns a new <see cref="OptionException"/> with the <paramref name="message"/>
        /// and <see cref="Prototype"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected OptionException ThrowOptionException(string message)
        {
            return new OptionException(message, Prototype, null);
        }
    }
}
