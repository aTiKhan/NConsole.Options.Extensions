using System;
using System.Linq;

namespace NConsole.Options
{
    using static Domain;
    using static String;
    using static OptionValueType;

    public static class PrototypeExtensionMethods
    {
        internal static string AppendRequiredOrOptional(this string prototype, char requiredOrOptional)
            => !IsNullOrEmpty(prototype) && $"{Colon}{Equal}".Contains(prototype.Last())
                ? prototype
                : $"{prototype}{requiredOrOptional}";

        // TODO: TBD: these might even make sense in the NConsole.Options package...
        /// <summary>
        /// Appends <paramref name="actualRequiredOrOptional"/> to <paramref name="prototype"/>
        /// as indicated by the caller. Null or <see cref="Empty"/> are likely not what you want
        /// to specify for a <paramref name="prototype"/>, but we will allow for it.
        /// </summary>
        /// <param name="prototype"></param>
        /// <param name="actualRequiredOrOptional"></param>
        /// <returns></returns>
        /// <see cref="MustSpecify"/>
        /// <see cref="MaySpecify"/>
        private static string SpecifyRequiredOrOptional(this string prototype, char actualRequiredOrOptional)
            => prototype?.Any() != true // Could also be Null.
                ? prototype
                : $"{RequiredOrOptional}".Contains(prototype.Last())
                    ? $"{prototype.Substring(0, prototype.Length - 1)}{actualRequiredOrOptional}"
                    : $"{prototype}{actualRequiredOrOptional}";

        /// <summary>
        /// Appends the <paramref name="prototype"/> with the <see cref="Required"/>
        /// specification.
        /// </summary>
        /// <param name="prototype"></param>
        /// <returns></returns>
        /// <see cref="Equal"/>
        public static string MustSpecify(this string prototype) => prototype.SpecifyRequiredOrOptional(Equal);

        /// <summary>
        /// Appends the <paramref name="prototype"/> with the <see cref="Optional"/>
        /// specification.
        /// </summary>
        /// <param name="prototype"></param>
        /// <returns></returns>
        /// <see cref="Colon"/>
        public static string MaySpecify(this string prototype) => prototype.SpecifyRequiredOrOptional(Colon);
    }
}
