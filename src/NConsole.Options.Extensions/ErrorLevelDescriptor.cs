using System.Collections;

namespace NConsole.Options
{
    public class ErrorLevelDescriptor
    {
        /// <summary>
        /// Gets the ErrorLevel.
        /// </summary>
        public int ErrorLevel { get; internal set; }

        private ErrorLevelFilterDelegate FilterCallback { get; }

        /// <summary>
        /// Implicitly Converts the <paramref name="descriptor"/> for evaluation purposes.
        /// </summary>
        /// <param name="descriptor"></param>
        public static implicit operator bool(ErrorLevelDescriptor descriptor) => descriptor.FilterCallback.Invoke();

        internal static ErrorLevelFilterDelegate DefaultFilterCallback => () => true;

        private ErrorLevelDescriptionDelegate DescriptionCallback { get; }

        internal static ErrorLevelDescriptionDelegate DefaultDescriptionCallback => () => null;

        /// <summary>
        /// Gets the Description.
        /// </summary>
        public string Description => DescriptionCallback.Invoke();

        internal ErrorLevelDescriptor(int errorLevel, ErrorLevelFilterDelegate filterCallback
            , ErrorLevelDescriptionDelegate descriptionCallback)
        {
            ErrorLevel = errorLevel;
            FilterCallback = filterCallback;
            DescriptionCallback = descriptionCallback;
        }
    }
}
