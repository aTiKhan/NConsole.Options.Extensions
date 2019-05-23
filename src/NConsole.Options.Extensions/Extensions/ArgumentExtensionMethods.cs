using System.Collections.Generic;

namespace NConsole.Options
{
    using static Domain;

    public static class ArgumentExtensionMethods
    {
        /// <summary>
        /// Splits the <paramref name="args"/> using the
        /// <see cref="DefaultArgumentMashUpSeparator"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <see cref="DefaultArgumentMashUpSeparator"/>
        public static string[] SplitArgumentMashUp(this string args) => args.Split(DefaultArgumentMashUpSeparator);
    }
}
